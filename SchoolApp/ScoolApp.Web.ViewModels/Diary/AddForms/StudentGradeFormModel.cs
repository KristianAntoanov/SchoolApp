using System.ComponentModel.DataAnnotations;
using static SchoolApp.Common.ApplicationConstants;

namespace SchoolApp.Web.ViewModels.Diary.AddForms
{
	public class StudentGradeFormModel
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

        [Range(GradeMinValue, GradeMaxValue)]
        public int Grade { get; set; }
    }
}