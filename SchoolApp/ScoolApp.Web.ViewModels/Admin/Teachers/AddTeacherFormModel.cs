using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

using static SchoolApp.Common.EntityValidationConstants.Teacher;
using static SchoolApp.Common.ErrorMessages;

namespace SchoolApp.Web.ViewModels.Admin.Teachers
{
	public class AddTeacherFormModel
	{
        [Required(ErrorMessage = TeacherNameRequiredMessage)]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength,
            ErrorMessage = "Името трябва да е между {2} и {1} символа")]
        [Display(Name = "Име")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = TeacherNameRequiredMessage)]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength,
            ErrorMessage = "Фамилията трябва да е между {2} и {1} символа")]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = TeacherJobTitleRequiredMessage)]
        [StringLength(JobTitleMaxLength, MinimumLength = JobTitleMinLength,
            ErrorMessage = "Длъжността трябва да е между {2} и {1} символа")]
        [Display(Name = "Длъжност")]
        public string JobTitle { get; set; } = null!;

        [Required(ErrorMessage = TeacherImageRequiredMessage)]
        [Display(Name = "Снимка")]
        public IFormFile Image { get; set; } = null!;

        [Required(ErrorMessage = TeacherSubjectRequiredMessage)]
        [Display(Name = "Предмети")]
        public IEnumerable<SubjectsViewModel> AvailableSubjects { get; set; }
            = new HashSet<SubjectsViewModel>();

        public IEnumerable<int> SelectedSubjects { get; set; }
            = new HashSet<int>();
    }
}