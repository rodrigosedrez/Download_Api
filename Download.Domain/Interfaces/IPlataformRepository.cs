using Download.Domain.Entities;

namespace Download.Domain.Interfaces
{
    public interface IPlataformRepository
    {
        Task<PlataformModels> AddPlataformAsync(PlataformModels plataformModel);
        Task<PlataformModels> UpdatePlataformAsync(PlataformModels plataformModel);
        Task<PlataformModels> DeletePlataformAsync(int id);
        Task<PlataformModels> GetPlataformByIdAsync(int id);
        Task<PlataformModels> GetPlataformBySlugAsync(string slug);
        Task<int> IncrementDownloadCountPlataform(int plataformID);
        Task<IEnumerable<PlataformModels>> GetAllPlataformsAsync();
    }
}
