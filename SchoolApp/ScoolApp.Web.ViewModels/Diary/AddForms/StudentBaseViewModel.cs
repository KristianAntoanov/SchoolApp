namespace SchoolApp.Web.ViewModels.Diary.AddForms
{
	public class StudentBaseViewModel
	{
		public DateTime AddedOn { get; set; }

		public int SubjectId { get; set; }

		public IEnumerable<SubjectFormModel> Subjects { get; set; }
			= new HashSet<SubjectFormModel>();
	}
}