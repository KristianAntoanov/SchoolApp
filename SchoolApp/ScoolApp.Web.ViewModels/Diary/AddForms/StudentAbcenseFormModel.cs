namespace SchoolApp.Web.ViewModels.Diary.AddForms
{
	public class StudentAbcenseFormModel
	{
        public int Id { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public bool IsChecked { get; set; } = false;
    }
}