namespace SchoolApp.Web.ViewModels.Diary.NewTest
{
	public class StudentBaseViewModel
	{
		public DateTime AddedOn { get; set; }

		public int SubjectId { get; set; }

		public IEnumerable<SubjectViewModel> Subjects { get; set; }
			= new HashSet<SubjectViewModel>();
	}
}