using System.ComponentModel.DataAnnotations;

namespace SchoolApp.Web.ViewModels
{
	public class SubjectViewModel
	{
		[Required]
		public int Id { get; set; }

		[Required]
		public string Name { get; set; } = null!;
	}
}