using System.ComponentModel.DataAnnotations;

using static SchoolApp.Common.EntityValidationConstants.Student;
using static SchoolApp.Common.ErrorMessages;

namespace SchoolApp.Web.ViewModels.Admin.Students
{
    public class EditStudentFormModel
	{
        public int Id { get; set; }

        [Required(ErrorMessage = StudentNameRequiredMessage)]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength,
            ErrorMessage = NameStringLengthMessage)]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = StudentNameRequiredMessage)]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength,
            ErrorMessage = NameStringLengthMessage)]
        public string MiddleName { get; set; } = null!;

        [Required(ErrorMessage = StudentNameRequiredMessage)]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength,
            ErrorMessage = NameStringLengthMessage)]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = ClassRequiredMessage)]
        public int ClassId { get; set; }

        public IList<ClassesViewModel> AvailableClasses { get; set; }
            = new List<ClassesViewModel>();
    }
}