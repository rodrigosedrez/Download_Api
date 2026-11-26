

using Download.Domain.Entities;
using Download.Domain.Interfaces;
using Download.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Download.Infrastructure.Repositories
{
    public class MisturebaRepository(DownloadsDBContext context) : IMisturebaRepository
    {
        public readonly DownloadsDBContext _context = context;

        public async Task<MisturebaModel> AddMisturebaAsync(MisturebaModel mistureba)
        {
          await _context.MisturebaModels.AddAsync(mistureba);
           await _context.SaveChangesAsync();
            return mistureba;
        }
        public async Task<MisturebaModel> UpdateMisturebaAsync(MisturebaModel mistureba)
        {
            _context.MisturebaModels.Update(mistureba);
            await _context.SaveChangesAsync();
            return mistureba;
        }
        public async Task<MisturebaModel> DeleteMisturebaAsync(int id)
        {
            var mistureba = await _context.MisturebaModels.FindAsync(id);
            if (mistureba != null)
            {
                _context.MisturebaModels.Remove(mistureba);
                await _context.SaveChangesAsync();
            }
            return mistureba!;
        }
        public async Task<MisturebaModel> GetMisturebaByIdAsync(int id)
        {
            var mistureba = await _context.MisturebaModels.FindAsync(id);
            return mistureba!;
        }

        public async Task<IEnumerable<MisturebaModel>> GetAllMisturebasAsync()
        {
            return await _context.MisturebaModels.ToListAsync();
        }
    }
}
