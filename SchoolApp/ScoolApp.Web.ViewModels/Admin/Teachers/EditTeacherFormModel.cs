using Microsoft.AspNetCore.Http;

using System.ComponentModel.DataAnnotations;

using static SchoolApp.Common.EntityValidationConstants.Teacher;
using static SchoolApp.Common.ErrorMessages;

namespace SchoolApp.Web.ViewModels.Admin.Teachers;

public class EditTeacherFormModel
{
    [Required]
    public string Id { get; set; } = null!;

    [Required(ErrorMessage = TeacherNameRequiredMessage)]
    [StringLength(NameMaxLength, MinimumLength = NameMinLength,
        ErrorMessage = NameStringLengthMessage)]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = TeacherNameRequiredMessage)]
    [StringLength(NameMaxLength, MinimumLength = NameMinLength,
        ErrorMessage = NameStringLengthMessage)]
    public string LastName { get; set; } = null!;

    [Required(ErrorMessage = TeacherJobTitleRequiredMessage)]
    [StringLength(JobTitleMaxLength, MinimumLength = JobTitleMinLength,
        ErrorMessage = NameStringLengthMessage)]
    public string JobTitle { get; set; } = null!;

    public string? CurrentImageUrl { get; set; }

    public string? CurrentImageFileName { get; set; }

    public IFormFile? Image { get; set; }

    [Required(ErrorMessage = TeacherSubjectRequiredMessage)]
    public IEnumerable<SubjectsViewModel> AvailableSubjects { get; set; }
        = new HashSet<SubjectsViewModel>();

    public IEnumerable<int> SelectedSubjects { get; set; }
        = new HashSet<int>();
}