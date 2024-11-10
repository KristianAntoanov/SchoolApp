using SchoolApp.Data.Models;
using SchoolApp.Web.ViewModels;
using SchoolApp.Web.ViewModels.Diary.New;

namespace SchoolApp.Services.Data.Contrancts
{
	public interface IDiaryService
	{
        Task<IEnumerable<DiaryIndexViewModel>> IndexGetAllClasses();

        Task<IEnumerable<StudentGradesViewModel>> GetGradeContent(int classId, int subjectId);

        Task<IEnumerable<SubjectsViewModel>> GetClassContent(int classId);

        Task<IEnumerable<StudentRemarksViewModel>> GetRemarksContent(int classId);

        Task<IEnumerable<StudentAbsencesViewModel>> GetAbsencesContent(int classId);

        Task<StudentRecordUpdateModel> GetClassStudentForGrades(int classId, int subjectId);

        Task<bool> AddStudentsRecords(string userId, StudentRecordUpdateModel model);

        Task<bool> AddGrades(string userId, StudentRecordUpdateModel model, Teacher? teacher);
    }
}