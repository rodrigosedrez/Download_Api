using Download.Application.DTOs;

namespace Download.Application.Interfaces
{
    public interface IExternalLinksService
    {
        Task<ExternalLinksModelDTO> AddExternalLinkAsync(ExternalLinksModelDTO externalLinksModelDTO);
        Task<ExternalLinksModelDTO> UpdateExternalLinkAsync(ExternalLinksModelDTO externalLinksModelDTO);
        Task<ExternalLinksModelDTO> DeleteExternalLinkAsync(int id);
        Task<ExternalLinksModelDTO?> GetExternalLinkByIdAsync(int id);
        Task<IEnumerable<ExternalLinksModelDTO>> GetAllExternalLinksAsync();
    }
}
