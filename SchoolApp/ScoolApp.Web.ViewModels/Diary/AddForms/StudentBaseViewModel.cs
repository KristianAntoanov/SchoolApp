using System.ComponentModel.DataAnnotations;
using SchoolApp.Web.Infrastructure.ValidationAttributes;
using static SchoolApp.Common.ErrorMessages;

namespace SchoolApp.Web.ViewModels.Diary.AddForms
{
	public class StudentBaseViewModel
	{
		[Required(ErrorMessage = DateRequiredMessage)]
		[IsDateValid(ErrorMessage = DateAfterMessage)]
        public DateTime AddedOn { get; set; }

		[Required]
		public int SubjectId { get; set; }

		public IEnumerable<SubjectViewModel> Subjects { get; set; }
			= new HashSet<SubjectViewModel>();
	}
}