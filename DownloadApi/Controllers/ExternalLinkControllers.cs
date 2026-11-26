

using Download.Application.DTOs;
using Download.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Download.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExternalLinkControllers : ControllerBase
    {
         private readonly IExternalLinksService _externalLinksService;
        public ExternalLinkControllers(IExternalLinksService externalLinksService)
        {
            _externalLinksService = externalLinksService;
        }
        [HttpGet("{skip}/{take}")]

        public async Task<ActionResult<IEnumerable<ExternalLinksModelDTO>>> GetAllLinks(int skip = 0, int take = 50)
        {
            var links = await _externalLinksService.GetAllExternalLinksAsync();
            var totalLinks = links.Count();
            var paginatedLinks = links.Skip(skip).Take(take).ToList();
            var totalPages = (int)Math.Ceiling((double)totalLinks / take);
            var currentPage = (skip / take) + 1;
            if (skip < 0 || take <= 0 || skip > totalLinks || take > 50)
            {
                return BadRequest("Invalid pagination parameters.");
            }
            return Ok(new { totalLinks, totalPages, currentPage, paginatedLinks });

        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ExternalLinksModelDTO>> GetLinkById(int id)
        {
            var link = await _externalLinksService.GetExternalLinkByIdAsync(id);
            if (link == null)
            {
                return NotFound();
            }
            return Ok(link);
        }
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ExternalLinksModelDTO>> AddLink([FromBody] ExternalLinksModelDTO externalLinksModelDTO)
        {
            if (externalLinksModelDTO == null)
            {
                return BadRequest("Invalid link data.");
            }
            var createdLink = await _externalLinksService.AddExternalLinkAsync(externalLinksModelDTO);
            return CreatedAtAction(nameof(GetLinkById), new { id = createdLink.Id }, createdLink);
        }
        [HttpPut("{id}")]
       [Authorize]
        public async Task<IActionResult> UpdateLink(int id, ExternalLinksModelDTO externalLinksModelDTO)
        {
           
            var updatedLink = await _externalLinksService.UpdateExternalLinkAsync(externalLinksModelDTO);
            
            return Ok(updatedLink);
        }
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<ExternalLinksModelDTO>> DeleteLink(int id)
        {
            var deletedLink = await _externalLinksService.DeleteExternalLinkAsync(id);
            if (deletedLink == null)
            {
                return NotFound();
            }
            return Ok(deletedLink);
        }

    }
}
