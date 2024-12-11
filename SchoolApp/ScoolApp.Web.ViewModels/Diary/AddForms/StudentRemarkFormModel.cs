using System.ComponentModel.DataAnnotations;

namespace SchoolApp.Web.ViewModels.Diary.AddForms;

public class StudentRemarkFormModel
{
    [Required]
    public int Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? FullName => $"{FirstName} {LastName}";
}