namespace SchoolApp.Web.ViewModels.Diary.AddForms
{
	public class AbsenceFormModel : StudentBaseViewModel
	{
        public IList<StudentAbcenseFormModel> Students { get; set; }
            = new List<StudentAbcenseFormModel>();
    }
}