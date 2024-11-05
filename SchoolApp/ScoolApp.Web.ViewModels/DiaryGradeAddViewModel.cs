using System.ComponentModel.DataAnnotations;

using static SchoolApp.Common.EntityValidationConstants.Grade;

namespace SchoolApp.Web.ViewModels
{
	public class DiaryGradeAddViewModel
    {
        [Required(ErrorMessage = AddedOnDateFormat)]
        [RegularExpression(@"^(0[1-9]|[12]\d|3[01])-(0[1-9]|1[0-2])-(\d{4})$",
             ErrorMessage = AddedOnDateFormat)]
        public DateTime AddedOn { get; set; }

		public IList<StudentVewModel> Students { get; set; }
			= new List<StudentVewModel>();

		[Required]
		public int SubjectId { get; set; }

		public IEnumerable<SubjectsViewModel> Subjects { get; set; }
			= new HashSet<SubjectsViewModel>();
	}
}