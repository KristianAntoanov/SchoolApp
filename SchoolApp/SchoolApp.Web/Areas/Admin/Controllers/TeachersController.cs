using Microsoft.AspNetCore.Mvc;

using SchoolApp.Services.Data.Contrancts;
using SchoolApp.Web.ViewModels.Admin.Teachers;

using static SchoolApp.Common.LoggerMessageConstants.Teachers;
using static SchoolApp.Common.TempDataMessages.Teachers;
using static SchoolApp.Common.TempDataMessages;

namespace SchoolApp.Web.Areas.Admin.Controllers;

public class TeachersController : AdminBaseController
{
    private readonly IAdminTeachersService _service;
    private readonly ILogger<TeachersController> _logger;

    public TeachersController(IAdminTeachersService service, ILogger<TeachersController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            IEnumerable<TeacherViewModel> model = await _service.GetAllTeachersAsync();

            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, LoadAllError);
            return StatusCode(StatusCodes.Status400BadRequest);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Add()
    {
        try
        {
            var model = new AddTeacherFormModel
            {
                AvailableSubjects = await _service.GetAvailableSubjectsAsync()
            };

            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, AddFormError);
            return StatusCode(StatusCodes.Status400BadRequest);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddTeacherFormModel model)
    {
        try
        {
            if (model == null)
            {
                TempData[TempDataError] = InvalidDataMessage;
                return RedirectToAction(nameof(Add));
            }

            if (!ModelState.IsValid)
            {
                model.AvailableSubjects = await _service.GetAvailableSubjectsAsync();
                return View(model);
            }

            var (isSuccessful, errorMessage) = await _service.AddTeacherAsync(model);

            if (!isSuccessful)
            {
                TempData[TempDataError] = errorMessage;

                model.AvailableSubjects = await _service.GetAvailableSubjectsAsync();
                return View(model);
            }

            TempData[TempDataSuccess] = AddSuccessMessage;
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, AddError);
            return StatusCode(StatusCodes.Status400BadRequest);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {
        try
        {
            var model = await _service.GetTeacherForEditAsync(id);

            if (model == null)
            {
                TempData[TempDataError] = NotFoundMessage;
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }
        catch (NullReferenceException e)
        {
            _logger.LogError(e, EditLoadError, id);
            return StatusCode(StatusCodes.Status404NotFound);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, EditLoadError, id);
            return StatusCode(StatusCodes.Status400BadRequest);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Edit(EditTeacherFormModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                model.AvailableSubjects = await _service.GetAvailableSubjectsAsync();
                return View(model);
            }

            var (isSuccessful, errorMessage) = await _service.EditTeacherAsync(model);

            if (isSuccessful)
            {
                TempData[TempDataSuccess] = EditSuccessMessage;
                return RedirectToAction(nameof(Index));
            }
            else
            {
                model.AvailableSubjects = await _service.GetAvailableSubjectsAsync();
                foreach (var subject in model.AvailableSubjects)
                {
                    subject.IsSelected = model.SelectedSubjects.Contains(subject.Id);
                }
                TempData[TempDataError] = errorMessage;
                return View(model);
            }
        }
        catch (IOException ex)
        {
            _logger.LogError(ex, ImageError);
            return StatusCode(StatusCodes.Status400BadRequest);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, EditError, model.Id);
            return StatusCode(StatusCodes.Status400BadRequest);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            bool result = await _service.DeleteTeacherAsync(id);

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
        catch (NullReferenceException e)
        {
            _logger.LogError(e, DeleteError, id);
            return StatusCode(StatusCodes.Status404NotFound);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, DeleteError, id);
            return StatusCode(StatusCodes.Status400BadRequest);
        }
    }
}