using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc.Rendering;

using SchoolApp.Data.Models;

using static SchoolApp.Common.ErrorMessages;

namespace SchoolApp.Web.ViewModels.Diary.AddForms;

public class GradeFormModel : StudentBaseViewModel
{
    public IList<StudentGradeFormModel> Students { get; set; }
        = new List<StudentGradeFormModel>();

    [Required(ErrorMessage = GradeTypeRequiredMessage)]
    public GradeType? GradeType { get; set; }

    public IEnumerable<SelectListItem> GradeTypes { get; set; }
        = new HashSet<SelectListItem>();
}