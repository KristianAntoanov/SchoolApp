namespace SchoolApp.Web.ViewModels.Diary.NewTest
{
	public class GradeFormModel : StudentBaseViewModel
    {
		public IList<StudentGradeFormModel> Students { get; set; }
			= new List<StudentGradeFormModel>();
	}
}