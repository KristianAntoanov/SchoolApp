using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Http;

using SchoolApp.Web.Infrastructure.ValidationAttributes;

using static SchoolApp.Common.ErrorMessages;

namespace SchoolApp.Web.ViewModels.Admin.Gallery.Components;

public class AddAlbumImageFormModel
{
    public string AlbumId { get; set; } = null!;

    [Required(ErrorMessage = GalleryImageRequiredMessage)]
    [AllowedExtensions(ImageAllowedExtensionJPG, ImageAllowedExtensionJPEG, ImageAllowedExtensionPNG)]
    [MaxFileSize(2 * 1024 * 1024, ErrorMessage = ImageFileLengthMessage)]
    public IFormFile Image { get; set; } = null!;
}