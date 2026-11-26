using Download.Domain.Entities;
using Download.Domain.Interfaces;
using Download.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Download.Infrastructure.Repositories
{
    public class PublicityRepository(DownloadsDBContext context) : IPublicityRepository
    {
        private readonly DownloadsDBContext _Context = context;

        public async Task<Publicity> AddPublicityAsync(Publicity publicity)
        {
            await _Context.Publicitys.AddAsync(publicity);
            await _Context.SaveChangesAsync();
            return publicity;
        }

        public async Task<Publicity> UpdatePublicityAsync(Publicity publicity)
        {
            _Context.Publicitys.Update(publicity);
            await _Context.SaveChangesAsync();
            return publicity;
        }

        public async Task<Publicity> DeletePublicityAsync(int id)
        {
            var publicity = await _Context.Publicitys.FindAsync(id);
            if (publicity == null)
                return null!;
            _Context.Publicitys.Remove(publicity);
            await _Context.SaveChangesAsync();
            return publicity;
        }

        public async Task<Publicity?> GetPublicityByIdAsync(int id)
        {
            var publicity = await _Context.Publicitys.FindAsync(id);
            return publicity;
        }

        public async Task<IEnumerable<Publicity>> GetAllPublicityAsync()
        {
            var result = await _Context.Publicitys.ToListAsync();
            return result;
        }
    }
}