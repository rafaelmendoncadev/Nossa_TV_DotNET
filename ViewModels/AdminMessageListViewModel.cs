using Nossa_TV.Models;

namespace Nossa_TV.ViewModels
{
    /// <summary>
    /// ViewModel para listagem de mensagens no painel admin
    /// </summary>
    public class AdminMessageListViewModel
    {
        public List<MessageListItemViewModel> Messages { get; set; } = new();
        public int TotalMessages { get; set; }
        public int UnreadMessages { get; set; }
        public int NewMessages { get; set; }
        public int RepliedMessages { get; set; }
        public string? StatusFilter { get; set; }
        public DateTime? DateFilter { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public int TotalPages => (int)Math.Ceiling((double)TotalMessages / PageSize);
    }

    public class MessageListItemViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string SenderName { get; set; } = string.Empty;
        public string SenderEmail { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? RepliedAt { get; set; }
    }
}
