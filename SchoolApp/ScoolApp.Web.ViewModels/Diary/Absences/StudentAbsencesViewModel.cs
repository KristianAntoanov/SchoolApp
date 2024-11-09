namespace SchoolApp.Web.ViewModels
{
	public class StudentAbsencesViewModel
	{
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public IEnumerable<AbsencesViewModel> Absences { get; set; }
            = new HashSet<AbsencesViewModel>();
    }
}