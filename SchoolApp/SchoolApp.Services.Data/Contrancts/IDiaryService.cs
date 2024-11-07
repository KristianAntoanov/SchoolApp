using SchoolApp.Web.ViewModels;

namespace SchoolApp.Services.Data.Contrancts
{
	public interface IDiaryService
	{
        Task<IEnumerable<DiaryIndexViewModel>> IndexGetAllClasses();

        Task<IEnumerable<StudentGradesViewModel>> GetGradeContent(int classId, int subjectId);

        Task<IEnumerable<SubjectsViewModel>> GetClassContent(int classId);

        Task<IEnumerable<StudentRemarksViewModel>> GetRemarksContent(int classId);

        Task<IEnumerable<StudentAbsencesViewModel>> GetAbsencesContent(int classId);

        Task<DiaryGradeAddViewModel> GetClassNames(int classId, int subjectId);

        Task<bool> AddGradesToStudents(int classId, DiaryGradeAddViewModel model);
    }
}