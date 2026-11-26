using Download.Domain.Entities;  

namespace Download.Domain.Interfaces
{
    public interface IMagazineRepository
    {
        Task<MagazineModel> AddMagazineAsync(MagazineModel magazineModel);
        Task<MagazineModel> UpdateMagazineAsync(MagazineModel magazineModel);
        Task<MagazineModel> DeleteMagazineAsync(int id);
        Task<MagazineModel?> GetMagazineByIdAsync(int id);
        Task<IEnumerable<MagazineModel>> GetAllMagazinesAsync();
    }
}
