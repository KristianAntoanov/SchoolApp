using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using SchoolApp.Data.Models;
using SchoolApp.Data.Repository.Contracts;
using SchoolApp.Services.Data.Contrancts;
using SchoolApp.Web.ViewModels.Admin.Roles;

namespace SchoolApp.Services.Data
{
	public class AdminUserRolesService : IAdminUserRolesService
    {
        private readonly IRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;


        public AdminUserRolesService(IRepository repository, UserManager<ApplicationUser> userManager,
                                        RoleManager<ApplicationRole> roleManager)
        {
            _repository = repository;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IEnumerable<UserRolesViewModel>> GetAllUsersWithRolesAsync()
        {
            IEnumerable<ApplicationUser> users = await _repository.GetAllAsync<ApplicationUser>();
            List<UserRolesViewModel> viewModels = new List<UserRolesViewModel>();

            foreach (var user in users)
            {
                IList<string> roles = await _userManager.GetRolesAsync(user);
                Guid? teacherId = await GetTeacherIdByUserIdAsync(user.Id);

                // Подаваме текущия teacherId към метода
                var availableTeachers = await GetAvailableTeachersForAssignmentAsync(teacherId);

                viewModels.Add(new UserRolesViewModel
                {
                    Id = user.Id,
                    Username = user.UserName!,
                    Email = user.Email!,
                    TeacherId = teacherId,
                    AvailableTeachers = availableTeachers,
                    UserRoles = roles,
                    AllRoles = _roleManager.Roles.Select(r => r.Name!)
                });
            }

            return viewModels;
        }

        public async Task<(bool success, string message)> UpdateUserTeacherAsync(Guid userId, Guid? teacherId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return (false, "Потребителят не беше намерен.");
            }

            if (!await UpdateTeacherUserAsync(userId, teacherId))
            {
                return (false, "Възникна грешка при обновяването на връзката с учител.");
            }

            if (teacherId.HasValue)
            {
                if (!await _userManager.IsInRoleAsync(user, "Teacher"))
                {
                    await _userManager.AddToRoleAsync(user, "Teacher");
                }
            }
            else
            {
                await _userManager.RemoveFromRoleAsync(user, "Teacher");
            }

            return (true, "Успешно обновена връзка с учител.");
        }

        public async Task<(bool success, string message)> UpdateUserRolesAsync(Guid userId, List<string> roles)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return (false, "Потребителят не беше намерен.");
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);

            if (roles.Count != 0)
            {
                await _userManager.AddToRolesAsync(user, roles);
            }

            return (true, "Успешно обновени роли.");
        }

        public async Task<bool> UpdateTeacherUserAsync(Guid userId, Guid? teacherId)
        {
            var currentTeacher = await _repository
                .GetAllAttached<Teacher>()
                .FirstOrDefaultAsync(t => t.ApplicationUserId == userId);

            if (currentTeacher != null)
            {
                currentTeacher.ApplicationUserId = null;
                await _repository.UpdateAsync(currentTeacher);
            }

            if (teacherId.HasValue)
            {
                var newTeacher = await _repository
                    .GetAllAttached<Teacher>()
                    .FirstOrDefaultAsync(t => t.GuidId == teacherId.Value);

                if (newTeacher != null)
                {
                    newTeacher.ApplicationUserId = userId;
                    return await _repository.UpdateAsync(newTeacher);
                }
            }
            return true;
        }

        public async Task<Guid?> GetTeacherIdByUserIdAsync(Guid userId)
            => await _repository
                .GetAllAttached<Teacher>()
                .Where(t => t.ApplicationUserId == userId)
                .Select(t => t.GuidId)
                .FirstOrDefaultAsync();

        public async Task<IEnumerable<TeacherDropdownViewModel>> GetAvailableTeachersForAssignmentAsync(Guid? currentTeacherId = null)
            => await _repository
                .GetAllAttached<Teacher>()
                .Where(t => t.ApplicationUserId == null || t.GuidId == currentTeacherId)
                .OrderBy(t => t.FirstName)
                .ThenBy(t => t.LastName)
                .Select(t => new TeacherDropdownViewModel
                {
                    Id = t.GuidId,
                    DisplayName = $"{t.FirstName} {t.LastName} - {t.JobTitle}"
                })
                .ToListAsync();

        public async Task<TeacherBasicInfoViewModel?> GetTeacherBasicInfoAsync(Guid teacherId)
            => await _repository
                .GetAllAttached<Teacher>()
                .Where(t => t.GuidId == teacherId)
                .Select(t => new TeacherBasicInfoViewModel
                {
                    FirstName = t.FirstName,
                    LastName = t.LastName,
                    JobTitle = t.JobTitle
                })
                .FirstOrDefaultAsync();
    }
}