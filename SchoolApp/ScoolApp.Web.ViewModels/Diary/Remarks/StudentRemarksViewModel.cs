namespace SchoolApp.Web.ViewModels;

public class StudentRemarksViewModel
{
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public IEnumerable<RemarksViewModel> Remarks { get; set; }
        = new HashSet<RemarksViewModel>();
}