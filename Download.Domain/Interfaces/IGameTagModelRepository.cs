using Download.Domain.Entities;
namespace Download.Domain.Interfaces
{
    public interface IGameTagModelRepository
    {
       Task<GameTagModel> AddGameTagAsync(GameTagModel gameTagModel);
        Task<GameTagModel> UpdateGameTagAsync(GameTagModel gameTagModel);
        Task<GameTagModel> DeleteGameTagAsync(int id);
        Task<GameTagModel?> GetGameTagByIdAsync(int id);
        Task<GameTagModel?> GetGameTagBySlugAsync(string slug);
        Task<IEnumerable<GameTagModel>> GetAllGameTagsAsync();

        Task<int> IncrementDownloadCount(int gameTagId);


    }
}
