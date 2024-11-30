namespace SchoolApp.Web.ViewModels.Admin.Students
{
	public class StudentGradesManagementViewModel
	{
        public int StudentId { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public IEnumerable<SubjectGradesViewModel> SubjectGrades { get; set; }
            = new List<SubjectGradesViewModel>();
    }
}