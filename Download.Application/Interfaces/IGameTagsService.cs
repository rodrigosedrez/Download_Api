using Download.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Download.Application.Interfaces
{
    public interface IGameTagsService
    {
       Task<GameTagModelDTO> AddGameTagAsync(GameTagModelDTO gameTagsModelDTO);
        Task<GameTagModelDTO> UpdateGameTagAsync(GameTagModelDTO gameTagsModelDTO);
        Task<GameTagModelDTO> DeleteGameTagAsync(int id);
        Task<GameTagModelDTO?> GetGameTagByIdAsync(int id);
        Task<GameTagModelDTO?> GetGameTagBySlugAsync(string slug);
        Task<IEnumerable<GameTagModelDTO>> GetAllGameTagsAsync();

        Task<GameTagModelDTO?> IncrementRequestCountAsync(int id);
    }
}
