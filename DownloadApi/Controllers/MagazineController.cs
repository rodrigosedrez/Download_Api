using Download.Application.DTOs;
using Download.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Download.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MagazineController : ControllerBase
    {
        private readonly IMagazineService _magazineService;
        public MagazineController(IMagazineService magazineService)
        {
            _magazineService = magazineService;
        }
        [HttpGet("{skip}/{take}")]
        public async Task<ActionResult<IEnumerable<MagazineModelDTO>>> GetAllMagazines(int skip = 0, int take = 50) 
        { 
         var magazine = await _magazineService.GetAllMagazinesAsync();
            var totalMagazines = magazine.Count();
            var paginatedMagazines = magazine.Skip(skip).Take(take).ToList();
            var totalPages = (int)Math.Ceiling((double)totalMagazines / take);
            var currentPage = (skip / take) + 1;
            if (skip < 0 || take <= 0 || skip > totalMagazines || take > 50)
            {
                return BadRequest("Invalid pagination parameters.");
            }
            return Ok(new { totalMagazines, totalPages, currentPage, paginatedMagazines });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<MagazineModelDTO>>> GetMagazine (int id)
        {
            var magazine = await _magazineService.GetMagazineByIdAsync(id);
            return Ok(magazine);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<MagazineModelDTO>> AddMagazine (MagazineModelDTO magazineModelDTO)
        {
            var createMagazine = await _magazineService.AddMagazineAsync(magazineModelDTO);
            return CreatedAtAction(nameof(GetMagazine), new {id = createMagazine.Id}, createMagazine);
        }


        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateMagazine (int id, MagazineModelDTO magazineModelDTO)
        {
            var updateMaga = await _magazineService.UpdateMagazineAsync(magazineModelDTO);
            return Ok(updateMaga);
        }
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteMagazine (int id)
        {
             var delete = await _magazineService.DeleteMagazineAsync (id);
            return NoContent();
        }

    }
}
