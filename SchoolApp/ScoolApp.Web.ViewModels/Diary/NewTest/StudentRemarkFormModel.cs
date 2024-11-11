namespace SchoolApp.Web.ViewModels.Diary.NewTest
{
	public class StudentRemarkFormModel
	{
        public int Id { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? FullName => $"{FirstName} {LastName}";
    }
}