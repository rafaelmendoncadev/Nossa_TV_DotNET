namespace Nossa_TV.ViewModels
{
    /// <summary>
    /// ViewModel para detalhes de um lead
    /// </summary>
    public class LeadDetailViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public DateTime FirstContactDate { get; set; }
        public DateTime LastContactDate { get; set; }
        public int MessageCount { get; set; }
        public List<string>? Tags { get; set; }
        public string? Notes { get; set; }
        
        public List<LeadMessageViewModel> Messages { get; set; } = new();
    }

    public class LeadMessageViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string MessageContent { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int ReplyCount { get; set; }
    }
}
