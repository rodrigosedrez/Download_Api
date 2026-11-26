

namespace Download.Application.Interfaces
{
    public interface ICloudflareService
    {
        Task UploadFileAsync(string key, Stream fileStream);
        Task UploadFileToFolderAsync(string folderName, string fileName, Stream fileStream, string contentType = "application/octet-stream");
        Task<Stream> DownloadFileAsync(string key);
        Task<Stream> DownloadArquivoDaPastaAsync(string pasta, string nomeArquivo);
        Task<Stream?> HashDownloadArquivoDaPastaHash(string pasta, string nomeArquivo, int minutosExpiracao);
        string GerarUrlAssinada(string pasta, string nomeArquivo, int minutosExpiracao = 5);
        Task DeleteFileFromFolderAsync(string folderName, string fileName);
        Task DeleteFileAsync(string folder);
        Task<List<string>> ListFilesAsync();
        Task<List<string>> ListarArquivosNaPastaAsync(string pasta);
        string GerarLinkDownload(string pasta, string arquivo);
        string GetPresignedUploadUrl(string folder, string fileName, string contentType);
        string GetPublicUrl(string objectKey);

    }
}
