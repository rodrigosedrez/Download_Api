using Download.Application.DTOs;
using Download.Application.Interfaces;
using Download.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Download.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly IPostModelService _postModelService;
        public PostsController(IPostModelService postModelService)
        {
            _postModelService = postModelService;
        }
        [HttpGet("{skip}/{take}")]
        public async Task<ActionResult<IEnumerable<PostModelDTO>>> GetAllPosts(int skip = 0, int take = 50)
        {
            var posts = await _postModelService.GetAllPostModelAsync();
            var totalPosts = posts.Count();
            var paginatedPosts = posts.Skip(skip).Take(take).ToList();
            var totalPages = (int)Math.Ceiling((double)totalPosts / take);
            var currentPage = (skip / take) + 1;
            if (skip < 0 || take <= 0 || skip > totalPosts || take > 50)
            {
                return BadRequest("Invalid pagination parameters.");
            }
            return Ok(new { totalPosts, totalPages, currentPage, paginatedPosts });

        }
        [HttpGet("{id}")]
        public async Task<ActionResult<PostModelDTO>> GetPostById(int id)
        {
            var post = await _postModelService.GetPostModelByIdAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            return Ok(post);
        }
        [HttpGet("postsbyslug/{slug}")]
        public async Task<ActionResult<PostModelDTO>> GetPostBySlug(string slug)
        {
            var post = await _postModelService.GetPostModelBySlugAsync(slug);
            if (post == null)
            {
                return NotFound();
            }
            return Ok(post);
        }
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<PostModelDTO>> AddPost([FromBody] PostModelDTO postModelDTO)
        {
            if (postModelDTO == null)
            {
                return BadRequest("Invalid post data.");
            }
            var createdPost = await _postModelService.AddPostModelAsync(postModelDTO);
            return CreatedAtAction(nameof(GetPostById), new { id = createdPost.Id }, createdPost);
        }
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdatePost(int id, PostModelDTO postModelDTO)
        {
            if (id != postModelDTO.Id)
            {
                return BadRequest("Post ID mismatch.");
            }
            var updatedPost = await _postModelService.UpdatePostModelAsync(postModelDTO);
            return Ok(updatedPost);
        }
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeletePost(int id)
        {
            var deletedPost = await _postModelService.DeletePostModelAsync(id);
            if (deletedPost == null)
            {
                return NotFound();
            }
            return NoContent();

        }
    }

}
