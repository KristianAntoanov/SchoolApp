using Microsoft.AspNetCore.Http;
using SchoolApp.Data.Models;

namespace SchoolApp.Web.ViewModels.News
{
	public class AddNewsViewModel
	{
        public string Title { get; set; } = null!;

        public string Content { get; set; } = null!;

        public IFormFile? Image { get; set; }

        public NewsCategory Category { get; set; }
    }
}