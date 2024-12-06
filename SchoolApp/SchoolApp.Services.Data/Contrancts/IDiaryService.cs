using SchoolApp.Data.Models;
using SchoolApp.Web.ViewModels;
using SchoolApp.Web.ViewModels.Diary.AddForms;
using SchoolApp.Web.ViewModels.Diary.Remarks;

namespace SchoolApp.Services.Data.Contrancts
{
	public interface IDiaryService
	{
        Task<IEnumerable<DiaryIndexViewModel>> IndexGetAllClassesAsync();

        Task<IEnumerable<StudentGradesViewModel>> GetGradeContentAsync(int classId, int subjectId);

        Task<IEnumerable<SubjectViewModel>> GetClassContentAsync(int classId);

        Task<IEnumerable<StudentRemarksViewModel>> GetRemarksContentAsync(int classId);

        Task<IEnumerable<StudentAbsencesViewModel>> GetAbsencesContentAsync(int classId);

        Task<T> GetClassStudentForAddAsync<T>(int classId, int subjectId) where T : StudentBaseViewModel, new();

        Task<bool> AddGradesAsync(string userId, GradeFormModel model);

        Task<bool> AddAbsenceAsync(AbsenceFormModel model);

        Task<bool> AddRemarkAsync(string userId, RemarkFormModel model);

        Task<bool> ExcuseAbsenceAsync(int id);

        Task<bool> DeleteAbsenceAsync(int id);

        Task<bool> DeleteRemarkAsync(int id);

        Task<EditRemarkViewModel?> GetRemarkByIdAsync(int id);

        Task<bool> EditRemarkAsync(EditRemarkViewModel model);

        Task<IEnumerable<SubjectViewModel>> GetSubjectsAsync();

        Task<IList<StudentRemarkFormModel>> GetStudentsAsync(RemarkFormModel model);
    }
}