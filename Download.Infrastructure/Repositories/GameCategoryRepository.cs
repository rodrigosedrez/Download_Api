using Download.Domain.Entities;
using Download.Domain.Interfaces;
using Download.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Download.Infrastructure.Repositories
{
    public class GameCategoryRepository(DownloadsDBContext downloadsDBContext) : IGameCategoryRepository
    {
        private readonly DownloadsDBContext _downloadsDBContext = downloadsDBContext;
      
        public async Task<GameCategoryModel> AddGameCategoryAsync(GameCategoryModel gameCategoryModel)
        {
            _downloadsDBContext.GameCategoryModels.Add(gameCategoryModel);
            await _downloadsDBContext.SaveChangesAsync();
            return gameCategoryModel;
        }
        public async Task<GameCategoryModel> UpdateGameCategoryAsync(GameCategoryModel gameCategoryModel)
        {
            _downloadsDBContext.Update(gameCategoryModel);
            await _downloadsDBContext.SaveChangesAsync();
            return gameCategoryModel;
        }
        public async Task<GameCategoryModel> DeleteGameCategoryAsync(int id)
        {
            var gameCategoryModel = await _downloadsDBContext.GameCategoryModels.FindAsync(id);
            _downloadsDBContext.GameCategoryModels.Remove(gameCategoryModel!);
            await _downloadsDBContext.SaveChangesAsync();
            return gameCategoryModel!;
        }
        // Changed return type to nullable to match the interface and allow returning null safely.
        public async Task<GameCategoryModel?> GetGameCategoryByIdAsync(int id)
        {
            var categoryModel = await _downloadsDBContext.GameCategoryModels.FindAsync(id);
            return categoryModel;
        }
        public async Task<IEnumerable<GameCategoryModel>> GetAllGameCategoriesAsync()
        {
            return await _downloadsDBContext.GameCategoryModels.ToListAsync();
        }
    }
}
