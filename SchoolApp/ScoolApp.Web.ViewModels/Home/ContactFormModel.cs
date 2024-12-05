using System.ComponentModel.DataAnnotations;

using static SchoolApp.Common.ApplicationConstants;
using static SchoolApp.Common.ErrorMessages;

namespace SchoolApp.Web.ViewModels.Home
{
	public class ContactFormModel
	{
        [Required(ErrorMessage = ContactNameRequiredMessage)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = ContactEmailRequiredMessage)]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = ValidEmailMessage)]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = ContactSubjectRequiredMessage)]
        public string Subject { get; set; } = null!;

        [Required(ErrorMessage = ContactMessageRequiredMessage)]
        [StringLength(ContactMessageMaxLenght, MinimumLength = ContactMessageMinLenght,
            ErrorMessage = ContactMessageStringLengthMessage)]
        public string Message { get; set; } = null!;
    }
}