using System.ComponentModel.DataAnnotations;

using static SchoolApp.Common.EntityValidationConstants.Student;
using static SchoolApp.Common.EntityValidationConstants.Grade;
using static SchoolApp.Common.ErrorMessages;

namespace SchoolApp.Web.ViewModels.Diary.AddForms;

public class StudentGradeFormModel
{
    [Required]
    public int Id { get; set; }

    [Required(ErrorMessage = StudentNameRequiredMessage)]
    [StringLength(NameMaxLength, MinimumLength = NameMinLength,
        ErrorMessage = NameStringLengthMessage)]
    public string FirstName { get; set; } = null!;

    [Required]
    [MinLength(NameMinLength)]
    [MaxLength(NameMaxLength)]
    public string LastName { get; set; } = null!;

    [Range(GradeMinValue, GradeMaxValue)]
    public int Grade { get; set; }
}