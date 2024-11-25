using SchoolApp.Web.ViewModels.Admin.Students;

namespace SchoolApp.Services.Data.Contrancts
{
	public interface IAdminStudentsService
    {
        Task<PaginatedList<StudentsViewModel>> GetAllStudentsAsync(int pageNumber, int pageSize, string searchTerm = null);

        Task<bool> DeleteStudent(int id);

        Task<EditStudentFormModel?> GetStudentForEditAsync(int id);

        Task<IList<ClassesViewModel>> GetAvailableClassesAsync();

        Task<bool> UpdateStudentAsync(EditStudentFormModel model);

        Task<StudentGradesManagementViewModel?> GetStudentGradesAsync(int studentId);

        Task<bool> DeleteGradeAsync(int gradeId);

        Task<bool> AddStudentAsync(AddStudentFormModel model, string userId);
    }
}