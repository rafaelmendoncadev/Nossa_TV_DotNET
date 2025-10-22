namespace Nossa_TV.ViewModels
{
    /// <summary>
    /// ViewModel para histórico de mensagens do usuário
    /// </summary>
    public class UserMessageHistoryViewModel
    {
        public List<UserMessageItemViewModel> Messages { get; set; } = new();
        public int TotalMessages { get; set; }
        public int UnreadReplies { get; set; }
    }

    public class UserMessageItemViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string MessageContent { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? RepliedAt { get; set; }
        public bool HasReplies { get; set; }
        public int ReplyCount { get; set; }
    }
}
