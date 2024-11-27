using SchoolApp.Web.ViewModels.Gallery;

namespace SchoolApp.Services.Data.Contrancts
{
	public interface IGalleryService
    {
        Task<IEnumerable<AlbumViewModel>> GetAllAlbumsWithImagesAsync();
    }
}