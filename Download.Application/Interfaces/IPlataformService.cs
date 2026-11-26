
using Download.Application.DTOs;

namespace Download.Application.Interfaces
{
    public interface IPlataformService
    {
        Task<PlataformModelsDTO> AddPlataformAsync(PlataformModelsDTO plataformModelDTO);
        Task<PlataformModelsDTO> UpdatePlataformAsync(PlataformModelsDTO plataformModelDTO);
        Task<PlataformModelsDTO> DeletePlataformAsync(int id);
        Task<PlataformModelsDTO?> GetPlataformByIdAsync(int id);
        Task<IEnumerable<PlataformModelsDTO>> GetAllPlataformsAsync();
        Task<PlataformModelsDTO?> IncrementDownloadCountPlataformAsync(int id);
    }
}
                