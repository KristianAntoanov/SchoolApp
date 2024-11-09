namespace SchoolApp.Web.ViewModels
{
	public class AbsencesViewModel
	{
        public string SubjectName { get; set; } = null!;

        public bool IsExcused { get; set; }

        public string AddedOn { get; set; } = null!;
    }
}