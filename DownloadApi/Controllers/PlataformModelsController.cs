using Amazon.Runtime.Internal.Util;
using Download.Application.DTOs;
using Download.Application.Interfaces;
using Download.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Identity.Client;
using System.Text.Json;

namespace Download.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlataformModelsController(IPlataformService plataformService, IDistributedCache cache) : ControllerBase
    {
        private readonly IPlataformService _plataformService= plataformService;
        private readonly IDistributedCache _cache = cache;
      
        [HttpGet("{skip}/{take}")]
        public async Task<IActionResult> GetAllPlataforms(int skip = 0, int take = 150)
        {
            if (skip < 0 || take <= 0 || take > 150)
            {
                return BadRequest("Invalid pagination parameters.");
            }
            string cacheKey = $"platforms_all_{skip}_{take}";

            var cachedData = await _cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedData))
            {
                return Content(cachedData, "application/json");
            }
            var platformtotal = await _plataformService.GetAllPlataformsAsync();
            var plataforms = platformtotal.Count();
          

            var result = new PlatformResponse{
                TotalPlatform = plataforms, 
                TotalPages = plataforms > 0 ? (int)Math.Ceiling((double)plataforms/ take) : 0, 
                CurrentPage = (skip / take) + 1, 
                paginatedPlataforms =  platformtotal.Skip(skip).Take(take).ToList()
            };

            // 5. Salvar no Redis (Ex: 15 minutos é ideal para popularidade)
            var cacheOptions = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(15))
                .SetSlidingExpiration(TimeSpan.FromMinutes(5));

            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(result), cacheOptions);

            return Ok(result);
        }


        [HttpGet("bypopularity/{skip}/{take}")]
        public async Task<IActionResult> GetAllPlataformsByPopularity(int skip = 0, int take = 150)
        {
          
            if (skip < 0 || take <= 0 || take > 150)
            {
                return BadRequest("Invalid pagination parameters.");
            }
            string cacheKey = $"platforms_pop_{skip}_{take}";

            var cachedData = await _cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedData))
            {
                return Content(cachedData, "application/json");
            }

           
            var popular = await _plataformService.GetAllPlataformsAsync();


            var platformtotal = popular.OrderByDescending(x => x.Count).ToList();
            var plataforms = platformtotal.Count();


            var result = new PlatformResponse
            {
                TotalPlatform = plataforms,
                TotalPages = plataforms > 0 ? (int)Math.Ceiling((double)plataforms/ take) : 0,
                CurrentPage = (skip / take) + 1,
                paginatedPlataforms =  platformtotal.Skip(skip).Take(take).ToList()
            };

            // 5. Salvar no Redis (Ex: 15 minutos é ideal para popularidade)
            var cacheOptions = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(15))
                .SetSlidingExpiration(TimeSpan.FromMinutes(5));

            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(result), cacheOptions);

            return Ok(result);
        }


        [HttpGet("Byslug/{slug}")]
        public async Task<ActionResult<IEnumerable<PlataformModelsDTO>>> GetPlataformBySlug(string slug)
        {
            string cacheKey = $"platform_slug_{slug}";

          
            var cachedGame = await _cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedGame))
            {
                var gameFromCache = JsonSerializer.Deserialize<PlataformModelsDTO>(cachedGame);
                return Ok(gameFromCache);
            }
            var plataformtemp = await _plataformService.GetAllPlataformsAsync();
            var plataform = plataformtemp.FirstOrDefault(p => p.Slug!.Equals(slug, StringComparison.OrdinalIgnoreCase));

            var cacheOptions = new DistributedCacheEntryOptions()
               .SetAbsoluteExpiration(TimeSpan.FromMinutes(30))
               .SetSlidingExpiration(TimeSpan.FromMinutes(10));

            string serializedGame = JsonSerializer.Serialize(plataform);
            await _cache.SetStringAsync(cacheKey, serializedGame, cacheOptions);
            return Ok(plataform);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<PlataformModelsDTO>>> GetPlataformById(int id)
        {
            string cacheKey = $"platform_id_{id}";


            var cachedGame = await _cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedGame))
            {
                var gameFromCache = JsonSerializer.Deserialize<PlataformModelsDTO>(cachedGame);
                return Ok(gameFromCache);
            }
            var plataform = await _plataformService.GetPlataformByIdAsync(id);

            var cacheOptions = new DistributedCacheEntryOptions()
               .SetAbsoluteExpiration(TimeSpan.FromMinutes(30))
               .SetSlidingExpiration(TimeSpan.FromMinutes(10));

            string serializedGame = JsonSerializer.Serialize(plataform);
            await _cache.SetStringAsync(cacheKey, serializedGame, cacheOptions);

            return Ok(plataform);
        }
        //-----------------------------------------adm----------------------------------------------------------

        [HttpGet("adm/{skip}/{take}")]
        [Authorize]
        public async Task<IActionResult> GetAllPlataformsAdm(int skip = 0, int take = 150)
        {
            var plataforms = await _plataformService.GetAllPlataformsAsync();
            var totalPlataforms = plataforms.Count();
            var paginatedPlataforms = plataforms.Skip(skip).Take(take).ToList();
            var totalPages = (int)Math.Ceiling((double)totalPlataforms / take);
            var currentPage = (skip / take) + 1;
            if (skip < 0 || take <= 0 || skip > totalPlataforms || take > 150)
            {
                return BadRequest("Invalid pagination parameters.");
            }
            return Ok(new { totalPlataforms, totalPages, currentPage, paginatedPlataforms });
        }

        [HttpGet("adm/Byslug/{slug}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<PlataformModelsDTO>>> GetPlataformBySlugAdm(string slug)
        {
            var plataformtemp = await _plataformService.GetAllPlataformsAsync();
            var plataform = plataformtemp.FirstOrDefault(p => p.Slug!.Equals(slug, StringComparison.OrdinalIgnoreCase));
            return Ok(plataform);
        }
        [HttpGet("adm/{id}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<PlataformModelsDTO>>> GetPlataformByIdAdm(int id)
        {
            var plataform = await _plataformService.GetPlataformByIdAsync(id);

            return Ok(plataform);
        }


        [HttpPost]
         [Authorize]
        public async Task<IActionResult> AddPlataform(PlataformModelsDTO plataformModelDTO)
        {

            var createdPlataform = await _plataformService.AddPlataformAsync(plataformModelDTO);
            return CreatedAtAction(nameof(GetPlataformById), new { id = createdPlataform.Id }, createdPlataform);
        }
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdatePlataform(int id, PlataformModelsDTO plataformModelDTO)
        {
            if (id != plataformModelDTO.Id)
            {
                return BadRequest("ID mismatch");
            }
            var updatedPlataform = await _plataformService.UpdatePlataformAsync(plataformModelDTO);
            return Ok(updatedPlataform);
        }
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeletePlataform(int id)
        {
            var deletedPlataform = await _plataformService.DeletePlataformAsync(id);
            if (deletedPlataform == null)
            {
                return NotFound();
            }
            return NoContent();

        }
        [HttpPut("{id}/downloads")]
        public async Task<IActionResult> UpdateDownloadCountByPlataform(int id)
        {
            var updatedPlataforms = await _plataformService.IncrementDownloadCountPlataformAsync(id);
           
            if (updatedPlataforms == null)
            {
                return NotFound();
            }
            return Ok(updatedPlataforms.Count);
        }
    }
}
public class PlatformResponse
{
    public int? TotalPlatform { get; set; }
    public int? TotalPages { get; set; }
    public int? CurrentPage { get; set; }
    public List<PlataformModelsDTO> paginatedPlataforms { get; set; }
}