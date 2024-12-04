using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

using SchoolApp.Data.Models;
using SchoolApp.Web.Infrastructure.ValidationAttributes;

using static SchoolApp.Common.EntityValidationConstants.News;
using static SchoolApp.Common.ErrorMessages;

namespace SchoolApp.Web.ViewModels.News
{
    public class AddNewsViewModel
	{
        [Required(ErrorMessage = NewsTitleRequiredMessage)]
        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength,
            ErrorMessage = NewsTitleStringLengthMessage)]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = NewsContentRequiredMessage)]
        [StringLength(ContentMaxLength, MinimumLength = ContentMinLength,
            ErrorMessage = NewsContentStringLengthMessage)]
        public string Content { get; set; } = null!;

        [AllowedExtensions(ImageAllowedExtensionJPG, ImageAllowedExtensionJPEG, ImageAllowedExtensionPNG)]
        [MaxFileSize(2 * 1024 * 1024)]
        public IFormFile? Image { get; set; }

        [Required]
        public NewsCategory Category { get; set; }
    }
}