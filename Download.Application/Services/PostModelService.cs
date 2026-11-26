using AutoMapper;
using Download.Domain.Entities;
using Download.Domain.Interfaces;
using Download.Application.DTOs;
using Download.Application.Interfaces;

namespace Download.Application.Services
{
    public class PostModelService(IPostModelRepository postModelRepository, IMapper mapper) : IPostModelService
    {
        private readonly IPostModelRepository _postModelRepository = postModelRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<PostModelDTO> AddPostModelAsync(PostModelDTO postModelDTO)
        {
            var postModel = _mapper.Map<PostModel>(postModelDTO);
            var result = await _postModelRepository.AddPostAsync(postModel);
            return _mapper.Map<PostModelDTO>(result);
        }
        public async Task<PostModelDTO> UpdatePostModelAsync(PostModelDTO postModelDTO)
        {
            var postModel = _mapper.Map<PostModel>(postModelDTO);
            var result = await _postModelRepository.UpdatePostAsync(postModel);
            return _mapper.Map<PostModelDTO>(result);
        }
        public async Task<PostModelDTO> DeletePostModelAsync(int id)
        {
            var result = await _postModelRepository.DeletePostAsync(id);
            return _mapper.Map<PostModelDTO>(result);
        }
        public async Task<PostModelDTO?> GetPostModelByIdAsync(int id)
        {
            var result = await _postModelRepository.GetPostByIdAsync(id);
            return _mapper.Map<PostModelDTO>(result);
        }
        public async Task<PostModelDTO?> GetPostModelBySlugAsync(string slug)
        {
            var result = await _postModelRepository.GetPostBySlugAsync(slug);
            return _mapper.Map<PostModelDTO>(result);
        }
        public async Task<IEnumerable<PostModelDTO>> GetAllPostModelAsync()
        {
            var result = await _postModelRepository.GetAllPostAsync();
            return _mapper.Map<IEnumerable<PostModelDTO>>(result);
        }
    }
}
