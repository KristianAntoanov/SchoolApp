using System.ComponentModel.DataAnnotations;

namespace SchoolApp.Web.ViewModels.Admin.Gallery
{
	public class AddAlbumViewModel
	{
        [Required(ErrorMessage = "Заглавието е задължително")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Заглавието трябва да е между 3 и 50 символа")]
        public string Title { get; set; } = null!;

        [StringLength(200, ErrorMessage = "Описанието не може да надвишава 200 символа")]
        public string? Description { get; set; }
    }
}