using System.ComponentModel.DataAnnotations;

namespace SchoolApp.Web.ViewModels.Admin
{
	public class ClassesViewModel
	{
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;
    }
}