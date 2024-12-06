using System.ComponentModel.DataAnnotations;

using SchoolApp.Web.Infrastructure.ValidationAttributes;

using static SchoolApp.Common.ErrorMessages;
using static SchoolApp.Common.EntityValidationConstants.Remark;

namespace SchoolApp.Web.ViewModels.Diary.Remarks;

public class EditRemarkViewModel
{
    [Required]
    public int Id { get; set; }

    [Required(ErrorMessage = DateRequiredMessage)]
    [IsDateValid(ErrorMessage = DateAfterMessage)]
    public DateTime AddedOn { get; set; }

    [Required]
    public int SubjectId { get; set; }

    [Required]
    public int StudentId { get; set; }

    [Required(ErrorMessage = RemarkRequiredMessage)]
    [StringLength(RemarkTextMaxLength, MinimumLength = RemarkTextMinLength,
        ErrorMessage = RemarkTextStringLengthMessage)]
    public string RemarkText { get; set; } = null!;
}