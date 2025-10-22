using Nossa_TV.Models;
using Nossa_TV.ViewModels;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Nossa_TV.Services
{
    /// <summary>
    /// Serviço para gerenciamento de mensagens usando Back4App REST API
    /// </summary>
    public class MessageService : IMessageService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILeadService _leadService;

        public MessageService(IHttpClientFactory httpClientFactory, ILeadService leadService)
        {
            _httpClientFactory = httpClientFactory;
            _leadService = leadService;
        }

        public async Task<bool> SendMessageAsync(SendMessageViewModel model, string? userId = null)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("Back4App");
                
                var message = new
                {
                    userId,
                    senderName = model.SenderName,
                    senderEmail = model.SenderEmail,
                    subject = model.Subject,
                    messageContent = model.MessageContent,
                    status = MessageStatus.New.ToString(),
                    isRead = false
                };

                var json = JsonSerializer.Serialize(message);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                var response = await client.PostAsync("classes/Message", content);
                
                if (response.IsSuccessStatusCode)
                {
                    // Capturar ou atualizar lead
                    await _leadService.CaptureLeadAsync(model.SenderName, model.SenderEmail, model.Phone);
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<AdminMessageListViewModel> GetMessagesAsync(string? statusFilter = null, DateTime? dateFilter = null, int page = 1, int pageSize = 20)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("Back4App");
                
                var where = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(statusFilter))
                {
                    where["status"] = statusFilter;
                }
                if (dateFilter.HasValue)
                {
                    var startOfDay = dateFilter.Value.Date;
                    var endOfDay = startOfDay.AddDays(1);
                    where["createdAt"] = new
                    {
                        __op = "GreaterThanOrEqualTo",
                        __iso = startOfDay.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
                    };
                }

                var skip = (page - 1) * pageSize;
                var queryString = $"?order=-createdAt&limit={pageSize}&skip={skip}";
                if (where.Count > 0)
                {
                    queryString += $"&where={Uri.EscapeDataString(JsonSerializer.Serialize(where))}";
                }

                var response = await client.GetAsync($"classes/Message{queryString}");
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<ParseResponse<Message>>(jsonResponse);

                // Buscar estatísticas
                var totalCount = result?.Results?.Count ?? 0;
                var unreadCount = await GetCountAsync("Message", new { isRead = false });
                var newCount = await GetCountAsync("Message", new { status = MessageStatus.New.ToString() });
                var repliedCount = await GetCountAsync("Message", new { status = MessageStatus.Replied.ToString() });

                var messages = result?.Results?.Select(m => new MessageListItemViewModel
                {
                    Id = m.ObjectId ?? "",
                    SenderName = m.SenderName,
                    SenderEmail = m.SenderEmail,
                    Subject = m.Subject,
                    Status = m.Status,
                    IsRead = m.IsRead,
                    CreatedAt = m.CreatedAt ?? DateTime.Now,
                    RepliedAt = m.RepliedAt
                }).ToList() ?? new List<MessageListItemViewModel>();

                return new AdminMessageListViewModel
                {
                    Messages = messages,
                    TotalMessages = totalCount,
                    UnreadMessages = unreadCount,
                    NewMessages = newCount,
                    RepliedMessages = repliedCount,
                    StatusFilter = statusFilter,
                    DateFilter = dateFilter,
                    CurrentPage = page,
                    PageSize = pageSize
                };
            }
            catch
            {
                return new AdminMessageListViewModel();
            }
        }

        public async Task<AdminMessageDetailViewModel?> GetMessageDetailAsync(string messageId)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("Back4App");
                
                var response = await client.GetAsync($"classes/Message/{messageId}");
                if (!response.IsSuccessStatusCode) return null;

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var message = JsonSerializer.Deserialize<Message>(jsonResponse);
                if (message == null) return null;

                // Buscar respostas
                var repliesWhere = new { messageId };
                var repliesQuery = $"?where={Uri.EscapeDataString(JsonSerializer.Serialize(repliesWhere))}&order=createdAt";
                var repliesResponse = await client.GetAsync($"classes/MessageReply{repliesQuery}");
                var repliesJson = await repliesResponse.Content.ReadAsStringAsync();
                var repliesResult = JsonSerializer.Deserialize<ParseResponse<MessageReply>>(repliesJson);

                var replyViewModels = repliesResult?.Results?.Select(r => new MessageReplyViewModel
                {
                    Id = r.ObjectId ?? "",
                    AdminUserId = r.AdminUserId,
                    ReplyContent = r.ReplyContent,
                    CreatedAt = r.CreatedAt ?? DateTime.Now
                }).ToList() ?? new List<MessageReplyViewModel>();

                return new AdminMessageDetailViewModel
                {
                    Id = message.ObjectId ?? "",
                    UserId = message.UserId,
                    SenderName = message.SenderName,
                    SenderEmail = message.SenderEmail,
                    Subject = message.Subject,
                    MessageContent = message.MessageContent,
                    Status = message.Status,
                    IsRead = message.IsRead,
                    CreatedAt = message.CreatedAt ?? DateTime.Now,
                    ReadAt = message.ReadAt,
                    RepliedAt = message.RepliedAt,
                    Replies = replyViewModels
                };
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> ReplyToMessageAsync(string messageId, string replyContent, string adminUserId)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("Back4App");

                // Criar resposta
                var reply = new
                {
                    messageId,
                    adminUserId,
                    replyContent
                };

                var replyJson = JsonSerializer.Serialize(reply);
                var replyResponse = await client.PostAsync("classes/MessageReply", 
                    new StringContent(replyJson, Encoding.UTF8, "application/json"));

                if (!replyResponse.IsSuccessStatusCode) return false;

                // Atualizar mensagem
                var update = new
                {
                    status = MessageStatus.Replied.ToString(),
                    repliedAt = new
                    {
                        __type = "Date",
                        iso = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
                    }
                };

                var updateJson = JsonSerializer.Serialize(update);
                var updateResponse = await client.PutAsync($"classes/Message/{messageId}",
                    new StringContent(updateJson, Encoding.UTF8, "application/json"));

                return updateResponse.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> MarkAsReadAsync(string messageId)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("Back4App");

                var update = new
                {
                    isRead = true,
                    readAt = new
                    {
                        __type = "Date",
                        iso = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
                    },
                    status = MessageStatus.Read.ToString()
                };

                var json = JsonSerializer.Serialize(update);
                var response = await client.PutAsync($"classes/Message/{messageId}",
                    new StringContent(json, Encoding.UTF8, "application/json"));

                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ArchiveMessageAsync(string messageId)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("Back4App");

                var update = new { status = MessageStatus.Archived.ToString() };
                var json = JsonSerializer.Serialize(update);
                var response = await client.PutAsync($"classes/Message/{messageId}",
                    new StringContent(json, Encoding.UTF8, "application/json"));

                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteMessageAsync(string messageId)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("Back4App");

                // Deletar respostas associadas
                var repliesWhere = new { messageId };
                var repliesQuery = $"?where={Uri.EscapeDataString(JsonSerializer.Serialize(repliesWhere))}";
                var repliesResponse = await client.GetAsync($"classes/MessageReply{repliesQuery}");
                
                if (repliesResponse.IsSuccessStatusCode)
                {
                    var repliesJson = await repliesResponse.Content.ReadAsStringAsync();
                    var repliesResult = JsonSerializer.Deserialize<ParseResponse<MessageReply>>(repliesJson);
                    
                    if (repliesResult?.Results != null)
                    {
                        foreach (var reply in repliesResult.Results)
                        {
                            await client.DeleteAsync($"classes/MessageReply/{reply.ObjectId}");
                        }
                    }
                }

                var response = await client.DeleteAsync($"classes/Message/{messageId}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<UserMessageHistoryViewModel> GetUserMessagesAsync(string userId)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("Back4App");
                
                var where = new { userId };
                var queryString = $"?where={Uri.EscapeDataString(JsonSerializer.Serialize(where))}&order=-createdAt";
                
                var response = await client.GetAsync($"classes/Message{queryString}");
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<ParseResponse<Message>>(jsonResponse);

                var messages = new List<UserMessageItemViewModel>();

                if (result?.Results != null)
                {
                    foreach (var message in result.Results)
                    {
                        var repliesWhere = new { messageId = message.ObjectId };
                        var repliesQuery = $"?where={Uri.EscapeDataString(JsonSerializer.Serialize(repliesWhere))}&count=1&limit=0";
                        var repliesResponse = await client.GetAsync($"classes/MessageReply{repliesQuery}");
                        var repliesJson = await repliesResponse.Content.ReadAsStringAsync();
                        var repliesCount = JsonSerializer.Deserialize<ParseCountResponse>(repliesJson);

                        messages.Add(new UserMessageItemViewModel
                        {
                            Id = message.ObjectId ?? "",
                            Subject = message.Subject,
                            MessageContent = message.MessageContent,
                            Status = message.Status,
                            CreatedAt = message.CreatedAt ?? DateTime.Now,
                            RepliedAt = message.RepliedAt,
                            HasReplies = (repliesCount?.Count ?? 0) > 0,
                            ReplyCount = repliesCount?.Count ?? 0
                        });
                    }
                }

                return new UserMessageHistoryViewModel
                {
                    Messages = messages,
                    TotalMessages = messages.Count,
                    UnreadReplies = messages.Count(m => m.HasReplies && m.Status != MessageStatus.Read.ToString())
                };
            }
            catch
            {
                return new UserMessageHistoryViewModel();
            }
        }

        public async Task<Dictionary<string, int>> GetDashboardStatsAsync()
        {
            var stats = new Dictionary<string, int>
            {
                ["Total"] = await GetCountAsync("Message", null),
                ["Unread"] = await GetCountAsync("Message", new { isRead = false }),
                ["New"] = await GetCountAsync("Message", new { status = MessageStatus.New.ToString() }),
                ["Replied"] = await GetCountAsync("Message", new { status = MessageStatus.Replied.ToString() }),
                ["Archived"] = await GetCountAsync("Message", new { status = MessageStatus.Archived.ToString() })
            };

            return stats;
        }

        private async Task<int> GetCountAsync(string className, object? where)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("Back4App");
                var queryString = "?count=1&limit=0";
                
                if (where != null)
                {
                    queryString += $"&where={Uri.EscapeDataString(JsonSerializer.Serialize(where))}";
                }

                var response = await client.GetAsync($"classes/{className}{queryString}");
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<ParseCountResponse>(json);
                
                return result?.Count ?? 0;
            }
            catch
            {
                return 0;
            }
        }
    }

    // Classes auxiliares para desserialização
    public class ParseResponse<T>
    {
        [JsonPropertyName("results")]
        public List<T>? Results { get; set; }
    }

    public class ParseCountResponse
    {
        [JsonPropertyName("count")]
        public int Count { get; set; }
    }
}
