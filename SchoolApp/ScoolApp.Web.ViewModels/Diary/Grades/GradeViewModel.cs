using SchoolApp.Data.Models;

namespace SchoolApp.Web.ViewModels;

public class GradeViewModel
{
    public int GradeValue { get; set; }

    public DateTime GradeDate { get; set; }

    public string TeacherName { get; set; } = null!;

    public GradeType? GradeType { get; set; }
}