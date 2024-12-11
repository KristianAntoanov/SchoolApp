namespace SchoolApp.Web.ViewModels.Admin.Students;

public class StudentsViewModel
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string MiddleName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public IEnumerable<GradeViewModel> Grades { get; set; }
    = new HashSet<GradeViewModel>();
}