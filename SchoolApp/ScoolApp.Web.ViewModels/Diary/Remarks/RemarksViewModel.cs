namespace SchoolApp.Web.ViewModels
{
	public class RemarksViewModel
	{
		public int Id { get; set; }

        public string SubjectName { get; set; } = null!;

        public string RemarkText { get; set; } = null!;

        public string AddedOn { get; set; } = null!;

        public string TeacherName { get; set; } = null!;
    }
}