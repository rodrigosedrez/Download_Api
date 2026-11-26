 using Download.Application.DTOs;
using Download.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Download.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmulatorController(IEmulatorService emulatorService, IDistributedCache cache) : ControllerBase
    {
        private readonly IEmulatorService _emulatorService= emulatorService;
        private readonly IDistributedCache _cache = cache;

        [HttpGet("{skip}/{take}")]
        public async Task<ActionResult<IEnumerable<EmulatorsModelDTO>>> GetEmus(int skip = 0, int take = 50)
        {
            if (skip < 0 || take <= 0 || take > 50)
            {
                return BadRequest("Invalid pagination parameters.");
            }
            string cacheKey = $"emulators_all_{skip}/{take}";

            var cachedData = await _cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedData))
            {
                return Content(cachedData, "application/json");
            }

            var emu =      await _emulatorService.GetAllEmulatorsAsync();
            var emulators = emu.Count();
          

            var result = new ResponseEmulator { 
                TotalServers = emulators, 
                TotalPages = emulators > 0 ? (int)Math.Ceiling((double)emulators / take) : 0,
                CurrentPage = (skip / take) + 1,
                paginatedEmulators = emu.Skip(skip).Take(take).ToList()
            };

            var cacheOptions = new DistributedCacheEntryOptions()
         .SetAbsoluteExpiration(TimeSpan.FromMinutes(15))
         .SetSlidingExpiration(TimeSpan.FromMinutes(5));

            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(result), cacheOptions);
            return Ok(result);
        }

        [HttpGet("/byname/{query}/{skip}/{take}")]
        public async Task<ActionResult<IEnumerable<EmulatorsModelDTO>>> GetEmusByName(string query, int skip = 0, int take = 50)
        {
            var todosEmus = await _emulatorService.GetAllEmulatorsAsync();
            var emulators = todosEmus.Where(e => e.EmulatorName != null && e.EmulatorName.Contains(query, StringComparison.OrdinalIgnoreCase)).ToList();
            var totalEmulators = emulators.Count();
            var paginatedEmulators = emulators.Skip(skip).Take(take).ToList();
            var totalPages = (int)Math.Ceiling((double)totalEmulators / take);
            var currentPage = (skip / take) + 1;
            if (skip < 0 || take <= 0 || skip > totalEmulators || take > 50)
            {
                return BadRequest("Invalid pagination parameters.");
            }
            return Ok(new { totalEmulators, totalPages, currentPage, paginatedEmulators });
        }


        [HttpGet("/byplataform/{id}/{skip}/{take}")]
        public async Task<ActionResult<IEnumerable<EmulatorsModelDTO>>> GetEmusByPlataform(int id, int skip = 0, int take = 50)
        {
            if (skip < 0 || take <= 0 || take > 50)
            {
                return BadRequest("Invalid pagination parameters.");
            }
            string cacheKey = $"emulator_byplatfom/{id}{skip}/{take}";

            var cachedData = await _cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedData))
            {
                return Content(cachedData, "application/json");
            }

            var emulators = await _emulatorService.GetEmulatorsByPlataformIdAsync(id);
            var totalEmulators = emulators.Count();


            var result = new ResponseEmulator
            {
                TotalServers = totalEmulators,
                TotalPages = totalEmulators > 0 ? (int)Math.Ceiling((double)totalEmulators / take) : 0,
                CurrentPage = (skip / take) + 1,
                paginatedEmulators = emulators.Skip(skip).Take(take).ToList()
            };

            var cacheOptions = new DistributedCacheEntryOptions()
           .SetAbsoluteExpiration(TimeSpan.FromMinutes(15))
           .SetSlidingExpiration(TimeSpan.FromMinutes(5));

            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(result), cacheOptions);
           
            return Ok(result);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<EmulatorsModelDTO>> GetEmu(int id)
        {
            string cacheKey = $"emulator_byId{id}";

         
            var cachedGame = await _cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedGame))
            {
                var gameFromCache = JsonSerializer.Deserialize<EmulatorsModelDTO>(cachedGame);
                return Ok(gameFromCache);
            }
            var emulator = await _emulatorService.GetEmulatorByIdAsync(id);
            if (emulator == null) { return NotFound(); }

            var cacheOptions = new DistributedCacheEntryOptions()
          .SetAbsoluteExpiration(TimeSpan.FromMinutes(30))
          .SetSlidingExpiration(TimeSpan.FromMinutes(10));

            string serializedGame = JsonSerializer.Serialize(emulator);
            await _cache.SetStringAsync(cacheKey, serializedGame, cacheOptions);

            return Ok(emulator);

        }


        [HttpGet("emulatorslug/{slug}")]
        public async Task<ActionResult<EmulatorsModelDTO>> GetEmuSlug(string slug)
        {
            string cacheKey = $"emulator_byslug{slug}";

            // 1. Tentar obter o jogo do Redis
            var cachedGame = await _cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedGame))
            {
                var gameFromCache = JsonSerializer.Deserialize<EmulatorsModelDTO>(cachedGame);
                return Ok(gameFromCache);
            }
            var emulator = await _emulatorService.GetEmulatorBySlugAsync(slug);
            if (emulator == null) { return NotFound(); }
            var cacheOptions = new DistributedCacheEntryOptions()
         .SetAbsoluteExpiration(TimeSpan.FromMinutes(30))
         .SetSlidingExpiration(TimeSpan.FromMinutes(10));

            string serializedGame = JsonSerializer.Serialize(emulator);
            await _cache.SetStringAsync(cacheKey, serializedGame, cacheOptions);

            return Ok(emulator);

        }

        //-------------------------------------------adm--------------------------------------------------

        [HttpGet("adm/{skip}/{take}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<EmulatorsModelDTO>>> GetEmusAdm(int skip = 0, int take = 50)
        {
            var emulators = await _emulatorService.GetAllEmulatorsAsync();
            var totalEmulators = emulators.Count();
            var paginatedEmulators = emulators.Skip(skip).Take(take).ToList();
            var totalPages = (int)Math.Ceiling((double)totalEmulators / take);
            var currentPage = (skip / take) + 1;
            if (skip < 0 || take <= 0 || skip > totalEmulators || take > 50)
            {
                return BadRequest("Invalid pagination parameters.");
            }
            return Ok(new { totalEmulators, totalPages, currentPage, paginatedEmulators });
        }
       
        [HttpGet("adm/byplataform/{id}/{skip}/{take}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<EmulatorsModelDTO>>> GetEmusByPlataformAdm(int id, int skip = 0, int take = 50)
        {
            var emulators = await _emulatorService.GetEmulatorsByPlataformIdAsync(id);
            var totalEmulators = emulators.Count();
            var paginatedEmulators = emulators.Skip(skip).Take(take).ToList();
            var totalPages = (int)Math.Ceiling((double)totalEmulators / take);
            var currentPage = (skip / take) + 1;
            if (skip < 0 || take <= 0 || skip > totalEmulators || take > 50)
            {
                return BadRequest("Invalid pagination parameters.");
            }
            return Ok(new { totalEmulators, totalPages, currentPage, paginatedEmulators });
        }
        [HttpGet("adm/{id}")]
        [Authorize]
        public async Task<ActionResult<EmulatorsModelDTO>> GetEmuAdm(int id)
        {
            var emulator = await _emulatorService.GetEmulatorByIdAsync(id);
            if (emulator == null) { return NotFound(); }
            return Ok(emulator);

        }
        [HttpGet("adm/slug/{id}")]
        [Authorize]
        public async Task<ActionResult<EmulatorsModelDTO>> GetEmuSlugAdm(string slug)
        {
            var emulator = await _emulatorService.GetEmulatorBySlugAsync(slug);
            if (emulator == null) { return NotFound(); }
            return Ok(emulator);

        }




        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateEmu (int id, EmulatorsModelDTO emulators)
        {
            if (id != emulators.Id)
            {
                return BadRequest("ID mismatch");
            }
            var umuUpdate = await _emulatorService.UpdateEmulatorAsync(emulators);
            return Ok(umuUpdate);
        }
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteEmulator(int id)
        {
            var deleteEmulator = await _emulatorService.DeleteEmulatorAsync(id);
            if (deleteEmulator == null) { return NotFound(); }
            return NoContent();
        }
        [HttpPost]
        [Authorize]
        public async  Task<ActionResult<EmulatorsModelDTO>> AddEmulator(EmulatorsModelDTO emulators)
        {
            var newEmulator = await _emulatorService.AddEmulatorAsync(emulators);
            return CreatedAtAction(nameof(GetEmu), new { id = newEmulator.Id }, newEmulator);
        }
    }
}

public class ResponseEmulator 
{
    public int? TotalServers { get; set; }
    public int? TotalPages { get; set; }
    public int? CurrentPage { get; set; }
    public List<EmulatorsModelDTO> paginatedEmulators { get; set; }
}