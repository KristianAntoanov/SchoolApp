using System.ComponentModel.DataAnnotations;

using static SchoolApp.Common.ErrorMessages;
using static SchoolApp.Common.EntityValidationConstants.Album;

namespace SchoolApp.Web.ViewModels.Admin.Gallery;

public class AddAlbumViewModel
{
    [Required(ErrorMessage = GalleryTitleRequiredMessage)]
    [StringLength(TitleMaxLength, MinimumLength = TitleMinLength,
        ErrorMessage = GalleryTitleStringLengthMessage)]
    public string Title { get; set; } = null!;

    [Required(ErrorMessage = GalleryDescriptionRequiredMessage)]
    [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength,
        ErrorMessage = GalleryDescriptionStringLengthMessage)]
    public string? Description { get; set; }
}