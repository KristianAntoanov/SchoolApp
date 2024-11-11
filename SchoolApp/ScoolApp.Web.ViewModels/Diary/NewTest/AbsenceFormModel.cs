namespace SchoolApp.Web.ViewModels.Diary.NewTest
{
	public class AbsenceFormModel : StudentBaseViewModel
	{
        public IList<StudentAbcenseFormModel> Students { get; set; }
            = new List<StudentAbcenseFormModel>();
    }
}