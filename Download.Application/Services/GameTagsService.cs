using AutoMapper;
using Download.Domain.Entities;
using Download.Domain.Interfaces;
using Download.Application.DTOs;
using Download.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Download.Application.Services
{
    public class GameTagsService : IGameTagsService
    {
        private readonly IGameTagModelRepository _gameTagsService;
        private readonly IMapper _mapper;

        public GameTagsService(IGameTagModelRepository gameTagsService, IMapper mapper)
        {
            _gameTagsService=gameTagsService;
            _mapper=mapper;
        }
        public async Task<GameTagModelDTO> AddGameTagAsync(GameTagModelDTO gameTagsModelDTO)
        {
            var gameTagEntity = _mapper.Map<GameTagModel>(gameTagsModelDTO);
            var createdGameTag = await _gameTagsService.AddGameTagAsync(gameTagEntity);
            return _mapper.Map<GameTagModelDTO>(createdGameTag);
        }
        public async Task<GameTagModelDTO> UpdateGameTagAsync(GameTagModelDTO gameTagsModelDTO)
        {
            var gameTagEntity = _mapper.Map<GameTagModel>(gameTagsModelDTO);
            var updatedGameTag = await _gameTagsService.UpdateGameTagAsync(gameTagEntity);
            return _mapper.Map<GameTagModelDTO>(updatedGameTag);
        }
        public async Task<GameTagModelDTO> DeleteGameTagAsync(int id)
        {
            var deletedGameTag = await _gameTagsService.DeleteGameTagAsync(id);
            return _mapper.Map<GameTagModelDTO>(deletedGameTag);
        }
        public async Task<GameTagModelDTO?> GetGameTagByIdAsync(int id)
        {
            var gameTag = await _gameTagsService.GetGameTagByIdAsync(id);
            return gameTag != null ? _mapper.Map<GameTagModelDTO>(gameTag) : null;
        }
        public async Task<GameTagModelDTO?> GetGameTagBySlugAsync(string slug)
        {
           var gameTag =  _gameTagsService.GetGameTagBySlugAsync(slug);
            return _mapper.Map<GameTagModelDTO?>(gameTag);
        }
        public async Task<IEnumerable<GameTagModelDTO>> GetAllGameTagsAsync()
        {
            var gameTags = await _gameTagsService.GetAllGameTagsAsync();
            return _mapper.Map<IEnumerable<GameTagModelDTO>>(gameTags);

        }
        public async Task<GameTagModelDTO?> IncrementRequestCountAsync(int id)
        {
            // Incremento atômico via repository
            var newCount = await _gameTagsService.IncrementDownloadCount(id);

            // Busca o jogo atualizado
            var game = await _gameTagsService.GetGameTagByIdAsync(id);
            if (game == null) return null;

            // Atualiza o campo de downloads
            return _mapper.Map<GameTagModelDTO>(game);
        }
    }
    }

