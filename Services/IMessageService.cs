using Nossa_TV.Models;
using Nossa_TV.ViewModels;

namespace Nossa_TV.Services
{
    /// <summary>
    /// Interface para o serviço de mensagens
    /// </summary>
    public interface IMessageService
    {
        Task<bool> SendMessageAsync(SendMessageViewModel model, string? userId = null);
        Task<AdminMessageListViewModel> GetMessagesAsync(string? statusFilter = null, DateTime? dateFilter = null, int page = 1, int pageSize = 20);
        Task<AdminMessageDetailViewModel?> GetMessageDetailAsync(string messageId);
        Task<bool> ReplyToMessageAsync(string messageId, string replyContent, string adminUserId);
        Task<bool> MarkAsReadAsync(string messageId);
        Task<bool> ArchiveMessageAsync(string messageId);
        Task<bool> DeleteMessageAsync(string messageId);
        Task<UserMessageHistoryViewModel> GetUserMessagesAsync(string userId);
        Task<Dictionary<string, int>> GetDashboardStatsAsync();
    }
}
