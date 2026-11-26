
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
    public class GameModelControllers : ControllerBase
    {
        private readonly IGameModelService _gameModelService;
        private readonly IGameCategoryService _gameCategoryService;
        private readonly IDistributedCache _cache;
        public GameModelControllers(IGameModelService gameModelService, IGameCategoryService gameCategoryService, IDistributedCache cache)
        {
            _gameModelService = gameModelService;
            _gameCategoryService=gameCategoryService;
            _cache=cache;
        }
        [HttpGet("{skip}/{take}")]
        public async Task<ActionResult<IEnumerable<GameModelDTO>>> GetAllGames(int skip = 0, int take = 50)
        {
            if (skip < 0 || take <= 0 || take > 50)
            {
                return BadRequest("Invalid pagination parameters.");
            }
            string cacheKey = $"game_{skip}/{take}";

            var cachedData = await _cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedData))
            {
                return Content(cachedData, "application/json");
            }

            var order  = await _gameModelService.GetAllGamesAsync();
            var games  = order.OrderBy(g => g.GameName).ToList();

            var result = new GamesRespose
            {
                TotalGames = games.Count,
                TotalPages = games.Any() ? (int)Math.Ceiling((double)games.Count / take) : 0,
                CurrentPage =  (skip / take) + 1,
                paginatedGames = games.Skip(skip).Take(take).ToList()

            };

            var cacheOptions = new DistributedCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(15))
            .SetSlidingExpiration(TimeSpan.FromMinutes(5));

            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(result), cacheOptions);

            return Ok(result);
        }

        [HttpGet("category/{plataform}/{categoryId}/{skip}/{take}")]
        public async Task<ActionResult<IEnumerable<GameModelDTO>>> GetGamesByCategory(int plataform, int categoryId, int skip = 0, int take = 50)
        {

            if (skip < 0 || take <= 0 || take > 50)
            {
                return BadRequest("Invalid pagination parameters.");
            }
            string cacheKey = $"game_cetego/{plataform}{skip}/{take}";

            var cachedData = await _cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedData))
            {
                return Content(cachedData, "application/json");
            }

            var games = await _gameModelService.GetAllGamesAsync();
            var filteredGames = games.Where(x => x.PlataformId == plataform).Where(g => g.GameCategoryModelIds!.Contains(categoryId)).OrderBy(x => x.GameName).ToList();
            if (!filteredGames.Any())
            {
                return NotFound($"No games found for category ID {categoryId}.");
            }
            var result = new GamesRespose
            {
                TotalGames = filteredGames.Count,
                TotalPages = filteredGames.Any() ? (int)Math.Ceiling((double)filteredGames.Count / take) : 0,
                CurrentPage =  (skip / take) + 1,
                paginatedGames = filteredGames.Skip(skip).Take(take).ToList()

            };

            var cacheOptions = new DistributedCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(15))
            .SetSlidingExpiration(TimeSpan.FromMinutes(5));

            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(result), cacheOptions);

            return Ok(result);
        }
        [HttpGet("categoryOrderByIncial/{letra}/{plataform}/{categoryId}/{skip}/{take}")]
        public async Task<ActionResult<IEnumerable<GameModelDTO>>> GetGamesByCategoryByInicial(string letra, int plataform, int categoryId, int skip = 0, int take = 50)
        {

            if (skip < 0 || take <= 0 || take > 50)
            {
                return BadRequest("Invalid pagination parameters.");
            }
            string cacheKey = $"game_cetego/{letra}/{plataform}{skip}/{take}";

            var cachedData = await _cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedData))
            {
                return Content(cachedData, "application/json");
            }

            var games = await _gameModelService.GetAllGamesAsync();
            var filteredGames = games.Where(x => x.PlataformId == plataform).Where(g => g.GameCategoryModelIds!.Contains(categoryId)).OrderBy(x => x.GameName)
                .Where(g => g.GameName!.StartsWith(letra, StringComparison.OrdinalIgnoreCase)).OrderBy(g => g.GameName).ToList();
            if (!filteredGames.Any())
            {
                return NotFound($"No games found for category ID {categoryId}.");
            }
            var result = new GamesRespose
            {
                TotalGames = filteredGames.Count,
                TotalPages = filteredGames.Any() ? (int)Math.Ceiling((double)filteredGames.Count / take) : 0,
                CurrentPage =  (skip / take) + 1,
                paginatedGames = filteredGames.Skip(skip).Take(take).ToList()

            };

            var cacheOptions = new DistributedCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(15))
            .SetSlidingExpiration(TimeSpan.FromMinutes(5));

            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(result), cacheOptions);

            return Ok(result);
        }


        // games por plataforma s slug

        [HttpGet("platformandslug/{plataformId:int}/{slug}")]
        public async Task<ActionResult> GetGameByPlataformAndSlug(int plataformId, string slug)
        {
            // Chave nova (v2) para não reaproveitar entradas do formato antigo
            string cacheKey = $"game_v2_{plataformId}-{slug.ToLowerInvariant()}";

            GameModelDTO? game;

            var cached = await _cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cached))
            {
                game = JsonSerializer.Deserialize<GameModelDTO>(cached);
            }
            else
            {
                game = await _gameModelService.GetGameByPlataformAndSlugAsync(plataformId, slug);

                if (game == null)
                    return NotFound();

                var cacheOptions = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(30))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(10));

                await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(game), cacheOptions);
            }

            if (game == null)
                return NotFound();

            // Formato que o cliente (paginatedGamesPagination<paginatedGames>) espera
            return Ok(new
            {
                paginatedGames = new List<GameModelDTO> { game }
            });
        }
        // games por plataforma s slug

        [HttpGet("platformandslugtext/{plataform}/{slug}")]
        public async Task<ActionResult<GameModelDTO>> GetGameBySlugandplataformtext(string plataform,string slug)
        {
            string cacheKey = $"game_{plataform}-{slug}";

                           
            var cachedGame = await _cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedGame))
            {
                var gameFromCache = JsonSerializer.Deserialize<GameModelDTO>(cachedGame);
                return Ok(gameFromCache);
            }

            var getgameplatform =await _gameModelService.GetGameBySlugAsync(slug);
            var game = getgameplatform.Slug == plataform ? getgameplatform : null;

            if (game == null)
            {
                return NotFound();
            }


            var cacheOptions = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(30))
                .SetSlidingExpiration(TimeSpan.FromMinutes(10));

            string serializedGame = JsonSerializer.Serialize(game);
            await _cache.SetStringAsync(cacheKey, serializedGame, cacheOptions);

            return Ok(game);
        }



        [HttpGet("categoryPopular/{plataform}/{categoryId}/{skip}/{take}")]
        public async Task<ActionResult<IEnumerable<GameModelDTO>>> GetGamesByCategoryPopular(int plataform, int categoryId, int skip = 0, int take = 50)
        {
            if (skip < 0 || take <= 0 || take > 50)
            {
                return BadRequest("Invalid pagination parameters.");
            }
            string cacheKey = $"game_popu_catego/{plataform}/{categoryId}{skip}/{take}";

            var cachedData = await _cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedData))
            {
                return Content(cachedData, "application/json");
            }


            var games = await _gameModelService.GetAllGamesAsync();
            var filteredGames = games.Where(x => x.PlataformId == plataform).Where(g => g.GameCategoryModelIds!.Contains(categoryId)).OrderByDescending(x => x.Count).ToList();

            var result = new GamesRespose
            {
                TotalGames = filteredGames.Count,
                TotalPages = filteredGames.Any() ? (int)Math.Ceiling((double)filteredGames.Count / take) : 0,
                CurrentPage =  (skip / take) + 1,
                paginatedGames = filteredGames.Skip(skip).Take(take).ToList()

            };

            var cacheOptions = new DistributedCacheEntryOptions()
             .SetAbsoluteExpiration(TimeSpan.FromMinutes(15))
             .SetSlidingExpiration(TimeSpan.FromMinutes(5));

            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(result), cacheOptions);
            return Ok(result);
        }
        [HttpGet("categoryPopularBest/{categoryId}/{skip}/{take}")]
        public async Task<ActionResult<IEnumerable<GameModelDTO>>> GetGamesByCategoryPopularBestof( int categoryId, int skip = 0, int take = 4)
        {
            if (skip < 0 || take <= 0 || take > 50)
            {
                return BadRequest("Invalid pagination parameters.");
            }
            string cacheKey = $"game_popu_categobest/{categoryId}{skip}/{take}";

            var cachedData = await _cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedData))
            {
                return Content(cachedData, "application/json");
            }


            var games = await _gameModelService.GetAllGamesAsync();
            var filteredGames = games.Where(g => g.GameCategoryModelIds!.Contains(categoryId)).OrderByDescending(x => x.Count).ToList();

            var result = new GamesRespose
            {
                TotalGames = filteredGames.Count,
                TotalPages = filteredGames.Any() ? (int)Math.Ceiling((double)filteredGames.Count / take) : 0,
                CurrentPage =  (skip / take) + 1,
                paginatedGames = filteredGames.Skip(skip).Take(take).ToList()

            };

            var cacheOptions = new DistributedCacheEntryOptions()
             .SetAbsoluteExpiration(TimeSpan.FromMinutes(15))
             .SetSlidingExpiration(TimeSpan.FromMinutes(5));

            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(result), cacheOptions);
            return Ok(result);
        }










        [HttpGet("platform/{platformId}/{skip}/{take}")]
        public async Task<ActionResult> GetGamesByPlatform(int platformId, int skip = 0, int take = 50)
        {
            if (skip < 0 || take <= 0 || take > 50)
            {
                return BadRequest("Invalid pagination parameters.");
            }
            string cacheKey = $"game_platform/{platformId}{skip}/{take}";

            var cachedData = await _cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedData))
            {
                return Ok(JsonSerializer.Deserialize<object>(cachedData));
            }
            var order = await _gameModelService.GetAllGamesAsync();
            var games = order.OrderBy(g => g.GameName).ToList();
            var filteredGames = games.Where(g => g.PlataformId == platformId).ToList();
            var result = new GamesRespose
            {
                TotalGames = filteredGames.Count,
                TotalPages = filteredGames.Any() ? (int)Math.Ceiling((double)filteredGames.Count / take) : 0,
                CurrentPage =  (skip / take) + 1,
                paginatedGames = filteredGames.Skip(skip).Take(take).ToList()

            };

            var cacheOptions = new DistributedCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(15))
            .SetSlidingExpiration(TimeSpan.FromMinutes(5));

            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(result), cacheOptions);

            return Ok(result);
        }
        [HttpGet("search/{query}/{skip}/{take}")]
        public async Task<ActionResult> SearchGames(string query, int skip = 0, int take = 200)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Search query cannot be empty.");
            }

            var games = await _gameModelService.GetAllGamesAsync();
            var filteredGames = games
                .Where(g => g.GameName.Contains(query, StringComparison.OrdinalIgnoreCase))
                .ToList();

            var totalGames = filteredGames.Count;
            var paginatedGames = filteredGames.Skip(skip).Take(take).ToList();
            var totalPages = totalGames > 0 ? (int)Math.Ceiling((double)totalGames / take) : 0;
            var currentPage = totalGames > 0 ? (skip / take) + 1 : 0;

            if (skip < 0 || take <= 0 || skip > totalGames || take > 200)
            {
                return BadRequest("Invalid pagination parameters.");
            }

            // Sempre retorna 200 OK, mesmo se não houver jogos
            return Ok(new
            {
                totalGames,
                totalPages,
                currentPage,
                paginatedGames
            });
        }

        //procura de jogos por plataforma e nome
        [HttpGet("searchforplaftormandname/{plataform}/{query}/{skip}/{take}")]
        public async Task<ActionResult> SearchGames(int plataform, string query, int skip = 0, int take = 200)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Search query cannot be empty.");
            }

            var games = await _gameModelService.GetAllGamesAsync();
            var filteredGames = games
                .Where(g => g.PlataformId == plataform && g.GameName.Contains(query, StringComparison.OrdinalIgnoreCase))
                .ToList();

            var totalGames = filteredGames.Count;
            var paginatedGames = filteredGames.Skip(skip).Take(take).ToList();
            var totalPages = totalGames > 0 ? (int)Math.Ceiling((double)totalGames / take) : 0;
            var currentPage = totalGames > 0 ? (skip / take) + 1 : 0;

            if (skip < 0 || take <= 0 || skip > totalGames || take > 200)
            {
                return BadRequest("Invalid pagination parameters.");
            }

            // Sempre retorna 200 OK, mesmo se não houver jogos
            return Ok(new
            {
                totalGames,
                totalPages,
                currentPage,
                paginatedGames
            });
        }
        [HttpGet("search/{plataform}/{query}/{skip}/{take}")]
        public async Task<ActionResult> SearchGamesByPlatform(int plataform, string query, int skip = 0, int take = 36)
        {

            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Search query cannot be empty.");
            }
            var games = await _gameModelService.GetAllGamesAsync();
            var filteredGames = games
                .Where(g => g.PlataformId == plataform && g.GameName.Contains(query, StringComparison.OrdinalIgnoreCase))
                .ToList();
            var totalGames = filteredGames.Count;
            var paginatedGames = filteredGames.Skip(skip).Take(take).ToList();
            var totalPages = totalGames > 0 ? (int)Math.Ceiling((double)totalGames / take) : 0;
            var currentPage = totalGames > 0 ? (skip / take) + 1 : 0;
            if (skip < 0 || take <= 0 || skip > totalGames || take > 36)
            {
                return BadRequest("Invalid pagination parameters.");
            }
            // Sempre retorna 200 OK, mesmo se não houver jogos
            return Ok(new
            {
                totalGames,
                totalPages,
                currentPage,
                paginatedGames
            });
        }
        [HttpGet("slug/{slug}")]
        public async Task<ActionResult<GameModelDTO>> GetGameBySlug(string slug)
        {
            string cacheKey = $"game_{slug}";


            var cachedGame = await _cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedGame))
            {
                var gameFromCache = JsonSerializer.Deserialize<GameModelDTO>(cachedGame);
                return Ok(gameFromCache);
            }


            var game = await _gameModelService.GetGameBySlugAsync(slug);

            if (game == null)
            {
                return NotFound();
            }


            var cacheOptions = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(30))
                .SetSlidingExpiration(TimeSpan.FromMinutes(10));

            string serializedGame = JsonSerializer.Serialize(game);
            await _cache.SetStringAsync(cacheKey, serializedGame, cacheOptions);

            return Ok(game);
        }
       
        //-----------------------------------------------area adm --------------------------------------------------

        [HttpGet("adm/{skip}/{take}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<GameModelDTO>>> GetAllGamesAdm(int skip = 0, int take = 50)
        {
            var order = await _gameModelService.GetAllGamesAsync();
            var games = order.OrderBy(g => g.GameName).ToList();
            var totalGames = games.Count();
            var paginatedGames = games.Skip(skip).Take(take).ToList();
            var totalPages = totalGames > 0 ? (int)Math.Ceiling((double)totalGames / take) : 0;
            var currentPage = totalGames > 0 ? (skip / take) + 1 : 0;


            if (skip < 0 || take <= 0 || skip > totalGames || take > 50)
            {
                return BadRequest("Invalid pagination parameters.");
            }


            return Ok(new { totalGames, totalPages, currentPage, paginatedGames });
        }
        [HttpGet("category/adm/{plataform}/{categoryId}/{skip}/{take}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<GameModelDTO>>> GetGamesByCategoryAdm(int plataform, int categoryId, int skip = 0, int take = 50)
        {
            var games = await _gameModelService.GetAllGamesAsync();
            var filteredGames = games.Where(x => x.PlataformId == plataform).Where(g => g.GameCategoryModelIds!.Contains(categoryId)).OrderBy(x =>x.GameName).ToList();
            if (!filteredGames.Any())
            {
                return NotFound($"No games found for category ID {categoryId}.");
            }
            var totalGames = filteredGames.Count();
            var paginatedGames = filteredGames.Skip(skip).Take(take).ToList();
            var totalPages = (int)Math.Ceiling((double)totalGames / take);
            var currentPage = (skip / take) + 1;
            if (skip < 0 || take <= 0 || skip > totalGames || take > 50)
            {
                return BadRequest("Invalid pagination parameters.");
            }
            return Ok(new { totalGames, totalPages, currentPage, paginatedGames });
        }

        [HttpGet("categoryPopular/adm/{plataform}/{categoryId}/{skip}/{take}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<GameModelDTO>>> GetGamesByCategoryPopularAdm(int plataform, int categoryId, int skip = 0, int take = 50)
        {
            var games = await _gameModelService.GetAllGamesAsync();
            var filteredGames = games.Where(x => x.PlataformId == plataform).Where(g => g.GameCategoryModelIds!.Contains(categoryId)).ToList();
            if (!filteredGames.Any())
            {
                return NotFound($"No games found for category ID {categoryId}.");
            }
            var totalGames = filteredGames.Count();
            var paginatedGames = filteredGames.Skip(skip).Take(take).ToList();
            var totalPages = (int)Math.Ceiling((double)totalGames / take);
            var currentPage = (skip / take) + 1;
            if (skip < 0 || take <= 0 || skip > totalGames || take > 50)
            {
                return BadRequest("Invalid pagination parameters.");
            }
            return Ok(new { totalGames, totalPages, currentPage, paginatedGames });
        }
        [HttpGet("platform/adm/{platformId}/{skip}/{take}")]
        [Authorize]
        public async Task<ActionResult> GetGamesByPlatformAdm(int platformId, int skip = 0, int take = 50)
        {

            var order = await _gameModelService.GetAllGamesAsync();
            var games = order.OrderBy(g => g.GameName).ToList();
            var filteredGames = games.Where(g => g.PlataformId == platformId).ToList();
            var totalGames = filteredGames.Count;

            // validação de parâmetros
            if (skip < 0 || take <= 0 || take > 50)
            {
                return BadRequest("Invalid pagination parameters.");
            }

            var paginatedGames = filteredGames.Skip(skip).Take(take).ToList();

            // se não houver jogos, totalPages = 0 e currentPage = 0
            var totalPages = totalGames > 0 ? (int)Math.Ceiling((double)totalGames / take) : 0;
            var currentPage = totalGames > 0 ? (skip / take) + 1 : 0;

            return Ok(new
            {
                totalGames,
                totalPages,
                currentPage,
                paginatedGames
            });
        }
        [HttpGet("slug/adm/{slug}")]
        [Authorize]
        public async Task<ActionResult<GameModelDTO>> GetGameBySlugAdm(string slug)
        {
            var game = await _gameModelService.GetGameBySlugAsync(slug);
            if (game == null)
            {
                return NotFound();
            }
            return Ok(game);
        }


        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<GameModelDTO>> GetGame(int id)
        {
            var game = await _gameModelService.GetGameByIdAsync(id);
            if (game == null)
            {
                return NotFound();
            }
            return Ok(game);
        }

      

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<GameModelDTOCreate>> AddGame(GameModelDTOCreate gameModelDTO)
        {
            try
            {
                var createdGame = await _gameModelService.AddGameAsync(gameModelDTO);
                var gameCategories = await _gameCategoryService.GetAllGameCategoriesAsync();
                if (gameModelDTO.GameCategoryModelIds != null)
                {
                    foreach (var categoryId in gameModelDTO.GameCategoryModelIds)
                    {
                        if (!gameCategories.Any(c => c.Id == categoryId))
                        {
                            return BadRequest($"Game category with ID {categoryId} is not valid.");
                        }
                    }
                }

                return CreatedAtAction(nameof(GetGame), new { id = createdGame.Id }, createdGame);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred while adding the game: {ex.Message}");
            }





        }
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateGame(int id, GameModelDTO gameModelDTO)
        {
            if (id != gameModelDTO.Id)
            {
                return BadRequest("ID mismatch");
            }
            var updatedGame = await _gameModelService.UpdateGameAsync(gameModelDTO);
            return Ok(updatedGame);
        }
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteGame(int id)
        {
            var deletedGame = await _gameModelService.DeleteGameAsync(id);
            if (deletedGame == null)
            {
                return NotFound();
            }
            return NoContent();
        }
        [HttpPut("{id}/downloads")]
        public async Task<IActionResult> UpdateDownloadCount(int id)
        {
            var conta = await _gameModelService.IncrementDownloadCountAsync(id);
            if (conta == null) return NotFound();
            return Ok(conta!.Count);
        }



    }
}

public class GamesRespose
{
    public int? TotalGames { get; set; }
    public int? TotalPages { get; set; }
    public int? CurrentPage { get; set; }
    public List<GameModelDTO> paginatedGames { get; set; } = new();
}