using System.ComponentModel.DataAnnotations;

using static SchoolApp.Common.EntityValidationConstants.Student;
using static SchoolApp.Common.ErrorMessages;

namespace SchoolApp.Web.ViewModels.Diary.AddForms;

public class StudentAbcenseFormModel
{
    [Required]
    public int Id { get; set; }

    [Required(ErrorMessage = StudentNameRequiredMessage)]
    [StringLength(NameMaxLength, MinimumLength = NameMinLength,
        ErrorMessage = NameStringLengthMessage)]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = StudentNameRequiredMessage)]
    [StringLength(NameMaxLength, MinimumLength = NameMinLength,
        ErrorMessage = NameStringLengthMessage)]
    public string LastName { get; set; } = null!;

    [Required]
    public bool IsChecked { get; set; } = false;
}