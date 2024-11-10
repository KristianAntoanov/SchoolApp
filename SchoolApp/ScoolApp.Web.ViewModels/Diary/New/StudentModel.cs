namespace SchoolApp.Web.ViewModels.Diary.New
{
	public class StudentModel
	{
        public int Id { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public int? Grade { get; set; }

        public string? RemarkText { get; set; }

        public bool IsChecked { get; set; } = false;
    }
}