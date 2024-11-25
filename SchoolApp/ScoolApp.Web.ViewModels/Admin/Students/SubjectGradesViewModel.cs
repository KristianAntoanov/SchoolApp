namespace SchoolApp.Web.ViewModels.Admin.Students
{
	public class SubjectGradesViewModel
	{
        public int SubjectId { get; set; }

        public string SubjectName { get; set; } = null!;

        public IList<GradeManagementViewModel> Grades { get; set; }
            = new List<GradeManagementViewModel>();
    }
}