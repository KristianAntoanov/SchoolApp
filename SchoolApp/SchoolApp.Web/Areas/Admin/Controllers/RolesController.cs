using Microsoft.AspNetCore.Mvc;
using SchoolApp.Services.Data.Contrancts;

namespace SchoolApp.Web.Areas.Admin.Controllers
{
    public class RolesController : AdminBaseController
    {
        private readonly IAdminUserRolesService _userRolesService;

        public RolesController(IAdminUserRolesService userRolesService)
        {
            _userRolesService = userRolesService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _userRolesService.GetAllUsersWithRolesAsync();

            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTeacher(Guid userId, Guid? teacherId)
        {
            var (success, message) = await _userRolesService.UpdateUserTeacherAsync(userId, teacherId);

            if (success)
            {
                TempData["SuccessMessage"] = message;
            }
            else
            {
                TempData["ErrorMessage"] = message;
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRoles(Guid userId, List<string> roles)
        {
            var (success, message) = await _userRolesService.UpdateUserRolesAsync(userId, roles);

            if (success)
            {
                TempData["SuccessMessage"] = message;
            }
            else
            {
                TempData["ErrorMessage"] = message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}