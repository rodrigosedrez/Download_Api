
using Amazon.Runtime.Internal.Util;
using Download.Application.DTOs;
using Download.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Download.Api.Controllers
{
       [ApiController]
        [Route("api/[controller]")]
    public class GameCategoryControllers : ControllerBase
    {
        private readonly IGameCategoryService _gameCategoryService;
        private readonly IDistributedCache _cache;
        public GameCategoryControllers(IGameCategoryService gameCategoryService, IDistributedCache cache)
        {
            _gameCategoryService = gameCategoryService;
            _cache=cache;
        }
        [HttpGet("{skip}/{take}")]
        public async Task<ActionResult<IEnumerable<GameCategoryModelDTO>>> GetAll(int skip = 0, int take = 50) 
        {
            if (skip < 0 || take <= 0 || take > 50)
            {
                return BadRequest("Invalid pagination parameters.");
            }
            string cacheKey = $"categrygame_{skip}/{take}";

            var cachedData = await _cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedData))
            {
                return Content(cachedData, "application/json");
            }

            var getall = await _gameCategoryService.GetAllGameCategoriesAsync();
            var filteredCategories = getall.Count();
            var result = new GameCategoryResponse
            {
                TotalCategories = filteredCategories,
                TotalPages = filteredCategories > 0 ? (int)Math.Ceiling((double)filteredCategories / take) : 0,
                CurrentPage = (skip / take) + 1,
                PaginatedGameCategories = getall.Skip(skip).Take(take).ToList()
            };

            var cacheOptions = new DistributedCacheEntryOptions()
          .SetAbsoluteExpiration(TimeSpan.FromMinutes(15))
          .SetSlidingExpiration(TimeSpan.FromMinutes(5));

            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(result), cacheOptions);
            return Ok(result);
        }
        [HttpGet("/categoryByPlatform/{plataformId}/{skip}/{take}")]
        public async Task<IActionResult> GetByPlataformId(int plataformId, int skip = 0, int take = 50)
        {
            if (skip < 0 || take <= 0 || take > 50) return BadRequest("Invalid parameters.");

            string cacheKey = $"category_plat_{plataformId}_{skip}_{take}";
            var cachedData = await _cache.GetStringAsync(cacheKey);

            if (!string.IsNullOrEmpty(cachedData))
            {
                return Content(cachedData, "application/json");
            }

            var categoriesAll = await _gameCategoryService.GetAllGameCategoriesAsync() ?? new List<GameCategoryModelDTO>();
            var filteredCategories = categoriesAll.Where(c => c.PlataformId == plataformId).ToList();

            // Monta o objeto usando a classe DTO
            var result = new GameCategoryResponse
            {
                TotalCategories = filteredCategories.Count,
                TotalPages = filteredCategories.Any() ? (int)Math.Ceiling((double)filteredCategories.Count / take) : 0,
                CurrentPage = (skip / take) + 1,
                PaginatedGameCategories = filteredCategories.Skip(skip).Take(take).ToList()
            };

            var cacheOptions = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(15));

            // Serializa o objeto completo
            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(result), cacheOptions);

            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<GameCategoryModelDTO>>> GetCategoryById (int id)
        {
            string cacheKey = $"ategory_byId{id}";

            // 1. Tentar obter o jogo do Redis
            var cachedGame = await _cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedGame))
            {
                var gameFromCache = JsonSerializer.Deserialize<GameCategoryModelDTO>(cachedGame);
                return Ok(gameFromCache);
            }
            var category = await _gameCategoryService.GetGameCategoryByIdAsync(id);
            var cacheOptions = new DistributedCacheEntryOptions()
          .SetAbsoluteExpiration(TimeSpan.FromMinutes(30))
          .SetSlidingExpiration(TimeSpan.FromMinutes(10));

            string serializedGame = JsonSerializer.Serialize(category);
            await _cache.SetStringAsync(cacheKey, serializedGame, cacheOptions);
            return Ok(category);
        }


        [HttpGet("adm/{skip}/{take}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<GameCategoryModelDTO>>> GetAllAdm(int skip = 0, int take = 50)
        {
            var getall = await _gameCategoryService.GetAllGameCategoriesAsync();
            var totalCategories = getall.Count();
            var paginatedGameCategories = getall.Skip(skip).Take(take).ToList();
            var totalPages = (int)Math.Ceiling((double)totalCategories / take);
            var currentPage = (skip / take) + 1;
            if (skip < 0 || take <= 0 || skip > totalCategories || take > 50)
            {
                return BadRequest("Invalid pagination parameters.");
            }
            return Ok(new { totalCategories, totalPages, currentPage, paginatedGameCategories });
        }
        [HttpGet("adm/categoryByPlatform/{plataformId}/{skip}/{take}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<GameCategoryModelDTO>>> GetByPlataformIdAdm(int plataformId, int skip = 0, int take = 50)
        {
            var categoriesAll = await _gameCategoryService.GetAllGameCategoriesAsync();
            var categories = categoriesAll.Where(c => c.PlataformId == plataformId).ToList();
            var totalCategories = categories.Count();
            var paginatedGameCategories = categories.Skip(skip).Take(take).ToList();
            var totalPages = (int)Math.Ceiling((double)totalCategories / take);
            var currentPage = (skip / take) + 1;
            if (skip < 0 || take <= 0 || skip > totalCategories || take > 50)
            {
                return BadRequest("Invalid pagination parameters.");
            }
            return Ok(new { totalCategories, totalPages, currentPage, paginatedGameCategories });
        }
        [HttpGet("adm/{id}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<GameCategoryModelDTO>>> GetCategoryByIdAdm(int id)
        {
            var category = await _gameCategoryService.GetGameCategoryByIdAsync(id);
            return Ok(category);
        }
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<GameCategoryModelDTO>> AddCategory (GameCategoryModelDTO gameCategoryDTO)
        {
            await _gameCategoryService.AddGameCategoryAsync(gameCategoryDTO);
            return CreatedAtAction(nameof(GetCategoryById),new { id =gameCategoryDTO.Id }, gameCategoryDTO );   
        }
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult> UpdateCategory (int id, GameCategoryModelDTO gameCategoryDTO)
        {
            var updateCategory = await _gameCategoryService.UpdateGameCategoryAsync(gameCategoryDTO);
            return Ok(updateCategory);
        }
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeteleCategory (int id)
        {
            var delete = await _gameCategoryService.DeleteGameCategoryAsync(id);
            return NoContent();
        }
        

    }
}

public class GameCategoryResponse
{
    public int TotalCategories { get; set; }
    public int TotalPages { get; set; }
    public int CurrentPage { get; set; }
    public List<GameCategoryModelDTO> PaginatedGameCategories { get; set; } = new();
}