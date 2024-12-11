using System.ComponentModel.DataAnnotations;

using static SchoolApp.Common.EntityValidationConstants.Announcement;
using static SchoolApp.Common.ErrorMessages;

namespace SchoolApp.Web.ViewModels.News;

public class AddAnnouncementViewModel
{
    [Required(ErrorMessage = AnnouncementTitleRequiredMessage)]
    [StringLength(TitleMaxLength, MinimumLength = TitleMinLength,
        ErrorMessage = AnnouncementTitleStringLengthMessage)]
    public string Title { get; set; } = null!;

    [Required(ErrorMessage = AnnouncementContentRequiredMessage)]
    [StringLength(ContentMaxLength, MinimumLength = ContentMinLength,
        ErrorMessage = AnnouncementContentStringLengthMessage)]
    public string Content { get; set; } = null!;
}