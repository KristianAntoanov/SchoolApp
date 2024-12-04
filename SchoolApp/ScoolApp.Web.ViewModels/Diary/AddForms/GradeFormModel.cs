using SchoolApp.Data.Models;

namespace SchoolApp.Web.ViewModels.Diary.AddForms
{
	public class GradeFormModel : StudentBaseViewModel
    {
		public IList<StudentGradeFormModel> Students { get; set; }
			= new List<StudentGradeFormModel>();

        public GradeType? GradeType { get; set; }
    }
}