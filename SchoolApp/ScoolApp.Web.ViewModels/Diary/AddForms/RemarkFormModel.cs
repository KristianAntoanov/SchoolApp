using System.ComponentModel.DataAnnotations;

using static SchoolApp.Common.EntityValidationConstants.Remark;
using static SchoolApp.Common.ErrorMessages;

namespace SchoolApp.Web.ViewModels.Diary.AddForms
{
	public class RemarkFormModel : StudentBaseViewModel
    {
        [Required]
        public int StudentId { get; set; }

        [Required(ErrorMessage = RemarkRequiredMessage)]
        [StringLength(RemarkTextMaxLength, MinimumLength =RemarkTextMinLength,
            ErrorMessage = RemarkTextStringLengthMessage)]
        public string RemarkText { get; set; } = null!;

        public IList<StudentRemarkFormModel> Students { get; set; }
            = new List<StudentRemarkFormModel>();
    }
}