using Download.Domain.Entities;
using Download.Domain.Interfaces;
using Download.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Download.Infrastructure.Repositories
{
    public class GameTagModelRepository(DownloadsDBContext context) : IGameTagModelRepository
    {
        private readonly DownloadsDBContext _context = context;

        public async Task<GameTagModel> AddGameTagAsync(GameTagModel gameTagModel)
        {
            await _context.GameTagModels.AddAsync(gameTagModel);
            await _context.SaveChangesAsync();
            return gameTagModel;
        }
        public async Task<GameTagModel> UpdateGameTagAsync(GameTagModel gameTagModel)
        {
            var existing = await _context.GameTagModels.FindAsync(gameTagModel);
            if (existing == null)
                return null!;
            _context.Update(existing);
            await _context.SaveChangesAsync();
            return existing;
        }
        public async Task<GameTagModel> DeleteGameTagAsync(int id)
        {
            var gameTag = await _context.GameTagModels.FindAsync(id);
            if (gameTag == null)
                return null!;
            _context.GameTagModels.Remove(gameTag);
            await _context.SaveChangesAsync();
            return gameTag;
        }
        public async Task<GameTagModel?> GetGameTagByIdAsync(int id)
        {
            var gameTag = await _context.GameTagModels.FindAsync(id);
            return gameTag;
        }
        public async Task<GameTagModel?> GetGameTagBySlugAsync(string slug)
        {
            var gameTag = await _context.GameTagModels.FirstOrDefaultAsync(g => g.Slug == slug);
            return gameTag;
        }

        public async Task<IEnumerable<GameTagModel>> GetAllGameTagsAsync()
        {
            var result = await _context.GameTagModels.ToListAsync();
            return result;
        }

        public async Task<int> IncrementDownloadCount(int gameTagId)
        {
            var rowsAffected = await _context.Database.ExecuteSqlRawAsync(
         "UPDATE GameTagModels SET RequestCount = RequestCount + 1 WHERE Id = {0}", gameTagId);

if (rowsAffected == 0)
    throw new KeyNotFoundException("Game not found.");

// Busca o jogo atualizado
var updated = await _context.GameTagModels
    .AsNoTracking()
    .FirstOrDefaultAsync(g => g.Id == gameTagId);

return updated?.RequestCount ?? 0;
        }
    }
}
