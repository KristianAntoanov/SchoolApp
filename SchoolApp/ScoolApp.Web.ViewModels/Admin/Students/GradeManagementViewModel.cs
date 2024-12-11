using SchoolApp.Data.Models;

namespace SchoolApp.Web.ViewModels.Admin.Students;

public class GradeManagementViewModel
{
    public int Id { get; set; }

    public int GradeValue { get; set; }

    public DateTime GradeDate { get; set; }

    public GradeType? GradeType { get; set; }
}