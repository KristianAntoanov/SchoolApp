using System.ComponentModel.DataAnnotations;

namespace SchoolApp.Web.ViewModels.Diary.AddForms
{
	public class StudentGradeFormModel
    {
        public int Id { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        [Range(0, 6)]
        public int Grade { get; set; }
    }
}