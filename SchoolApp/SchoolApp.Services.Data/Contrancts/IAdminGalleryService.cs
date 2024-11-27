using Microsoft.AspNetCore.Http;
using SchoolApp.Web.ViewModels.Admin.Gallery;

namespace SchoolApp.Services.Data.Contrancts
{
    public interface IAdminGalleryService
	{
        Task<IEnumerable<MenageAlbumsViewModel>> GetAllAlbumsWithImagesAsync();

        Task<(bool success, string message)> AddAlbumAsync(AddAlbumViewModel model);

        Task<MenageAlbumsViewModel?> GetDetailsForAlbumAsync(string id);

        Task<(bool success, string message)> AddImagesAsync(string albumId, IFormFile file);

        Task<(bool success, string message)> DeleteImageAsync(string imageId);

        Task<(bool success, string message)> DeleteAlbumAsync(string albumId);
    }
}