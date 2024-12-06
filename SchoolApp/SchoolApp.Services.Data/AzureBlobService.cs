﻿using Azure.Storage.Blobs;

using Microsoft.AspNetCore.Http;

using SchoolApp.Services.Data.Contrancts;

using static SchoolApp.Common.ApplicationConstants;

namespace SchoolApp.Services.Data;

public class AzureBlobService : IAzureBlobService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly BlobContainerClient _newsContainerClient;
    private readonly BlobContainerClient _galleryContainerClient;
    private readonly BlobContainerClient _teacherContainerClient;

    public AzureBlobService(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
        _newsContainerClient = _blobServiceClient.GetBlobContainerClient(AzureNewsContainerName);
        _galleryContainerClient = _blobServiceClient.GetBlobContainerClient(AzureGalleryContainerName);
        _teacherContainerClient = _blobServiceClient.GetBlobContainerClient(AzureTeacherContainerName);
    }

    public async Task<(bool isSuccessful, string? errorMessage, string? imageUrl)> UploadTeacherImageAsync(IFormFile file, string firstName, string lastName)
    {
        string extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        string blobName = $"teacher-{firstName.ToLower()}-{lastName.ToLower()}{extension}";

        BlobClient blobClient = _teacherContainerClient.GetBlobClient(blobName);
         
        await using (var stream = file.OpenReadStream())
        {
            await blobClient.UploadAsync(stream, overwrite: true);
        }

        return (true, string.Empty, blobClient.Uri.ToString());
    }

    public async Task<bool> DeleteTeacherImageAsync(string imageUrl)
    {
        Uri uri = new Uri(imageUrl);
        string blobName = Path.GetFileName(uri.LocalPath);

        BlobClient blobClient = _teacherContainerClient.GetBlobClient(blobName);

        if (await blobClient.ExistsAsync())
        {
            await blobClient.DeleteAsync();
            return true;
        }

        return false;
    }

    public async Task<(bool isSuccessful, string? errorMessage, string? imageUrl)> UploadGalleryImageAsync(IFormFile file, Guid imageId)
    {
        string extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        string blobName = $"image-{imageId}{extension}";

        BlobClient blobClient = _galleryContainerClient.GetBlobClient(blobName);

        await using (var stream = file.OpenReadStream())
        {
            await blobClient.UploadAsync(stream, overwrite: true);
        }

        return (true, string.Empty, blobClient.Uri.ToString());
    }

    public async Task<bool> DeleteGalleryImageAsync(string imageUrl)
    {
        Uri uri = new Uri(imageUrl);
        string blobName = Path.GetFileName(uri.LocalPath);

        BlobClient blobClient = _galleryContainerClient.GetBlobClient(blobName);

        if (await blobClient.ExistsAsync())
        {
            await blobClient.DeleteAsync();
            return true;
        }

        return false;
    }

    public async Task<(bool isSuccessful, string? errorMessage, string? imageUrl)> UploadNewsImageAsync(IFormFile file, string title)
    {
        string extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        string blobName = $"news-{title.ToLower()}{extension}";

        BlobClient blobClient = _newsContainerClient.GetBlobClient(blobName);

        await using (var stream = file.OpenReadStream())
        {
            await blobClient.UploadAsync(stream, overwrite: true);
        }

        return (true, string.Empty, blobClient.Uri.ToString());
    }

    public async Task<bool> DeleteNewsImageAsync(string imageUrl)
    {
        Uri uri = new Uri(imageUrl);
        string blobName = Path.GetFileName(uri.LocalPath);

        BlobClient blobClient = _newsContainerClient.GetBlobClient(blobName);

        if (await blobClient.ExistsAsync())
        {
            await blobClient.DeleteAsync();
            return true;
        }

        return false;
    }
}