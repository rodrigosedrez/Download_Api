using Download.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace Download.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class StorageController : ControllerBase
    {
        private readonly ICloudflareService _cloudflare;
        private readonly IDistributedCache _cache;

        public StorageController(ICloudflareService claudflare)
        {
            _cloudflare = claudflare;
        }
        [HttpGet("presigned")]
        [Authorize]
        public IActionResult GetPresignedUrl(
                 [FromQuery] string folder,
                 [FromQuery] string fileName,
                 [FromQuery] string contentType = "application/octet-stream")
        {
            var uploadUrl = _cloudflare.GetPresignedUploadUrl(folder, fileName, contentType);

            var objectKey = $"{folder}/{fileName}";

            var publicUrl = _cloudflare.GetPublicUrl(objectKey);

            return Ok(new
            {
                UploadUrl = uploadUrl,
                ObjectKey = objectKey,
                PublicUrl = publicUrl
            });
        }

        [HttpPost("upload")]
        [Authorize]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            await _cloudflare.UploadFileAsync(file.FileName, stream);
            return Ok("Arquivo enviado com sucesso");
        }

        [HttpPost("upload/{folder}")]
        [Authorize]
        [DisableRequestSizeLimit]

        public async Task<IActionResult> Upload(string folder, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Arquivo inválido.");

            using var stream = file.OpenReadStream();
            await _cloudflare.UploadFileToFolderAsync(folder, file.FileName, stream, file.ContentType);

            return Ok($"Arquivo enviado para pasta '{folder}' com nome '{file.FileName}'");
        }




        [HttpGet("download/{fileName}")]
        public async Task<IActionResult> Download(string fileName)
        {
            var stream = await _cloudflare.DownloadFileAsync(fileName);

            if (stream == null)
                return NotFound($"Arquivo {fileName} não encontrado.");

            return File(stream, "application/octet-stream", fileName);
        }


        [HttpGet("download/{pasta}/{fileName}")]
        public async Task<IActionResult> Download(string pasta, string fileName)
        {

            var stream = await _cloudflare.DownloadArquivoDaPastaAsync(pasta, fileName);

            if (stream == null)
                return NoContent(); // retorna 204 sem body

            return File(stream, "application/octet-stream", fileName);
        }

        [HttpGet("downloadhash/{pasta}/{fileName}")]
        public IActionResult DownloadHash(string pasta, string fileName)
        {
            var url = _cloudflare.GerarLinkDownload(pasta, fileName);
            return Ok(new { url });
        }

        [HttpDelete("delete/{fileName}")]
        [Authorize]
        public async Task<IActionResult> Delete(string fileName)
        {
            await _cloudflare.DeleteFileAsync(fileName);
            return Ok("Arquivo excluído");
        }
        [HttpDelete("delete/{pasta}/{fileName}")]
        [Authorize]
        public async Task<IActionResult> Delete(string pasta, string fileName)
        {
            await _cloudflare.DeleteFileFromFolderAsync(pasta, fileName);
            return Ok("Arquivo excluído da pasta");
        }

        [HttpGet("list")]
        [Authorize]
        public async Task<IActionResult> List()
        {
            var files = await _cloudflare.ListFilesAsync();
            return Ok(files);
        }
        [HttpGet("list/{prefix}")]
        [Authorize]
        public async Task<IActionResult> List(string prefix)
        {
            var files = await _cloudflare.ListarArquivosNaPastaAsync(prefix);
            return Ok(files);
        }

    }
}
