namespace SchoolApp.Web.ViewModels.Admin.Roles;

public class UserRolesViewModel
{
    public Guid Id { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public Guid? TeacherId { get; set; }

    public IEnumerable<TeacherDropdownViewModel> AvailableTeachers { get; set; }
        = new List<TeacherDropdownViewModel>();

    public IEnumerable<string> UserRoles { get; set; }
        = new List<string>();

    public IEnumerable<string> AllRoles { get; set; }
        = new List<string>();
}