
using Download.Application.DTOs;

namespace Download.Application.Interfaces
{
    public interface IGameCategoryService
    {
        Task<GameCategoryModelDTO> AddGameCategoryAsync(GameCategoryModelDTO gameCategoryModelDTO);
        Task<GameCategoryModelDTO> UpdateGameCategoryAsync(GameCategoryModelDTO gameCategoryModelDTO);
        Task<GameCategoryModelDTO> DeleteGameCategoryAsync(int id);
        Task<GameCategoryModelDTO?> GetGameCategoryByIdAsync(int id);
        Task<IEnumerable<GameCategoryModelDTO>> GetAllGameCategoriesAsync();
    }
}
