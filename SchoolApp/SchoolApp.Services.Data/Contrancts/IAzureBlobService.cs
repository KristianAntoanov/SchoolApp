using Microsoft.AspNetCore.Http;

namespace SchoolApp.Services.Data.Contrancts
{
	public interface IAzureBlobService
    {
        Task<(bool isSuccessful, string? errorMessage, string? imageUrl)> UploadTeacherImageAsync(IFormFile file, string firstName, string lastName);

        Task<bool> DeleteTeacherImageAsync(string imageUrl);

        Task<(bool isSuccessful, string? errorMessage, string? imageUrl)> UploadGalleryImageAsync(IFormFile file, Guid imageId);

        Task<bool> DeleteGalleryImageAsync(string imageUrl);
    }
}