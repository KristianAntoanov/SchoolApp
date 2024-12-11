namespace SchoolApp.Web.ViewModels;

public class AbsencesViewModel
{
    public int Id { get; set; }

    public string SubjectName { get; set; } = null!;

    public bool IsExcused { get; set; }

    public string AddedOn { get; set; } = null!;
}