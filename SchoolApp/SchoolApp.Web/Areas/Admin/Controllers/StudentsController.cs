using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using SchoolApp.Data.Models;
using SchoolApp.Services.Data.Contrancts;
using SchoolApp.Web.ViewModels.Admin.Students;

using static SchoolApp.Common.LoggerMessageConstants.Students;
using static SchoolApp.Common.TempDataMessages.Students;
using static SchoolApp.Common.TempDataMessages;

namespace SchoolApp.Web.Areas.Admin.Controllers;

public class StudentsController : AdminBaseController
{
    private readonly IAdminStudentsService _service;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<RolesController> _logger;
    private const int PageSize = 10;

    public StudentsController(IAdminStudentsService service, UserManager<ApplicationUser> userManager,
                                ILogger<RolesController> logger)
    {
        _service = service;
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<IActionResult> Index(int page = 1, string search = null)
    {
        try
        {
            var result = await _service.GetAllStudentsAsync(page, PageSize, search);
            result.SearchTerm = search;

            return View(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, LoadAllError);
            return BadRequest();
        }
    }

    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            bool result = await _service.DeleteStudent(id);

            if (result)
            {
                TempData[TempDataSuccess] = DeleteSuccessMessage;
            }
            else
            {
                TempData[TempDataError] = DeleteErrorMessage;
            }

            return RedirectToAction(nameof(Index));
        }
        catch (NullReferenceException ex)
        {
            _logger.LogError(ex, DeleteError, id);
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, DeleteError, id);
            return BadRequest();
        }
        
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var student = await _service.GetStudentForEditAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, EditLoadError, id);
            return BadRequest();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Edit(EditStudentFormModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                model.AvailableClasses = await _service.GetAvailableClassesAsync();
                return View(model);
            }

            bool result = await _service.UpdateStudentAsync(model);

            if (result)
            {
                TempData[TempDataSuccess] = EditSuccessMessage;
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData[TempDataError] = EditErrorMessage;
                model.AvailableClasses = await _service.GetAvailableClassesAsync();
                return View(model);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, EditError, model.Id);
            return BadRequest();
        }
    }

    [HttpGet]
    public async Task<IActionResult> ManageGrades(int id)
    {
        try
        {
            var model = await _service.GetStudentGradesAsync(id);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }
        catch (Exception ex)
        {
             _logger.LogError(ex, LoadGradesError, id);
            return BadRequest();
        }
        
    }

    [HttpPost]
    public async Task<IActionResult> DeleteGrade(int gradeId, int studentId)
    {
        try
        {
            var result = await _service.DeleteGradeAsync(gradeId);

            if (result)
            {
                TempData[TempDataSuccess] = GradeDeleteSuccessMessage;
                return RedirectToAction(nameof(ManageGrades), new { id = studentId });
            }

            TempData[TempDataError] = GradeDeleteErrorMessage;
            return RedirectToAction(nameof(ManageGrades));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, DeleteGradeError, gradeId, studentId);
            return BadRequest();
        }
    }

    [HttpGet]
    public async Task<IActionResult> Add()
    {
        try
        {
            var model = new AddStudentFormModel
            {
                AvailableClasses = await _service.GetAvailableClassesAsync()
            };

            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, AddError);
            return BadRequest();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddStudentFormModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                model.AvailableClasses = await _service.GetAvailableClassesAsync();
                return View(model);
            }

            string userId = _userManager.GetUserId(User)!;
            if (String.IsNullOrWhiteSpace(userId))
            {
                return Redirect("/Identity/Account/Login");
            }

            bool result = await _service.AddStudentAsync(model, userId);

            if (result)
            {
                TempData[TempDataSuccess] = AddSuccessMessage;
                return RedirectToAction(nameof(Index));
            }

            TempData[TempDataError] = AddErrorMessage;
            model.AvailableClasses = await _service.GetAvailableClassesAsync();
            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, AddError);
            return BadRequest();
        }
    }
}