using Nossa_TV.Models;
using Nossa_TV.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Nossa_TV.Services
{
    /// <summary>
    /// Serviço para gerenciamento de mensagens usando EF Core (SQLite)
    /// </summary>
    public class MessageService : IMessageService
    {
        private readonly Data.ApplicationDbContext _db;
        private readonly ILeadService _leadService;

        public MessageService(Data.ApplicationDbContext db, ILeadService leadService)
        {
            _db = db;
            _leadService = leadService;
        }

        public async Task<bool> SendMessageAsync(SendMessageViewModel model, string? userId = null)
        {
            try
            {
                var message = new Message
                {
                    UserId = userId,
                    SenderName = model.SenderName,
                    SenderEmail = model.SenderEmail,
                    Subject = model.Subject,
                    MessageContent = model.MessageContent,
                    Status = MessageStatus.New.ToString(),
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow
                };

                _db.Messages.Add(message);
                await _db.SaveChangesAsync();

                // Capturar ou atualizar lead
                await _leadService.CaptureLeadAsync(model.SenderName, model.SenderEmail, model.Phone);

                return true;
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
                var query = _db.Messages.AsQueryable();

                if (!string.IsNullOrEmpty(statusFilter))
                    query = query.Where(x => x.Status == statusFilter);

                if (dateFilter.HasValue)
                {
                    var start = dateFilter.Value.Date;
                    var end = start.AddDays(1);
                    query = query.Where(x => x.CreatedAt >= start && x.CreatedAt < end);
                }

                query = query.OrderByDescending(x => x.CreatedAt);

                var totalCount = await query.CountAsync();

                var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

                var messages = items.Select(m => new MessageListItemViewModel
                {
                    Id = m.Id,
                    SenderName = m.SenderName,
                    SenderEmail = m.SenderEmail,
                    Subject = m.Subject,
                    Status = m.Status,
                    IsRead = m.IsRead,
                    CreatedAt = m.CreatedAt,
                    RepliedAt = m.RepliedAt
                }).ToList();

                var unreadCount = await _db.Messages.CountAsync(x => x.IsRead == false);
                var newCount = await _db.Messages.CountAsync(x => x.Status == MessageStatus.New.ToString());
                var repliedCount = await _db.Messages.CountAsync(x => x.Status == MessageStatus.Replied.ToString());

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
                var message = await _db.Messages.FindAsync(messageId);
                if (message == null) return null;

                var replies = await _db.MessageReplies.Where(r => r.MessageId == messageId).OrderBy(r => r.CreatedAt).ToListAsync();

                var replyViewModels = replies.Select(r => new MessageReplyViewModel
                {
                    Id = r.Id,
                    AdminUserId = r.AdminUserId,
                    ReplyContent = r.ReplyContent,
                    CreatedAt = r.CreatedAt
                }).ToList();

                return new AdminMessageDetailViewModel
                {
                    Id = message.Id,
                    UserId = message.UserId,
                    SenderName = message.SenderName,
                    SenderEmail = message.SenderEmail,
                    Subject = message.Subject,
                    MessageContent = message.MessageContent,
                    Status = message.Status,
                    IsRead = message.IsRead,
                    CreatedAt = message.CreatedAt,
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
                var message = await _db.Messages.FindAsync(messageId);
                if (message == null) return false;

                var reply = new MessageReply
                {
                    MessageId = messageId,
                    AdminUserId = adminUserId,
                    ReplyContent = replyContent,
                    CreatedAt = DateTime.UtcNow
                };

                _db.MessageReplies.Add(reply);

                message.Status = MessageStatus.Replied.ToString();
                message.RepliedAt = DateTime.UtcNow;

                await _db.SaveChangesAsync();

                return true;
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
                var message = await _db.Messages.FindAsync(messageId);
                if (message == null) return false;

                message.IsRead = true;
                message.ReadAt = DateTime.UtcNow;
                message.Status = MessageStatus.Read.ToString();

                await _db.SaveChangesAsync();

                return true;
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
                var message = await _db.Messages.FindAsync(messageId);
                if (message == null) return false;

                message.Status = MessageStatus.Archived.ToString();

                await _db.SaveChangesAsync();

                return true;
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
                var replies = _db.MessageReplies.Where(x => x.MessageId == messageId);
                _db.MessageReplies.RemoveRange(replies);

                var message = await _db.Messages.FindAsync(messageId);
                if (message != null)
                {
                    _db.Messages.Remove(message);
                }

                await _db.SaveChangesAsync();

                return true;
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
                var response = await _db.Messages.Where(x => x.UserId == userId).OrderByDescending(x => x.CreatedAt).ToListAsync();

                var messages = new List<UserMessageItemViewModel>();

                foreach (var message in response)
                {
                    var replyCount = await _db.MessageReplies.CountAsync(x => x.MessageId == message.Id);

                    messages.Add(new UserMessageItemViewModel
                    {
                        Id = message.Id,
                        Subject = message.Subject,
                        MessageContent = message.MessageContent,
                        Status = message.Status,
                        CreatedAt = message.CreatedAt,
                        RepliedAt = message.RepliedAt,
                        HasReplies = replyCount > 0,
                        ReplyCount = replyCount
                    });
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
            var total = await _db.Messages.CountAsync();
            var unread = await _db.Messages.CountAsync(x => x.IsRead == false);
            var New = await _db.Messages.CountAsync(x => x.Status == MessageStatus.New.ToString());
            var replied = await _db.Messages.CountAsync(x => x.Status == MessageStatus.Replied.ToString());
            var archived = await _db.Messages.CountAsync(x => x.Status == MessageStatus.Archived.ToString());

            return new Dictionary<string, int>
            {
                ["Total"] = total,
                ["Unread"] = unread,
                ["New"] = New,
                ["Replied"] = replied,
                ["Archived"] = archived
            };
        }
    }
}
