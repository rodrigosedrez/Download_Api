using Download.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Download.Domain.Interfaces
{
    public interface IPostModelRepository
    {
        Task<PostModel> AddPostAsync(PostModel  postModel);
        Task<PostModel> UpdatePostAsync(PostModel  postModel);
        Task<PostModel> DeletePostAsync(int id);
        Task<PostModel?> GetPostByIdAsync(int id);
        Task<PostModel?> GetPostBySlugAsync(string slug);
        Task<IEnumerable<PostModel>> GetAllPostAsync();
    }
}
