using Microsoft.AspNetCore.Http;

namespace SchoolApp.Services.Data.Contrancts
{
	public interface IAzureBlobService
    {
        Task<(bool isSuccessful, string? errorMessage, string? imageUrl)> UploadTeacherImageAsync(IFormFile file, string firstName, string lastName);

        Task<bool> DeleteTeacherImageAsync(string imageUrl);
    }
}