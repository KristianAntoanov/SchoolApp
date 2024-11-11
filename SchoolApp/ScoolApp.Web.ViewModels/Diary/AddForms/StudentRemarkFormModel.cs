namespace SchoolApp.Web.ViewModels.Diary.AddForms
{
	public class StudentRemarkFormModel
	{
        public int Id { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? FullName => $"{FirstName} {LastName}";
    }
}