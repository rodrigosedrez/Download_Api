using Download.Application.DTOs;
using Download.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Download.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PublicityControllers : ControllerBase
    {
        private readonly IPublicityService _publicityService;

        public PublicityControllers(IPublicityService publicityService)
        {
            _publicityService=publicityService;
        }
        [HttpGet("{skip}/{take}")]
        public async Task<ActionResult<IEnumerable<PublicityDTO>>> GetAllPublicity(int skip = 0, int take = 50)
        {
            var publicitys = await _publicityService.GetAllPublicityDTOAsync();
            var totalPublicitys = publicitys.Count();
            var paginatedPublicitys = publicitys.Skip(skip).Take(take).ToList();
            var totalPages = (int)Math.Ceiling((double)totalPublicitys / take);
            var currentPage = (skip / take) + 1;
            if (skip < 0 || take <= 0 || skip > totalPublicitys || take > 50)
            {
                return BadRequest("Invalid pagination parameters.");
            }
            return Ok(new { totalPublicitys, totalPages, currentPage, paginatedPublicitys });
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<PublicityDTO>> GetPublicityById(int id)
        {
            var publicity = await _publicityService.GetPublicityDTOByIdAsync(id);
            if (publicity == null)
            {
                return NotFound();
            }
            return Ok(publicity);
        }
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<PublicityDTO>> AddPublicity([FromBody] PublicityDTO publicityDTO)
        {
            if (publicityDTO == null)
            {
                return BadRequest("Invalid publicity data.");
            }
            var createdPublicity = await _publicityService.AddPublicityDTOAsync(publicityDTO);
            return CreatedAtAction(nameof(GetPublicityById), new { id = createdPublicity.Id }, createdPublicity);
        }
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdatePublicity(int id, PublicityDTO publicityDTO)
        {
            if (id != publicityDTO.Id)
            {
                return BadRequest("Publicity ID mismatch.");
            }
            var updatedPublicity = await _publicityService.UpdatePublicityDTOAsync(publicityDTO);
            return Ok(updatedPublicity);
        }
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeletePublicity(int id)
        {
            var deletedPublicity = await _publicityService.DeletePublicityDTOAsync(id);
            if (deletedPublicity == null)
            {
                return NotFound();
            }
            return NoContent();
        }

    }
    }
