using Microsoft.AspNetCore.Http;
using SchoolApp.Common;
using SchoolApp.Web.Infrastructure.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

using static SchoolApp.Common.EntityValidationConstants.Teacher;
using static SchoolApp.Common.ErrorMessages;

namespace SchoolApp.Web.ViewModels.Admin.Teachers
{
	public class AddTeacherFormModel
	{
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
            ErrorMessage = JobTitleStringLengthMessage)]
        public string JobTitle { get; set; } = null!;

        [Required(ErrorMessage = TeacherImageRequiredMessage)]
        [AllowedExtensions(ImageAllowedExtensionJPG, ImageAllowedExtensionJPEG, ImageAllowedExtensionPNG)]
        [MaxFileSize(2 * 1024 * 1024, ErrorMessage = ImageFileLengthMessage)]
        public IFormFile Image { get; set; } = null!;

        [Required(ErrorMessage = TeacherSubjectRequiredMessage)]
        public IEnumerable<SubjectsViewModel> AvailableSubjects { get; set; }
            = new HashSet<SubjectsViewModel>();

        public IEnumerable<int> SelectedSubjects { get; set; }
            = new HashSet<int>();
    }
}