

using Download.Application.DTOs;

namespace Download.Application.Interfaces
{
    public interface IMagazineService
    {
        Task<MagazineModelDTO> AddMagazineAsync(MagazineModelDTO magazineModelDTO);
        Task<MagazineModelDTO> UpdateMagazineAsync(MagazineModelDTO magazineModelDTO);
        Task<MagazineModelDTO> DeleteMagazineAsync(int id);
        Task<MagazineModelDTO?> GetMagazineByIdAsync(int id);
        Task<IEnumerable<MagazineModelDTO>> GetAllMagazinesAsync();
    }
}
