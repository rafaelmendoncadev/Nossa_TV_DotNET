using Nossa_TV.Models;
using Nossa_TV.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;

namespace Nossa_TV.Services
{
    /// <summary>
    /// Serviço para gerenciamento de leads usando EF Core (SQLite)
    /// </summary>
    public class LeadService : ILeadService
    {
        private readonly Data.ApplicationDbContext _db;

        public LeadService(Data.ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<bool> CaptureLeadAsync(string name, string email, string? phone = null)
        {
            try
            {
                var existing = await _db.Leads.FirstOrDefaultAsync(l => l.Email == email);

                if (existing != null)
                {
                    existing.LastContactDate = DateTime.UtcNow;
                    existing.MessageCount = existing.MessageCount + 1;
                    if (!string.IsNullOrEmpty(phone) && string.IsNullOrEmpty(existing.Phone))
                        existing.Phone = phone;

                    _db.Leads.Update(existing);
                }
                else
                {
                    var newLead = new Lead
                    {
                        Name = name,
                        Email = email,
                        Phone = phone,
                        FirstContactDate = DateTime.UtcNow,
                        LastContactDate = DateTime.UtcNow,
                        MessageCount = 1,
                        Tags = new List<string>(),
                        CreatedAt = DateTime.UtcNow
                    };

                    _db.Leads.Add(newLead);
                }

                await _db.SaveChangesAsync();
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
                var query = _db.Leads.AsQueryable();

                if (!string.IsNullOrEmpty(searchTerm))
                    query = query.Where(l => l.Email.Contains(searchTerm) || l.Name.Contains(searchTerm));

                if (!string.IsNullOrEmpty(tagFilter))
                    query = query.Where(l => l.Tags != null && l.Tags.Contains(tagFilter));

                query = query.OrderByDescending(l => l.LastContactDate);

                var total = await query.CountAsync();

                var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

                var leads = items.Select(l => new LeadItemViewModel
                {
                    Id = l.Id,
                    Name = l.Name,
                    Email = l.Email,
                    Phone = l.Phone,
                    FirstContactDate = l.FirstContactDate,
                    LastContactDate = l.LastContactDate,
                    MessageCount = l.MessageCount,
                    Tags = l.Tags
                }).ToList();

                return new LeadListViewModel
                {
                    Leads = leads,
                    TotalLeads = total,
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
                var lead = await _db.Leads.FindAsync(leadId);
                if (lead == null) return null;

                var messages = await _db.Messages.Where(m => m.SenderEmail == lead.Email).OrderByDescending(m => m.CreatedAt).ToListAsync();

                var messageViewModels = new List<LeadMessageViewModel>();

                foreach (var message in messages)
                {
                    var replyCount = await _db.MessageReplies.CountAsync(r => r.MessageId == message.Id);

                    messageViewModels.Add(new LeadMessageViewModel
                    {
                        Id = message.Id,
                        Subject = message.Subject,
                        MessageContent = message.MessageContent,
                        Status = message.Status,
                        CreatedAt = message.CreatedAt,
                        ReplyCount = replyCount
                    });
                }

                return new LeadDetailViewModel
                {
                    Id = lead.Id,
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
                var lead = await _db.Leads.FindAsync(leadId);
                if (lead == null) return false;

                var tags = lead.Tags ?? new List<string>();
                if (!tags.Contains(tag))
                {
                    tags.Add(tag);
                    lead.Tags = tags;
                    _db.Leads.Update(lead);
                    await _db.SaveChangesAsync();
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
                var lead = await _db.Leads.FindAsync(leadId);
                if (lead == null) return false;

                var tags = lead.Tags ?? new List<string>();
                if (tags.Contains(tag))
                {
                    tags.Remove(tag);
                    lead.Tags = tags;
                    _db.Leads.Update(lead);
                    await _db.SaveChangesAsync();
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
                var lead = await _db.Leads.FindAsync(leadId);
                if (lead == null) return false;

                lead.Notes = notes;
                _db.Leads.Update(lead);
                await _db.SaveChangesAsync();

                return true;
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
                var leads = await _db.Leads.OrderByDescending(l => l.LastContactDate).Take(1000).ToListAsync();

                var csv = new StringBuilder();
                csv.AppendLine("Nome,Email,Telefone,Primeira Interação,Última Interação,Total de Mensagens,Tags");

                foreach (var lead in leads)
                {
                    var name = (lead.Name ?? "").Replace("\"", "\"\"");
                    var email = (lead.Email ?? "").Replace("\"", "\"\"");
                    var phone = (lead.Phone ?? "").Replace("\"", "\"\"");
                    var firstContact = lead.FirstContactDate.ToString("dd/MM/yyyy");
                    var lastContact = lead.LastContactDate.ToString("dd/MM/yyyy");
                    var messageCount = lead.MessageCount;
                    var tags = lead.Tags != null ? string.Join(";", lead.Tags).Replace("\"", "\"\"") : "";

                    csv.AppendLine($"\"{name}\",\"{email}\",\"{phone}\",\"{firstContact}\",\"{lastContact}\",{messageCount},\"{tags}\"");
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
