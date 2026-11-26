using Download.Domain.Entities;
using Download.Domain.Interfaces;
using Download.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Download.Infrastructure.Repositories
{
    public class PlataformRepository(DownloadsDBContext context) : IPlataformRepository
    {
        private readonly DownloadsDBContext _context = context;
     
        public async Task<PlataformModels> AddPlataformAsync(PlataformModels plataformModel)
        {
            _context.PlataformModels!.Add(plataformModel);
            await _context.SaveChangesAsync();
            return plataformModel!;
        }
        public async Task<PlataformModels> UpdatePlataformAsync(PlataformModels plataformModel)
        {
            _context.PlataformModels.Update(plataformModel);
            await _context.SaveChangesAsync();
            return plataformModel;
        }
        public async Task<PlataformModels> DeletePlataformAsync(int id)
        {
            var plataformModel = await _context.PlataformModels.FindAsync(id);
            if (plataformModel != null)
            {
                _context.PlataformModels.Remove(plataformModel);
                await _context.SaveChangesAsync();
            }
            return plataformModel!;
        }
        public async Task<PlataformModels> GetPlataformByIdAsync(int id)
        {
            var plataformModel = await _context.PlataformModels.FindAsync(id);
            return plataformModel!;
        }
        public async Task<PlataformModels> GetPlataformBySlugAsync(string slug)
        {
            var result = await _context.PlataformModels.FirstOrDefaultAsync(x => x.Slug == slug);
            return result!;
        }
        public async Task<IEnumerable<PlataformModels>> GetAllPlataformsAsync()
        {
            return await _context.PlataformModels.ToListAsync();
        }
        public async Task<int> IncrementDownloadCountPlataform(int plataformID)
        {
            // Atualização atômica diretamente na tabela de jogos
            var rowsAffected = await _context.Database.ExecuteSqlRawAsync(
                "UPDATE PlataformModels SET Count = Count + 1 WHERE Id = {0}", plataformID);

            if (rowsAffected == 0)
                throw new KeyNotFoundException("Game not found.");

            // Busca o jogo atualizado
            var updated = await _context.PlataformModels
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.Id == plataformID);

            return updated?.Count ?? 0;
        }
    }
}
