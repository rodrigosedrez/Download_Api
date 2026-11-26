
using Download.Domain.Entities;

namespace Download.Domain.Interfaces
{
    public interface IPublicityRepository
    {
        Task<Publicity> AddPublicityAsync(Publicity publicity);
        Task<Publicity> UpdatePublicityAsync(Publicity publicity);
        Task<Publicity> DeletePublicityAsync(int id);
        Task<Publicity?> GetPublicityByIdAsync(int id);
        Task<IEnumerable<Publicity>> GetAllPublicityAsync();
    }
}
