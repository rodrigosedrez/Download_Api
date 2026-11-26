using Download.Domain.Entities;
using Download.Domain.Interfaces;
using Download.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Download.Infrastructure.Repositories
{
    public class PostModelRepository(DownloadsDBContext context) : IPostModelRepository
    {
        private readonly DownloadsDBContext _context = context;

        public async Task<PostModel> AddPostAsync(PostModel postModel)
        {
            _context.PostModels.Add(postModel);
            await _context.SaveChangesAsync();
            return postModel;
        }
        public async Task<PostModel> UpdatePostAsync(PostModel postModel)
        {
            _context.PostModels.Update(postModel);
            await _context.SaveChangesAsync();
            return postModel;
        }
        public async Task<PostModel> DeletePostAsync(int id)
        {
            var postModel = await _context.PostModels.FindAsync(id);
            if (postModel != null)
            {
                _context.PostModels.Remove(postModel);
                await _context.SaveChangesAsync();
            }
            return postModel!;
        }
        public async Task<PostModel?> GetPostBySlugAsync(string slug)
        {
            var result = await _context.PostModels.FirstOrDefaultAsync(x => x.Slug == slug);
           if(result != null) { return result; }
            return result;
        }
        public async Task<PostModel?> GetPostByIdAsync(int id)
        {
            return await _context.PostModels.FindAsync(id);
        }
        public async Task<IEnumerable<PostModel>> GetAllPostAsync()
        {
            return await _context.PostModels.ToListAsync();
        }
    }
}
