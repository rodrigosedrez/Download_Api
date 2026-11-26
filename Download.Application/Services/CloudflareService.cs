using Amazon.S3;
using Amazon.S3.Model;
using Download.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace Download.Application.Services
{
    public class CloudflareService : ICloudflareService
    {
        private readonly AmazonS3Client amazonS3Client;
        private readonly string _bucketName;
        private readonly string _cdnDomain;
        private readonly string _cdnSecret;
        private static readonly HttpClient _sharedHttpClient = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(20)
        };

        public CloudflareService(IConfiguration configuration)
        {
            _bucketName = configuration["Cloudflare:BucketName"]!;
            _cdnDomain = configuration["Cloudflare:CDNDomain"]!;
            _cdnSecret = configuration["Cloudflare:CDNSecret"]!;
            amazonS3Client = new AmazonS3Client(
                configuration["Cloudflare:AccessKeyId"],
                configuration["Cloudflare:SecretAccessKey"],
                new AmazonS3Config
                {
                    ServiceURL = configuration["Cloudflare:AccountUrl"], // Ex: https://<accountid>.r2.cloudflarestorage.com
                    ForcePathStyle = true,
                    Timeout = TimeSpan.FromSeconds(20),
                    MaxErrorRetry = 2
                });
        }

        public string GetPresignedUploadUrl(string folder, string fileName, string contentType = "application/octet-stream")
        {
            var key = $"{folder}/{fileName}";

            var request = new GetPreSignedUrlRequest
            {
                BucketName = _bucketName,
                Key = key,
                Verb = HttpVerb.PUT,
                Expires = DateTime.UtcNow.AddMinutes(30), // Expira em 30 minutos
            };

            request.Headers["Content-Type"] = contentType;

            return amazonS3Client.GetPreSignedURL(request);
        }

        public string GetPublicUrl(string objectKey)
        {
            var safeKey = Uri.EscapeDataString(objectKey);
            return $"{_cdnDomain}/{safeKey}";
        }

        public string GerarLinkDownload(string pasta, string arquivo)
        {
            // 1. Definir expiração (ex: 2 horas / 120 minutos)
            var expiry = DateTimeOffset.UtcNow.AddMinutes(120).ToUnixTimeSeconds();

            // 2. Montar o caminho exato (ex: snes/mario.zip)
            string path = $"{pasta}/{arquivo}".TrimStart('/');

            // 3. Gerar o HMAC SHA256 (Igual ao que a Worker faz)
            string token = GerarHmacSha256(path, expiry, _cdnSecret);

            // 4. Montar a URL final com os parâmetros
            // Usamos Uri.EscapeDataString apenas no path para garantir caracteres especiais
            return $"{_cdnDomain}/{path}?token={token}&exp={expiry}";
        }

        private string GerarHmacSha256(string path, long expiry, string secret)
        {
            var encoding = new UTF8Encoding();
            // A mensagem deve ser exatamente: caminho + expiração + segredo
            string message = $"{path}{expiry}{secret}";

            byte[] keyBytes = encoding.GetBytes(secret);
            byte[] messageBytes = encoding.GetBytes(message);

            using (var hmacsha256 = new HMACSHA256(keyBytes))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                // IMPORTANTE: .ToLower() para bater com o JavaScript da Worker
                return Convert.ToHexString(hashmessage).ToLower();
            }
        }

        public async Task UploadFileAsync(string key, Stream fileStream)
        {
            var request = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = key,
                InputStream = fileStream,
                UseChunkEncoding = false
            };

            await amazonS3Client.PutObjectAsync(request);
        }

        public async Task UploadFileToFolderAsync(string folderName, string fileName, Stream fileStream, string contentType = "application/octet-stream")
        {
            var key = $"{folderName.TrimEnd('/')}/{fileName}";

            // Verifica se o arquivo já existe
            try
            {
                var metadataRequest = new GetObjectMetadataRequest
                {
                    BucketName = _bucketName,
                    Key = key
                };

                var metadata = await amazonS3Client.GetObjectMetadataAsync(metadataRequest);

                // Se chegou aqui, o arquivo existe
                return;
            }
            catch (AmazonS3Exception ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                // Arquivo não existe, pode seguir com o upload
                var request = new PutObjectRequest
                {
                    BucketName = _bucketName,
                    Key = key,
                    InputStream = fileStream,
                    ContentType = contentType,
                    AutoCloseStream = true,
                    UseChunkEncoding = false,
                };

                await amazonS3Client.PutObjectAsync(request);
            }
        }

        public async Task<Stream> DownloadFileAsync(string key)
        {
            try
            {
                using var response = await amazonS3Client.GetObjectAsync(_bucketName, key);
                var memoryStream = new MemoryStream();
                await response.ResponseStream.CopyToAsync(memoryStream);
                memoryStream.Position = 0;
                return memoryStream;
            }
            catch (AmazonS3Exception s3Ex) when (s3Ex.ErrorCode == "NoSuchKey")
            {
                return null!;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CloudflareService.DownloadFileAsync] {ex.Message}");
                return null!;
            }
        }

        public async Task<Stream?> HashDownloadArquivoDaPastaHash(string pasta, string nomeArquivo, int minutosExpiracao = 5)
        {
            try
            {
                var chave = $"{pasta}/{nomeArquivo}"; // Ex: "imagens/produtos/foto1.jpg"

                // 1. Gerar URL temporária
                var request = new GetPreSignedUrlRequest
                {
                    BucketName = _bucketName,
                    Key = chave,
                    Expires = DateTime.UtcNow.AddMinutes(minutosExpiracao),
                    Verb = HttpVerb.GET
                };

                var urlTemporaria = amazonS3Client.GetPreSignedURL(request);

                // 2. Usar HttpClient compartilhado para baixar o arquivo via URL temporária
                using var response = await _sharedHttpClient.GetAsync(urlTemporaria, HttpCompletionOption.ResponseHeadersRead);

                if (!response.IsSuccessStatusCode)
                    return null;

                // 3. Copiar para MemoryStream e liberar resposta HTTP
                var memoryStream = new MemoryStream();
                using var remoteStream = await response.Content.ReadAsStreamAsync();
                await remoteStream.CopyToAsync(memoryStream);
                memoryStream.Position = 0;
                return memoryStream;
            }
            catch (AmazonS3Exception s3Ex) when (s3Ex.ErrorCode == "NoSuchKey")
            {
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CloudflareService.HashDownloadArquivoDaPastaHash] {ex.Message}");
                return null;
            }
        }

        public string GerarUrlAssinada(string pasta, string nomeArquivo, int minutosExpiracao = 1)
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = _bucketName,
                Key = $"{pasta}/{nomeArquivo}",
                Expires = DateTime.UtcNow.AddMinutes(minutosExpiracao),
                Verb = HttpVerb.GET
            };

            return amazonS3Client.GetPreSignedURL(request);
        }

        public async Task<Stream?> DownloadArquivoDaPastaAsync(string pasta, string nomeArquivo)
        {
            try
            {
                var chave = $"{pasta}/{nomeArquivo}"; // Ex: "imagens/produtos/foto1.jpg"
                using var response = await amazonS3Client.GetObjectAsync(_bucketName, chave);
                var memoryStream = new MemoryStream();
                await response.ResponseStream.CopyToAsync(memoryStream);
                memoryStream.Position = 0;
                return memoryStream;
            }
            catch (AmazonS3Exception s3Ex) when (s3Ex.ErrorCode == "NoSuchKey")
            {
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CloudflareService.DownloadArquivoDaPastaAsync] {ex.Message}");
                return null;
            }
        }

        public async Task DeleteFileAsync(string key)
        {
            await amazonS3Client.DeleteObjectAsync(_bucketName, key);
        }

        public async Task DeleteFileFromFolderAsync(string folderName, string fileName)
        {
            var key = $"{folderName}/{fileName}";

            var deleteRequest = new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = key
            };

            await amazonS3Client.DeleteObjectAsync(deleteRequest);
        }

        public async Task<List<string>> ListFilesAsync()
        {
            var response = await amazonS3Client.ListObjectsV2Async(new ListObjectsV2Request
            {
                BucketName = _bucketName
            });

            return response.S3Objects.ConvertAll(o => o.Key);
        }

        public async Task<List<string>> ListarArquivosNaPastaAsync(string pasta)
        {
            var response = await amazonS3Client.ListObjectsV2Async(new ListObjectsV2Request
            {
                BucketName = _bucketName,
                Prefix = $"{pasta}/"
            });

            return response.S3Objects.ConvertAll(o => o.Key);
        }
    }
}
