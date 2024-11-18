using SchoolApp.Data.Models;
using SchoolApp.Web.ViewModels;
using SchoolApp.Web.ViewModels.Diary.AddForms;
using SchoolApp.Web.ViewModels.Diary.Remarks;

namespace SchoolApp.Services.Data.Contrancts
{
	public interface IDiaryService
	{
        Task<IEnumerable<DiaryIndexViewModel>> IndexGetAllClasses();

        Task<IEnumerable<StudentGradesViewModel>> GetGradeContent(int classId, int subjectId);

        Task<IEnumerable<SubjectViewModel>> GetClassContent(int classId);

        Task<IEnumerable<StudentRemarksViewModel>> GetRemarksContent(int classId);

        Task<IEnumerable<StudentAbsencesViewModel>> GetAbsencesContent(int classId);

        Task<T> GetClassStudentForGrades<T>(int classId, int subjectId) where T : StudentBaseViewModel, new();

        Task<bool> AddGrades(string userId, GradeFormModel model);

        Task<bool> AddAbsence(AbsenceFormModel model);

        Task<bool> AddRemark(string userId, RemarkFormModel model);

        Task<bool> ExcuseAbsence(int id);

        Task<bool> DeleteAbsence(int id);

        Task<bool> DeleteRemark(int id);

        Task<EditRemarkViewModel?> GetRemarkById(int id);

        Task<bool> EditRemark(EditRemarkViewModel model);

        IEnumerable<SubjectViewModel> GetSubjects();

        IList<StudentRemarkFormModel> GetStudents(RemarkFormModel model);
    }
}