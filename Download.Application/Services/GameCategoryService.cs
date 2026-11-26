using AutoMapper;
using Download.Domain.Entities;
using Download.Domain.Interfaces;
using Download.Application.DTOs;
using Download.Application.Interfaces;

namespace Download.Application.Services
{
    public class GameCategoryService(IGameCategoryRepository gameCategoryRepository, IMapper mapper) : IGameCategoryService
    {
        private readonly IGameCategoryRepository _gameCategoryRepository = gameCategoryRepository;
        private readonly IMapper _mapper = mapper;
        
        public async Task<GameCategoryModelDTO> AddGameCategoryAsync(GameCategoryModelDTO gameCategoryModelDTO)
        {
            var gameCategoryModel = _mapper.Map<GameCategoryModel>(gameCategoryModelDTO);
            var addedGameCategory = await _gameCategoryRepository.AddGameCategoryAsync(gameCategoryModel);
            return _mapper.Map<GameCategoryModelDTO>(addedGameCategory);
        }
        public async Task<GameCategoryModelDTO> UpdateGameCategoryAsync(GameCategoryModelDTO gameCategoryModelDTO)
        {
            var gameCategoryModel = _mapper.Map<GameCategoryModel>(gameCategoryModelDTO);
             await _gameCategoryRepository.UpdateGameCategoryAsync(gameCategoryModel);
            return _mapper.Map<GameCategoryModelDTO>(gameCategoryModel);
        }
        public async Task<GameCategoryModelDTO> DeleteGameCategoryAsync(int id)
        {
            var deletedGameCategory = await _gameCategoryRepository.DeleteGameCategoryAsync(id);
            return _mapper.Map<GameCategoryModelDTO>(deletedGameCategory);
        }
        public async Task<GameCategoryModelDTO?> GetGameCategoryByIdAsync(int id)
        {
            var gameCategoryModel = await _gameCategoryRepository.GetGameCategoryByIdAsync(id);
            return _mapper.Map<GameCategoryModelDTO>(gameCategoryModel);
        }
        public async Task<IEnumerable<GameCategoryModelDTO>> GetAllGameCategoriesAsync()
        {
            var gameCategoryModels = await _gameCategoryRepository.GetAllGameCategoriesAsync();
            return _mapper.Map<IEnumerable<GameCategoryModelDTO>>(gameCategoryModels);
        }
    }
}
