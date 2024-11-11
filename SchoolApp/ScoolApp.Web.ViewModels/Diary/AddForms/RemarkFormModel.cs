namespace SchoolApp.Web.ViewModels.Diary.AddForms
{
	public class RemarkFormModel : StudentBaseViewModel
    {
        public int StudentId { get; set; }

        public string RemarkText { get; set; } = null!;

        public IList<StudentRemarkFormModel> Students { get; set; }
            = new List<StudentRemarkFormModel>();
    }
}