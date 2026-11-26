using Download.Domain.Entities;
using Download.Domain.Interfaces;
using Download.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
namespace Download.Infrastructure.Repositories
{
    public class MagazineRepository(DownloadsDBContext context) : IMagazineRepository
    {
        private readonly DownloadsDBContext _context = context;
        
        public async Task<MagazineModel> AddMagazineAsync(MagazineModel magazineModel)
        {
            await _context.MagazineModels.AddAsync(magazineModel);
            await _context.SaveChangesAsync();
            return magazineModel;
        }
        public async Task<MagazineModel> UpdateMagazineAsync(MagazineModel magazineModel)
        {
            _context.MagazineModels.Update(magazineModel);
            await _context.SaveChangesAsync();
            return magazineModel;
        }
        public async Task<MagazineModel> DeleteMagazineAsync(int id)
        {
            var magazine = await _context.MagazineModels.FindAsync(id);
            if (magazine != null)
            {
                _context.MagazineModels.Remove(magazine);
                await _context.SaveChangesAsync();
            }
            return magazine!;
        }

        public async Task<MagazineModel?> GetMagazineByIdAsync(int id)
        {
            return await _context.MagazineModels.FindAsync(id);
        }


        public async Task<IEnumerable<MagazineModel>> GetAllMagazinesAsync()
        {
            return await _context.MagazineModels.ToListAsync();
        }
    }
}
