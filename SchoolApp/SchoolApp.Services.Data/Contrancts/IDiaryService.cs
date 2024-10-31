using SchoolApp.Web.ViewModels;

namespace SchoolApp.Services.Data.Contrancts
{
	public interface IDiaryService
	{
        Task<IEnumerable<DiaryIndexViewModel>> IndexGetAllClasses();

        Task<IEnumerable<SubjectsViewModel>> GetClassContent();
    }
}