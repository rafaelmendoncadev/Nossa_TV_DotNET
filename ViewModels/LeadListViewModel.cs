namespace Nossa_TV.ViewModels
{
    /// <summary>
    /// ViewModel para listagem de leads no painel admin
    /// </summary>
    public class LeadListViewModel
    {
        public List<LeadItemViewModel> Leads { get; set; } = new();
        public int TotalLeads { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public int TotalPages => (int)Math.Ceiling((double)TotalLeads / PageSize);
        public string? SearchTerm { get; set; }
        public string? TagFilter { get; set; }
    }

    public class LeadItemViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public DateTime FirstContactDate { get; set; }
        public DateTime LastContactDate { get; set; }
        public int MessageCount { get; set; }
        public List<string>? Tags { get; set; }
    }
}
