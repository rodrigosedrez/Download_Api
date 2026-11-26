

using Download.Application.DTOs;

namespace Download.Application.Interfaces
{
    public interface IGameModelService
    {
        Task<GameModelDTOCreate> AddGameAsync(GameModelDTOCreate gameModelDTO);
        Task<GameModelDTO> UpdateGameAsync(GameModelDTO gameModelDTO);
        Task<GameModelDTO> DeleteGameAsync(int id);
        Task<GameModelDTO?> GetGameByIdAsync(int id);
        Task<IEnumerable<GameModelDTO>> GetAllGamesAsync();
        Task<GameModelDTO?> GetGameBySlugAsync(string slug);

        Task<GameModelDTO?> IncrementDownloadCountAsync(int id);
        Task<GameModelDTO?> GetGameByPlataformAndSlugAsync(int plataformId, string slug);


    }
}
