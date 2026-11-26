

using Download.Application.DTOs;

namespace Download.Application.Interfaces
{
    public interface IPostModelService
    {
        Task<PostModelDTO> AddPostModelAsync(PostModelDTO postModelDTO);
        Task<PostModelDTO> UpdatePostModelAsync(PostModelDTO postModelDTO);
        Task<PostModelDTO> DeletePostModelAsync(int id);
        Task<PostModelDTO?> GetPostModelByIdAsync(int id);
        Task<PostModelDTO?> GetPostModelBySlugAsync(string slug);
        Task<IEnumerable<PostModelDTO>> GetAllPostModelAsync();
    }
}
