using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using SchoolApp.Services.Data.Contrancts;

namespace SchoolApp.Services.Data
{
    public class AzureBlobService : IAzureBlobService
    {
        private readonly BlobServiceClient _blobServiceClient;

        public AzureBlobService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public async Task<(bool isSuccessful, string? errorMessage, string? imageUrl)> UploadTeacherImageAsync(IFormFile file, string firstName, string lastName)
        {
            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient("teachersimages");
            await containerClient.CreateIfNotExistsAsync();

            string extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            string blobName = $"teacher-{firstName.ToLower()}-{lastName.ToLower()}{extension}";

            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            if (await blobClient.ExistsAsync())
            {
                return (false, "Снимка с това име вече съществува!", string.Empty);
            }

            await using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, overwrite: true);
            }

            return (true, string.Empty, blobClient.Uri.ToString());
        }

        public async Task<bool> DeleteTeacherImageAsync(string imageUrl)
        {

            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient("teachersimages");

            // Extract blob name from the full URL
            Uri uri = new Uri(imageUrl);
            string blobName = Path.GetFileName(uri.LocalPath);

            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            if (await blobClient.ExistsAsync())
            {
                await blobClient.DeleteAsync();
                return true;
            }

            return false;
        }
    }
}