using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

using static SchoolApp.Common.ErrorMessages;
using static SchoolApp.Common.EntityValidationConstants.Album;
using SchoolApp.Web.Infrastructure.ValidationAttributes;

namespace SchoolApp.Web.ViewModels.Admin.Gallery
{
	public class MenageAlbumsViewModel
	{
        [Required]
        public string Id { get; set; } = null!;

        [Required(ErrorMessage = GalleryTitleRequiredMessage)]
        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength, ErrorMessage = GalleryTitleStringLengthMessage)]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = GalleryDescriptionRequiredMessage)]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength, ErrorMessage = GalleryDescriptionStringLengthMessage)]
        public string? Description { get; set; }

        [Required(ErrorMessage = GalleryImageRequiredMessage)]
        [FileExtensions(Extensions = "jpg,jpeg,png", ErrorMessage = "Проба дали излиза")]
        [MaxFileSize(2 * 1024 * 1024)]
        public IFormFile Image { get; set; } = null!;

        public IList<MenageAlbumImageViewModel> Images { get; set; }
            = new List<MenageAlbumImageViewModel>();
    }
}