using Nossa_TV.Models;
using Nossa_TV.ViewModels;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Nossa_TV.Services
{
    /// <summary>
    /// Serviço para gerenciamento de leads usando Back4App REST API
    /// </summary>
    public class LeadService : ILeadService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public LeadService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<bool> CaptureLeadAsync(string name, string email, string? phone = null)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("Back4App");

                // Verificar se o lead já existe
                var where = new { email };
                var queryString = $"?where={Uri.EscapeDataString(JsonSerializer.Serialize(where))}";
                
                var response = await client.GetAsync($"classes/Lead{queryString}");
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<ParseResponse<Lead>>(jsonResponse);

                if (result?.Results != null && result.Results.Any())
                {
                    // Atualizar lead existente
                    var existingLead = result.Results.First();
                    var update = new
                    {
                        lastContactDate = new
                        {
                            __type = "Date",
                            iso = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
                        },
                        messageCount = existingLead.MessageCount + 1,
                        phone = !string.IsNullOrEmpty(phone) && string.IsNullOrEmpty(existingLead.Phone) ? phone : existingLead.Phone
                    };

                    var updateJson = JsonSerializer.Serialize(update);
                    await client.PutAsync($"classes/Lead/{existingLead.ObjectId}",
                        new StringContent(updateJson, Encoding.UTF8, "application/json"));
                }
                else
                {
                    // Criar novo lead
                    var newLead = new
                    {
                        name,
                        email,
                        phone,
                        firstContactDate = new
                        {
                            __type = "Date",
                            iso = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
                        },
                        lastContactDate = new
                        {
                            __type = "Date",
                            iso = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
                        },
                        messageCount = 1,
                        tags = new List<string>()
                    };

                    var json = JsonSerializer.Serialize(newLead);
                    await client.PostAsync("classes/Lead",
                        new StringContent(json, Encoding.UTF8, "application/json"));
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<LeadListViewModel> GetLeadsAsync(string? searchTerm = null, string? tagFilter = null, int page = 1, int pageSize = 20)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("Back4App");
                
                var skip = (page - 1) * pageSize;
                var queryString = $"?order=-lastContactDate&limit={pageSize}&skip={skip}";

                // Aplicar filtros se necessário
                if (!string.IsNullOrEmpty(searchTerm) || !string.IsNullOrEmpty(tagFilter))
                {
                    var where = new Dictionary<string, object>();
                    
                    if (!string.IsNullOrEmpty(searchTerm))
                    {
                        // Busca simples por email (Back4App suporta regex)
                        where["email"] = new
                        {
                            __regex = searchTerm,
                            __options = "i"
                        };
                    }

                    if (!string.IsNullOrEmpty(tagFilter))
                    {
                        where["tags"] = tagFilter;
                    }

                    if (where.Count > 0)
                    {
                        queryString += $"&where={Uri.EscapeDataString(JsonSerializer.Serialize(where))}";
                    }
                }

                var response = await client.GetAsync($"classes/Lead{queryString}");
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<ParseResponse<Lead>>(jsonResponse);

                // Contar total
                var countQuery = "?count=1&limit=0";
                var countResponse = await client.GetAsync($"classes/Lead{countQuery}");
                var countJson = await countResponse.Content.ReadAsStringAsync();
                var countResult = JsonSerializer.Deserialize<ParseCountResponse>(countJson);

                var leads = result?.Results?.Select(l => new LeadItemViewModel
                {
                    Id = l.ObjectId ?? "",
                    Name = l.Name,
                    Email = l.Email,
                    Phone = l.Phone,
                    FirstContactDate = l.FirstContactDate,
                    LastContactDate = l.LastContactDate,
                    MessageCount = l.MessageCount,
                    Tags = l.Tags
                }).ToList() ?? new List<LeadItemViewModel>();

                return new LeadListViewModel
                {
                    Leads = leads,
                    TotalLeads = countResult?.Count ?? 0,
                    CurrentPage = page,
                    PageSize = pageSize,
                    SearchTerm = searchTerm,
                    TagFilter = tagFilter
                };
            }
            catch
            {
                return new LeadListViewModel();
            }
        }

        public async Task<LeadDetailViewModel?> GetLeadDetailAsync(string leadId)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("Back4App");
                
                var response = await client.GetAsync($"classes/Lead/{leadId}");
                if (!response.IsSuccessStatusCode) return null;

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var lead = JsonSerializer.Deserialize<Lead>(jsonResponse);
                if (lead == null) return null;

                // Buscar mensagens do lead
                var messagesWhere = new { senderEmail = lead.Email };
                var messagesQuery = $"?where={Uri.EscapeDataString(JsonSerializer.Serialize(messagesWhere))}&order=-createdAt";
                
                var messagesResponse = await client.GetAsync($"classes/Message{messagesQuery}");
                var messagesJson = await messagesResponse.Content.ReadAsStringAsync();
                var messagesResult = JsonSerializer.Deserialize<ParseResponse<Message>>(messagesJson);

                var messageViewModels = new List<LeadMessageViewModel>();

                if (messagesResult?.Results != null)
                {
                    foreach (var message in messagesResult.Results)
                    {
                        var repliesWhere = new { messageId = message.ObjectId };
                        var repliesQuery = $"?where={Uri.EscapeDataString(JsonSerializer.Serialize(repliesWhere))}&count=1&limit=0";
                        var repliesResponse = await client.GetAsync($"classes/MessageReply{repliesQuery}");
                        var repliesJson = await repliesResponse.Content.ReadAsStringAsync();
                        var repliesCount = JsonSerializer.Deserialize<ParseCountResponse>(repliesJson);

                        messageViewModels.Add(new LeadMessageViewModel
                        {
                            Id = message.ObjectId ?? "",
                            Subject = message.Subject,
                            MessageContent = message.MessageContent,
                            Status = message.Status,
                            CreatedAt = message.CreatedAt ?? DateTime.Now,
                            ReplyCount = repliesCount?.Count ?? 0
                        });
                    }
                }

                return new LeadDetailViewModel
                {
                    Id = lead.ObjectId ?? "",
                    Name = lead.Name,
                    Email = lead.Email,
                    Phone = lead.Phone,
                    FirstContactDate = lead.FirstContactDate,
                    LastContactDate = lead.LastContactDate,
                    MessageCount = lead.MessageCount,
                    Tags = lead.Tags ?? new List<string>(),
                    Notes = lead.Notes,
                    Messages = messageViewModels
                };
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> AddTagAsync(string leadId, string tag)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("Back4App");

                // Buscar lead atual
                var response = await client.GetAsync($"classes/Lead/{leadId}");
                if (!response.IsSuccessStatusCode) return false;

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var lead = JsonSerializer.Deserialize<Lead>(jsonResponse);
                if (lead == null) return false;

                var tags = lead.Tags ?? new List<string>();
                
                if (!tags.Contains(tag))
                {
                    tags.Add(tag);
                    var update = new { tags };
                    var updateJson = JsonSerializer.Serialize(update);
                    
                    var updateResponse = await client.PutAsync($"classes/Lead/{leadId}",
                        new StringContent(updateJson, Encoding.UTF8, "application/json"));
                    
                    return updateResponse.IsSuccessStatusCode;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RemoveTagAsync(string leadId, string tag)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("Back4App");

                // Buscar lead atual
                var response = await client.GetAsync($"classes/Lead/{leadId}");
                if (!response.IsSuccessStatusCode) return false;

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var lead = JsonSerializer.Deserialize<Lead>(jsonResponse);
                if (lead == null) return false;

                var tags = lead.Tags ?? new List<string>();
                
                if (tags.Contains(tag))
                {
                    tags.Remove(tag);
                    var update = new { tags };
                    var updateJson = JsonSerializer.Serialize(update);
                    
                    var updateResponse = await client.PutAsync($"classes/Lead/{leadId}",
                        new StringContent(updateJson, Encoding.UTF8, "application/json"));
                    
                    return updateResponse.IsSuccessStatusCode;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateNotesAsync(string leadId, string notes)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("Back4App");

                var update = new { notes };
                var json = JsonSerializer.Serialize(update);
                
                var response = await client.PutAsync($"classes/Lead/{leadId}",
                    new StringContent(json, Encoding.UTF8, "application/json"));

                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<byte[]> ExportLeadsAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient("Back4App");
                
                var queryString = "?order=-lastContactDate&limit=1000";
                var response = await client.GetAsync($"classes/Lead{queryString}");
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<ParseResponse<Lead>>(jsonResponse);

                var csv = new StringBuilder();
                csv.AppendLine("Nome,Email,Telefone,Primeira Interação,Última Interação,Total de Mensagens,Tags");

                if (result?.Results != null)
                {
                    foreach (var lead in result.Results)
                    {
                        var name = lead.Name.Replace("\"", "\"\"");
                        var email = lead.Email.Replace("\"", "\"\"");
                        var phone = (lead.Phone ?? "").Replace("\"", "\"\"");
                        var firstContact = lead.FirstContactDate.ToString("dd/MM/yyyy");
                        var lastContact = lead.LastContactDate.ToString("dd/MM/yyyy");
                        var messageCount = lead.MessageCount;
                        var tags = lead.Tags != null ? string.Join(";", lead.Tags).Replace("\"", "\"\"") : "";

                        csv.AppendLine($"\"{name}\",\"{email}\",\"{phone}\",\"{firstContact}\",\"{lastContact}\",{messageCount},\"{tags}\"");
                    }
                }

                return Encoding.UTF8.GetBytes(csv.ToString());
            }
            catch
            {
                return Encoding.UTF8.GetBytes("Erro ao exportar leads");
            }
        }
    }
}
