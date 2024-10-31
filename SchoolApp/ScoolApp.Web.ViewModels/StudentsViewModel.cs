namespace SchoolApp.Web.ViewModels
{
	public class StudentsViewModel
	{
		public string FirstName { get; set; } = null!;

		public string LastName { get; set; } = null!;

		public IEnumerable<GradeViewModel> Grades { get; set; }
			= new HashSet<GradeViewModel>();
	}
}