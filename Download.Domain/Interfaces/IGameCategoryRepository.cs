using Download.Domain.Entities;

namespace Download.Domain.Interfaces
{
    public interface IGameCategoryRepository
    {
        Task<GameCategoryModel> AddGameCategoryAsync(GameCategoryModel gameCategoryModel);
        Task<GameCategoryModel> UpdateGameCategoryAsync(GameCategoryModel gameCategoryModel);
        Task<GameCategoryModel> DeleteGameCategoryAsync(int id);
        Task<GameCategoryModel?> GetGameCategoryByIdAsync(int id);
        Task<IEnumerable<GameCategoryModel>> GetAllGameCategoriesAsync();
    }
}
