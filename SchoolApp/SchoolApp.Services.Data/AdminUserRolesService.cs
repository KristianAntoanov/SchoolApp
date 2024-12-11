using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using SchoolApp.Data.Models;
using SchoolApp.Data.Repository.Contracts;
using SchoolApp.Services.Data.Contrancts;
using SchoolApp.Web.ViewModels.Admin.Roles;
using SchoolApp.Web.ViewModels.Admin.Students;

using static SchoolApp.Common.ApplicationConstants;
using static SchoolApp.Common.TempDataMessages.UserRoles;

namespace SchoolApp.Services.Data;

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

    public async Task<PaginatedList<UserRolesViewModel>> GetPagedUsersWithRolesAsync(int pageNumber, int pageSize)
    {
        var totalItems = await _repository.GetAllAttached<ApplicationUser>().CountAsync();
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        var pagedUsers = await _repository
            .GetAllAttached<ApplicationUser>()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        List<UserRolesViewModel> viewModels = new List<UserRolesViewModel>();

        foreach (var user in pagedUsers)
        {
            IList<string> roles = await _userManager.GetRolesAsync(user);
            Guid? teacherId = await GetTeacherIdByUserIdAsync(user.Id);

            IEnumerable<TeacherDropdownViewModel> availableTeachers =
                await GetAvailableTeachersForAssignmentAsync(teacherId);

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

        return new PaginatedList<UserRolesViewModel>
        {
            Items = viewModels,
            PageNumber = pageNumber,
            TotalPages = totalPages,
            TotalItems = totalItems
        };
    }

    public async Task<(bool success, string message)> UpdateUserTeacherAsync(Guid userId, Guid? teacherId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user == null)
        {
            return (false, UserNotFoundMessage);
        }

        if (!await UpdateTeacherUserAsync(userId, teacherId))
        {
            return (false, TeacherLinkUpdateErrorMessage);
        }

        if (teacherId.HasValue)
        {
            if (!await _userManager.IsInRoleAsync(user, TeacherRole))
            {
                await _userManager.AddToRoleAsync(user, TeacherRole);
            }
        }
        else
        {
            await _userManager.RemoveFromRoleAsync(user, TeacherRole);
        }

        return (true, TeacherLinkUpdateSuccessMessage);
    }

    public async Task<(bool success, string message)> UpdateUserRolesAsync(Guid userId, List<string> roles)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user == null)
        {
            return (false, UserNotFoundMessage);
        }

        var currentRoles = await _userManager.GetRolesAsync(user);

        if (currentRoles.Any())
        {
            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeResult.Succeeded)
            {
                return (false, RolesUpdateErrorMessage);
            }
        }

        if (roles.Count != 0)
        {
            var addResult = await _userManager.AddToRolesAsync(user, roles);
            if (!addResult.Succeeded)
            {
                await _userManager.AddToRolesAsync(user, currentRoles);

                return (false, RolesUpdateErrorMessage);
            }
        }

        return (true, RolesUpdateSuccessMessage);
    }

    public async Task<bool> UpdateTeacherUserAsync(Guid userId, Guid? teacherId)
    {
        Teacher? currentTeacher = await _repository
            .GetAllAttached<Teacher>()
            .FirstOrDefaultAsync(t => t.ApplicationUserId == userId);

        if (currentTeacher != null)
        {
            currentTeacher.ApplicationUserId = null;
            await _repository.UpdateAsync(currentTeacher);
        }

        if (teacherId.HasValue)
        {
            Teacher? newTeacher = await _repository
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