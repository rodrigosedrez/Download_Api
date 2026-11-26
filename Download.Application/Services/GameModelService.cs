using AutoMapper;
using Download.Domain.Entities;
using Download.Domain.Interfaces;
using Download.Application.DTOs;
using Download.Application.Interfaces;

namespace Download.Application.Services
{
    public class GameModelService(IGameModelRepository gameModelRepository, IMapper mapper) : IGameModelService
    {
        private readonly IGameModelRepository _gameModelRepository = gameModelRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<GameModelDTO?> GetGameByPlataformAndSlugAsync(int plataformId, string slug)
        {
            var gameModel = await _gameModelRepository.GetGameByPlataformAndSlugAsync(plataformId, slug);
            return gameModel == null ? null : _mapper.Map<GameModelDTO>(gameModel);
        }
        public async Task<GameModelDTOCreate> AddGameAsync(GameModelDTOCreate gameModelDTO)
        {
            var gameModel = _mapper.Map<GameModel>(gameModelDTO);
            var addedGame = await _gameModelRepository.AddGameAsync(gameModel);
            return _mapper.Map<GameModelDTOCreate>(addedGame);
        }
        public async Task<GameModelDTO> UpdateGameAsync(GameModelDTO gameModelDTO)
        {
            var gameModel = _mapper.Map<GameModel>(gameModelDTO);
            var updatedGame = await _gameModelRepository.UpdateGameAsync(gameModel);
            return _mapper.Map<GameModelDTO>(updatedGame);
        }
        public async Task<GameModelDTO> DeleteGameAsync(int id)
        {
            var deletedGame = await _gameModelRepository.DeleteGameAsync(id);
            return _mapper.Map<GameModelDTO>(deletedGame);
        }
        public async Task<GameModelDTO?> GetGameByIdAsync(int id)
        {
            var gameModel = await _gameModelRepository.GetGameByIdAsync(id);
            return _mapper.Map<GameModelDTO>(gameModel);
        }
        public async Task<GameModelDTO?> GetGameBySlugAsync(string slug)
        {
            var gameModel = await _gameModelRepository.GetgameBySlug(slug);
            return _mapper.Map<GameModelDTO>(gameModel);
        }
        public async Task<IEnumerable<GameModelDTO>> GetAllGamesAsync()
        {
            var gameModels = await _gameModelRepository.GetAllGamesAsync();
            return _mapper.Map<IEnumerable<GameModelDTO>>(gameModels);
        }
        public async Task<GameModelDTO?> IncrementDownloadCountAsync(int id)
        {
            // Incremento atômico via repository
            var newCount = await _gameModelRepository.IncrementDownloadCount(id);

            // Busca o jogo atualizado
            var game = await _gameModelRepository.GetGameByIdAsync(id);
            if (game == null) return null;

            // Atualiza o campo de downloads
            return _mapper.Map<GameModelDTO>(game);
        }
     
    }
}
