using Download.Domain.Entities;

namespace Download.Domain.Interfaces
{
    public interface IExternalLinksRepository
    {
        Task<ExternalLinksModel> AddExternalLinkAsync(ExternalLinksModel externalLinksModel);
        Task<ExternalLinksModel> UpdateExternalLinkAsync(ExternalLinksModel externalLinksModel);
        Task<ExternalLinksModel> DeleteExternalLinkAsync(int id);
        Task<ExternalLinksModel?> GetExternalLinkByIdAsync(int id);
        Task<IEnumerable<ExternalLinksModel>> GetAllExternalLinksAsync();
    }
}
