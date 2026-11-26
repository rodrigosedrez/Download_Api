using Download.Application.DTOs;

namespace Download.Application.Interfaces
{
    public interface IEmulatorService
    {
        Task<EmulatorsModelDTO> AddEmulatorAsync(EmulatorsModelDTO emulatorsModelDTO);

        Task<EmulatorsModelDTO> UpdateEmulatorAsync(EmulatorsModelDTO emulatorsModelDTO);

        Task<EmulatorsModelDTO> DeleteEmulatorAsync(int id);

        Task<EmulatorsModelDTO?> GetEmulatorByIdAsync(int id);
        Task<EmulatorsModelDTO?> GetEmulatorBySlugAsync(string slug);

        Task<IEnumerable<EmulatorsModelDTO>> GetAllEmulatorsAsync();
        Task<IEnumerable<EmulatorsModelDTO>> GetEmulatorsByPlataformIdAsync(int plataformId);
    }
}