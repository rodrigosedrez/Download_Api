using Download.Domain.Entities;
using Download.Domain.Interfaces;
using Download.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Download.Infrastructure.Repositories
{
    public class GameModelRepository(DownloadsDBContext context) : IGameModelRepository
    {
        private readonly DownloadsDBContext _context = context;


        public async Task<GameModel> AddGameAsync(GameModel gameModel)
        {
            _context.GameModels.Add(gameModel);
            await _context.SaveChangesAsync();
            return gameModel;
        }
        public async Task<GameModel> UpdateGameAsync(GameModel gameModel)
        {
           _context.GameModels.Update(gameModel);
            await _context.SaveChangesAsync();
            return gameModel;
        }
        public async Task<GameModel?> GetGameByPlataformAndSlugAsync(int plataformId, string slug)
        {
            return await _context.GameModels
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.PlataformId == plataformId && x.Slug == slug);
        }

        public async Task<GameModel> DeleteGameAsync(int id)
        {
            var game = await _context.GameModels.FindAsync(id);
            if (game != null)
            {
            _context.GameModels.Remove(game);
            await _context.SaveChangesAsync();

            }
               

            return game!;
        }
        public async Task<GameModel?> GetGameByIdAsync(int id)
        {
            var game = await _context.GameModels.FindAsync(id);
            return game!;
        }
        public async Task<GameModel?> GetgameBySlug(string slug)
        {
            var game = await _context.GameModels.FirstOrDefaultAsync(x => x.Slug == slug);
            return game!;
        }
        public async Task<IEnumerable<GameModel>> GetAllGamesAsync()
        {
            var result = await _context.GameModels.ToListAsync();
            return result!;
        }
        public async Task<int> IncrementDownloadCount(int gameId)
        {
            // Atualização atômica diretamente na tabela de jogos
            var rowsAffected = await _context.Database.ExecuteSqlRawAsync(
                "UPDATE GameModels SET Count = Count + 1 WHERE Id = {0}", gameId);

            if (rowsAffected == 0)
                throw new KeyNotFoundException("Game not found.");

            // Busca o jogo atualizado
            var updated = await _context.GameModels
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.Id == gameId);

            return updated?.Count ?? 0;
        }





    }
}
