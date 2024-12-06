namespace SchoolApp.Web.ViewModels.Team;

public class TeachersViewModel
{
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Photo { get; set; } = null!;

    public string JobTitle { get; set; } = null!;

    public IEnumerable<TeacherSubjectsViewModel> Subjects { get; set; }
        = new HashSet<TeacherSubjectsViewModel>();
}