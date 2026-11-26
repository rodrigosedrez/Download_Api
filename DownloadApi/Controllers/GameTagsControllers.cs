using Download.Application.DTOs;
using Download.Application.Interfaces;
using Download.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Download.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameTagsControllers : ControllerBase
    {
        private readonly IGameTagsService _gameTagsService;
        private readonly IDistributedCache _cache;

        public GameTagsControllers(IGameTagsService gameTagsService, IDistributedCache cache)
        {
            _gameTagsService=gameTagsService;
            _cache=cache;
        }

        [HttpGet("{skip}/{take}")]
        public async Task<ActionResult<IEnumerable<GameTagModelDTO>>> GetAllGameTag(int skip, int take)
        {
            if (skip < 0 || take <= 0 || take > 50)
            {
                return BadRequest("Invalid pagination parameters.");
            }
            var cacheKey = $"GameTags_{skip}_{take}";
            var cachedData = await _cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedData))
            {
                return Content(cachedData, "application/json");
            }
            var tags = await _gameTagsService.GetAllGameTagsAsync();
            var order = tags.OrderBy(g => g.RequestCount).ToList();
            var gameTags = order.Skip(skip).Take(take).ToList();
            var result = new GameTagRespose
            {
                TotalGameTags = order.Count(),
                TotalPages = gameTags.Any() ? (int)Math.Ceiling((double)gameTags.Count / take) : 0,
                CurrentPage = (skip / take) + 1,
                paginatedGamesTags = gameTags.Skip(skip).Take(take).ToList()
            };

            var cacheOptions = new DistributedCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(15))
            .SetSlidingExpiration(TimeSpan.FromMinutes(5));

            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(result), cacheOptions);

            return Ok(result);
        }
        [HttpGet("slug/{slug}")]
        public async Task<ActionResult<GameTagModelDTO>> GetGameTagBySlug(string slug)
        {
            var cacheKey = $"GameTag_{slug}";
            var cachedGame = await _cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedGame))
            {
                var gameFromCache = JsonSerializer.Deserialize<GameTagModelDTO>(cachedGame);
                return Ok(gameFromCache);
            }
            var gameTag = await _gameTagsService.GetGameTagBySlugAsync(slug);
            if (gameTag == null)
            {
                return NotFound();
            }
            var cacheOptions = new DistributedCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(15))
            .SetSlidingExpiration(TimeSpan.FromMinutes(5));

            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(gameTag), cacheOptions);
            return Ok(gameTag);
        }
        [HttpPut("{id}/downloads")]
        public async Task<IActionResult> UpdateDownloadCount(int id)
        {
            var conta = await _gameTagsService.IncrementRequestCountAsync(id);
            if (conta == null) return NotFound();
            return Ok(conta!.RequestCount);
        }

        // __________________________________adm _____________________________________________________

        [HttpGet("adm/{skip}/{take}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<GameModelDTO>>> GetAllGameTagsAdm(int skip = 0, int take = 50)
        {
            var tags = await _gameTagsService.GetAllGameTagsAsync();
            var order = tags.OrderBy(g => g.RequestCount).ToList();
            var gameTags = order.Skip(skip).Take(take).ToList();
            var result = new GameTagRespose
            {
                TotalGameTags = order.Count(),
                TotalPages = gameTags.Any() ? (int)Math.Ceiling((double)gameTags.Count / take) : 0,
                CurrentPage = (skip / take) + 1,
                paginatedGamesTags = gameTags.Skip(skip).Take(take).ToList()
            };


            if (skip < 0 || take <= 0 || take > 50)
            {
                return BadRequest("Invalid pagination parameters.");
            }


            return Ok(result);
        }


        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<GameTagModelDTO>> GetGame(int id)
        {
            var gameTag = await _gameTagsService.GetGameTagByIdAsync(id);
            if (gameTag == null)
            {
                return NotFound();
            }
            return Ok(gameTag);
        }
        [HttpGet("adm/{slug}")]
        [Authorize]
        public async Task<ActionResult<GameTagModelDTO>> GetGameBySlug(string slug)
        {
            var gameTag = await _gameTagsService.GetGameTagBySlugAsync(slug);
            if (gameTag == null)
            {
                return NotFound();
            }
            return Ok(gameTag);
        }
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateGameTag(int id, GameTagModelDTO gameTagDto)
        {
            if (id != gameTagDto.Id)
            {
                return BadRequest("ID mismatch");
            }
            var updatedGame = await _gameTagsService.UpdateGameTagAsync(gameTagDto);
            return Ok(updatedGame);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<GameTagModelDTO>> AddGameTag(GameTagModelDTO gameTagDto)
        {
            try
            {
                var createdGameTag = await _gameTagsService.AddGameTagAsync(gameTagDto);


                return CreatedAtAction(nameof(GetGame), new { id = createdGameTag.Id }, createdGameTag);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred while adding the game: {ex.Message}");
            }
        }
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteGameTag(int id)
        {
            var existingGameTag = await _gameTagsService.GetGameTagByIdAsync(id);
            if (existingGameTag == null)
            {
                return NotFound();
            }
            await _gameTagsService.DeleteGameTagAsync(id);
            return NoContent();
        }
    }
}      
public class GameTagRespose
{
    public int? TotalGameTags { get; set; }
    public int? TotalPages { get; set; }
    public int? CurrentPage { get; set; }
    public List<GameTagModelDTO> paginatedGamesTags { get; set; } = new();
}