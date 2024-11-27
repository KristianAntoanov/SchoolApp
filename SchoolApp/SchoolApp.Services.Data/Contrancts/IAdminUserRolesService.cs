using SchoolApp.Web.ViewModels.Admin.Roles;

namespace SchoolApp.Services.Data.Contrancts
{
	public interface IAdminUserRolesService
	{
        Task<IEnumerable<UserRolesViewModel>> GetAllUsersWithRolesAsync();

        Task<(bool success, string message)> UpdateUserTeacherAsync(Guid userId, Guid? teacherId);

        Task<(bool success, string message)> UpdateUserRolesAsync(Guid userId, List<string> roles);

        Task<bool> UpdateTeacherUserAsync(Guid userId, Guid? teacherId);

        Task<Guid?> GetTeacherIdByUserIdAsync(Guid userId);

        Task<IEnumerable<TeacherDropdownViewModel>> GetAvailableTeachersForAssignmentAsync(Guid? currentTeacherId = null);

        Task<TeacherBasicInfoViewModel?> GetTeacherBasicInfoAsync(Guid teacherId);
    }
}