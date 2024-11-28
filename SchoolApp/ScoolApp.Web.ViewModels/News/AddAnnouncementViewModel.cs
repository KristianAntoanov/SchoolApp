using SchoolApp.Common;
using System.ComponentModel.DataAnnotations;

namespace SchoolApp.Web.ViewModels.News
{
	public class AddAnnouncementViewModel
	{
        [Display(Name = "Заглавие")]
        [Required(ErrorMessage = "Полето {0} е задължително")]
        [StringLength(100, ErrorMessage = "{0}то трябва да е между {2} и {1} символа", MinimumLength = 5)]
        public string Title { get; set; } = null!;

        [Display(Name = "Съдържание")]
        [Required(ErrorMessage = "Полето {0} е задължително")]
        [StringLength(500, ErrorMessage = "{0}то трябва да е между {2} и {1} символа", MinimumLength = 10)]
        public string Content { get; set; } = null!;
    }
}