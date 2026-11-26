using Download.Domain.Entities;
using Download.Domain.Interfaces;
using Download.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;


namespace Download.Infrastructure.Repositories
{
    public class EmulatorRepository(DownloadsDBContext context) : IEmulatorRepository
    {
        public readonly DownloadsDBContext _context = context;

        public async Task<EmulatorsModel> AddEmulatorAsync(EmulatorsModel emulatorsModel)
        {
            await  _context.Emulators.AddAsync(emulatorsModel);
            await _context.SaveChangesAsync();
            return emulatorsModel;
        }

        public async Task<EmulatorsModel> UpdateEmulatorAsync(EmulatorsModel emulatorsModel)
        {
            var existing = await _context.Emulators.FindAsync(emulatorsModel);
            if (existing == null)
                return null!;

            _context.Update(existing);
            await _context.SaveChangesAsync();

            return existing;
        }

        public async Task<EmulatorsModel> DeleteEmulatorAsync(int id)
        {
            var emulator = await _context.Emulators.FindAsync(id);
            if (emulator == null)
                return null!;

            _context.Emulators.Remove(emulator);
            await _context.SaveChangesAsync();
            return emulator;
        }

        public async Task<EmulatorsModel?> GetEmulatorByIdAsync(int id)
        {
            var emulator = await _context.Emulators.FindAsync(id);

            return emulator;
        }
        public async Task<EmulatorsModel?> GetEmulatorBySlugAsync(string slug)
        {
            var emulator = await _context.Emulators.FirstOrDefaultAsync(x => x.Slug == slug);

            return emulator;
        }

        public async Task<IEnumerable<EmulatorsModel>> GetAllEmulatorsAsync()
        {
             var result = await _context.Emulators.ToListAsync();

            return result;
        }
        public async Task<IEnumerable<EmulatorsModel>> GetEmulatorsByPlataformIdAsync(int plataformId)
        {
            var result = await _context.Emulators
                .Where(e => e.PlataformModelsId == plataformId)
                .ToListAsync();
            return result;
        }
    }
}
