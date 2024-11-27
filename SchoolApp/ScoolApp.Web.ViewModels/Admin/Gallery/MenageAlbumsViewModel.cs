using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

using static SchoolApp.Common.ErrorMessages;

namespace SchoolApp.Web.ViewModels.Admin.Gallery
{
	public class MenageAlbumsViewModel
	{
        public string Id { get; set; } = null!;
        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        [Required(ErrorMessage = GalleryImageRequiredMessage)]
        public IFormFile Image { get; set; } = null!;

        public IList<MenageAlbumImageViewModel> Images { get; set; }
            = new List<MenageAlbumImageViewModel>();
    }
}