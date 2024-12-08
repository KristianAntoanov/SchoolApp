using Microsoft.AspNetCore.Mvc;

using SchoolApp.Services.Data.Contrancts;

using static SchoolApp.Common.LoggerMessageConstants.Roles;
using static SchoolApp.Common.TempDataMessages;

namespace SchoolApp.Web.Areas.Admin.Controllers;

public class RolesController : AdminBaseController
{
    private readonly IAdminUserRolesService _userRolesService;
    private readonly ILogger<RolesController> _logger;
    private const int PageSize = 5;

    public RolesController(IAdminUserRolesService userRolesService, ILogger<RolesController> logger)
    {
        _userRolesService = userRolesService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int pageNumber = 1)
    {
        try
        {
            var pagedUsers = await _userRolesService.GetPagedUsersWithRolesAsync(pageNumber, PageSize);

            return View(pagedUsers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, LoadAllError);
            return BadRequest();
        }
    }

    [HttpPost]
    public async Task<IActionResult> UpdateTeacher(Guid userId, Guid? teacherId)
    {
        try
        {
            var (success, message) = await _userRolesService.UpdateUserTeacherAsync(userId, teacherId);

            if (success)
            {
                TempData[TempDataSuccess] = message;
            }
            else
            {
                TempData[TempDataError] = message;
            }

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, UpdateTeacherError, userId, teacherId);
            return BadRequest();
        }
        
    }

    [HttpPost]
    public async Task<IActionResult> UpdateRoles(Guid userId, List<string> roles)
    {
        try
        {
            var (success, message) = await _userRolesService.UpdateUserRolesAsync(userId, roles);

            if (success)
            {
                TempData[TempDataSuccess] = message;
            }
            else
            {
                TempData[TempDataError] = message;
            }

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, UpdateRolesError, userId);
            return BadRequest();
        }
    }
}