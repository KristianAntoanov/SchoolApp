using SchoolApp.Web.ViewModels.Admin.Roles;

namespace SchoolApp.Services.Data.Contrancts
{
	public interface IAdminUserRolesService
	{
        Task<IEnumerable<UserRolesViewModel>> GetAllUsersWithRolesAsync();

        Task<(bool success, string message)> UpdateUserTeacherAsync(Guid userId, Guid? teacherId);

        Task<(bool success, string message)> UpdateUserRolesAsync(Guid userId, List<string> roles);

        Task<Guid?> GetTeacherIdByUserIdAsync(Guid userId);

        Task<IEnumerable<TeacherDropdownViewModel>> GetAvailableTeachersForAssignmentAsync();

        Task<bool> UpdateTeacherUserAsync(Guid userId, Guid? teacherId);

        Task<TeacherBasicInfoViewModel?> GetTeacherBasicInfoAsync(Guid teacherId);
    }
}