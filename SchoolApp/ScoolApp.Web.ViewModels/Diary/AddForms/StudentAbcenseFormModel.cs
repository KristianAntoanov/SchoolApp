using System.ComponentModel.DataAnnotations;
using static SchoolApp.Common.ApplicationConstants;

namespace SchoolApp.Web.ViewModels.Diary.AddForms
{
	public class StudentAbcenseFormModel
	{
        [Required]
        public int Id { get; set; }

        [Required]
        [MinLength(NameMinLength)]
        [MaxLength(NameMaxLength)]
        public string FirstName { get; set; } = null!;

        [Required]
        [MinLength(NameMinLength)]
        [MaxLength(NameMaxLength)]
        public string LastName { get; set; } = null!;

        public bool IsChecked { get; set; } = false;
    }
}