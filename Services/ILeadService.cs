using Nossa_TV.ViewModels;

namespace Nossa_TV.Services
{
    /// <summary>
    /// Interface para o serviço de leads
    /// </summary>
    public interface ILeadService
    {
        Task<bool> CaptureLeadAsync(string name, string email, string? phone = null);
        Task<LeadListViewModel> GetLeadsAsync(string? searchTerm = null, string? tagFilter = null, int page = 1, int pageSize = 20);
        Task<LeadDetailViewModel?> GetLeadDetailAsync(string leadId);
        Task<bool> AddTagAsync(string leadId, string tag);
        Task<bool> RemoveTagAsync(string leadId, string tag);
        Task<bool> UpdateNotesAsync(string leadId, string notes);
        Task<byte[]> ExportLeadsAsync();
    }
}
