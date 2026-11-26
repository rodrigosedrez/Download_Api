using Download.Domain.Entities;

namespace Download.Domain.Interfaces
{
    public interface IEmulatorRepository
    {
        Task<EmulatorsModel> AddEmulatorAsync(EmulatorsModel emulatorsModel);
        Task<EmulatorsModel> UpdateEmulatorAsync(EmulatorsModel emulatorsModel);
        Task<EmulatorsModel> DeleteEmulatorAsync(int id);
        Task<EmulatorsModel?> GetEmulatorByIdAsync(int id);
        Task<EmulatorsModel?> GetEmulatorBySlugAsync(string slug);
        Task<IEnumerable<EmulatorsModel>> GetAllEmulatorsAsync();
        Task<IEnumerable<EmulatorsModel>> GetEmulatorsByPlataformIdAsync(int plataformId);
    }
}
