using SchoolApp.Web.Infrastructure.ValidationAttributes;
using System.ComponentModel.DataAnnotations;
using static SchoolApp.Common.ErrorMessages;

namespace SchoolApp.Web.ViewModels.Diary.Remarks
{
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
        public string RemarkText { get; set; } = null!;
    }
}