using System.ComponentModel.DataAnnotations;

using static SchoolApp.Common.ErrorMessages;

namespace SchoolApp.Web.ViewModels.Admin.Gallery;

public class MenageAlbumImageViewModel
{
    [Required]
    public string Id { get; set; } = null!;

    [Required(ErrorMessage = GalleryImageRequiredMessage)]
    public string ImageUrl { get; set; } = null!;
}