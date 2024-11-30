using Microsoft.AspNetCore.Http;
using SchoolApp.Web.ViewModels.Admin.Roles;
using SchoolApp.Web.ViewModels.Admin.Teachers;

namespace SchoolApp.Services.Data.Contrancts
{
	public interface IAdminTeachersService
	{
        Task<IEnumerable<TeacherViewModel>> GetAllTeachersAsync();

        Task<bool> DeleteTeacherAsync(string id);

        Task<IEnumerable<SubjectsViewModel>> GetAvailableSubjectsAsync();

        Task<(bool isSuccessful, string? errorMessage)> AddTeacherAsync(AddTeacherFormModel model);

        Task<EditTeacherFormModel?> GetTeacherForEditAsync(string id);

        Task<(bool isSuccessful, string? errorMessage)> EditTeacherAsync(EditTeacherFormModel model);
    }
}