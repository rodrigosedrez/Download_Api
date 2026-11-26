using Download.Domain.Entities;

namespace Download.Domain.Interfaces
{
    public interface IGameModelRepository
    {
        Task<GameModel> AddGameAsync(GameModel gameModel);
        Task<GameModel> UpdateGameAsync(GameModel gameModel);
        Task<GameModel> DeleteGameAsync(int id);
        Task<GameModel?> GetGameByIdAsync(int id);
        Task<GameModel?> GetgameBySlug(string slug);
        Task<IEnumerable<GameModel?>> GetAllGamesAsync();
        Task<int> IncrementDownloadCount(int gameId);

        Task<GameModel?> GetGameByPlataformAndSlugAsync(int plataformId, string slug);



    }
}
