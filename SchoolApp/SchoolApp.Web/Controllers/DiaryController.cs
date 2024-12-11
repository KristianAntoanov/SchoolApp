using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using SchoolApp.Services.Data.Contrancts;
using SchoolApp.Data.Models;
using SchoolApp.Web.ViewModels;
using SchoolApp.Web.ViewModels.Diary.AddForms;
using SchoolApp.Web.ViewModels.Diary.Remarks;

using static SchoolApp.Common.LoggerMessageConstants.Diary;
using static SchoolApp.Common.TempDataMessages.Diary;
using static SchoolApp.Common.TempDataMessages;

namespace SchoolApp.Web.Controllers;

public class DiaryController : BaseController
{
    private readonly IDiaryService _service;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<DiaryController> _logger;

    public DiaryController(IDiaryService service, UserManager<ApplicationUser> userManager,
                            ILogger<DiaryController> logger)
    {
        _service = service;
        _userManager = userManager;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            IEnumerable<DiaryIndexViewModel> model = await _service
            .IndexGetAllClassesAsync();

            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, LoadClassesError);
            return StatusCode(StatusCodes.Status400BadRequest);
        }
    }

    [HttpGet]
    public async Task<IActionResult> LoadClassAndContent(int classId)
    {

        try
        {
            IEnumerable<SubjectViewModel> model = await _service
            .GetClassContentAsync(classId);

            return PartialView("Content", model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, LoadContentError);
            return StatusCode(StatusCodes.Status400BadRequest);
        }
    }

    [HttpGet]
    public async Task<IActionResult> LoadGradeContent(int classId, int subjectId)
    {
        try
        {
            IEnumerable<StudentGradesViewModel> model = await _service
            .GetGradeContentAsync(classId, subjectId);

            return PartialView("Grades", model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, LoadContentError);
            return StatusCode(StatusCodes.Status400BadRequest);
        }
    }

    [HttpGet]
    public async Task<IActionResult> LoadRemarksContent(int classId)
    {
        try
        {
            IEnumerable<StudentRemarksViewModel> model = await _service
            .GetRemarksContentAsync(classId);

            return PartialView("Remarks", model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, LoadContentError);
            return StatusCode(StatusCodes.Status400BadRequest);
        }
    }

    [HttpGet]
    public async Task<IActionResult> LoadAbsencesContent(int classId)
    {
        try
        {
            IEnumerable<StudentAbsencesViewModel> model = await _service
            .GetAbsencesContentAsync(classId);

            return PartialView("Absences", model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, LoadContentError);
            return StatusCode(StatusCodes.Status400BadRequest);
        }
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> AddGrades(int classId, int subjectId)
    {
        try
        {
            GradeFormModel model = await _service
            .GetClassStudentForAddAsync<GradeFormModel>(classId, subjectId);

            model.GradeTypes = _service.GetGradeTypes();

            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, AddGradesError);
            return StatusCode(StatusCodes.Status400BadRequest);
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> AddGrades(GradeFormModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                model.Subjects = await _service.GetSubjectsAsync();
                model.GradeTypes = _service.GetGradeTypes();
                return View(model);
            }

            string userId = _userManager.GetUserId(User)!;
            if (String.IsNullOrWhiteSpace(userId))
            {
                return Redirect("/Identity/Account/Login");
            }

            bool result = await _service.AddGradesAsync(userId, model);

            if (!result)
            {
                TempData[TempDataError] = InvalidData;
                throw new ArgumentException(InvalidData);
            }

            TempData[TempDataSuccess] = GradesAddSuccess;
            return RedirectToAction(nameof(Index), new
            {
                selectedSubjectId = model.SubjectId
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, AddGradesError);
            return StatusCode(StatusCodes.Status400BadRequest);
        }
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> AddAbsence(int classId, int subjectId)
    {
        try
        {
            AbsenceFormModel model = await _service
            .GetClassStudentForAddAsync<AbsenceFormModel>(classId, subjectId);

            return View(model);
        }
        catch (NullReferenceException e)
        {
            _logger.LogError(e, AddAbsenceError);
            return StatusCode(StatusCodes.Status404NotFound);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, AddAbsenceError);
            return StatusCode(StatusCodes.Status400BadRequest);
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> AddAbsence(AbsenceFormModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                model.Subjects = await _service.GetSubjectsAsync();
                return View(model);
            }

            bool result = await _service.AddAbsenceAsync(model);

            if (!result)
            {
                TempData[TempDataError] = InvalidData;
                throw new InvalidOperationException();
            }

            TempData[TempDataSuccess] = AbsencesAddSuccess;
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, AddAbsenceError);
            return StatusCode(StatusCodes.Status404NotFound);
        }
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> AddRemark(int classId, int subjectId)
    {
        try
        {
            RemarkFormModel model = await _service
            .GetClassStudentForAddAsync<RemarkFormModel>(classId, subjectId);

            return View(model);
        }
        catch (NullReferenceException ex)
        {
            _logger.LogError(ex, AddRemarkError);
            return StatusCode(StatusCodes.Status404NotFound);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, AddRemarkError);
            return StatusCode(StatusCodes.Status400BadRequest);
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> AddRemark(RemarkFormModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                model.Subjects = await _service.GetSubjectsAsync();
                model.Students = await _service.GetStudentsAsync(model);
                return View(model);
            }

            string userId = _userManager.GetUserId(User)!;
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Redirect("/Identity/Account/Login");
            }

            bool result = await _service.AddRemarkAsync(userId, model);

            if (!result)
            {
                TempData[TempDataError] = InvalidData;
                throw new InvalidOperationException();
            }

            TempData[TempDataSuccess] = RemarkAddSuccess;
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, AddRemarkError);
            return StatusCode(StatusCodes.Status400BadRequest);
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> ExcuseAbsence(int id)
    {
        try
        {
            bool result = await _service.ExcuseAbsenceAsync(id);

            if (!result)
            {
                TempData[TempDataError] = InvalidData;
                throw new NullReferenceException();
            }

            TempData[TempDataSuccess] = AbsenceExcuseSuccess;
            return RedirectToAction(nameof(Index));
        }
        catch (NullReferenceException e)
        {
            _logger.LogError(e, ExcuseAbsenceError, id);
            return StatusCode(StatusCodes.Status404NotFound);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ExcuseAbsenceError, id);
            return StatusCode(StatusCodes.Status400BadRequest);
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> DeleteAbsence(int id)
    {
        try
        {
            bool result = await _service.DeleteAbsenceAsync(id);

            if (!result)
            {
                TempData[TempDataError] = InvalidData;
                throw new NullReferenceException();
            }

            TempData[TempDataSuccess] = AbsenceDeleteSuccess;
            return RedirectToAction(nameof(Index));
        }
        catch (NullReferenceException e)
        {
            _logger.LogError(e, DeleteAbsenceError, id);
            return StatusCode(StatusCodes.Status404NotFound);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, DeleteAbsenceError, id);
            return StatusCode(StatusCodes.Status400BadRequest);
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> DeleteRemark(int id)
    {
        try
        {
            bool result = await _service.DeleteRemarkAsync(id);

            if (!result)
            {
                TempData[TempDataError] = InvalidData;
                throw new NullReferenceException();
            }

            TempData[TempDataSuccess] = RemarkDeleteSuccess;
            return RedirectToAction(nameof(Index));
        }
        catch (NullReferenceException e)
        {
            _logger.LogError(e, DeleteRemarkError, id);
            return StatusCode(StatusCodes.Status404NotFound);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, DeleteRemarkError, id);
            return StatusCode(StatusCodes.Status400BadRequest);
        }
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> EditRemark(int id)
    {
        try
        {
            EditRemarkViewModel? model = await _service.GetRemarkByIdAsync(id);

            if (model == null)
            {
                TempData[TempDataError] = RemarkNotFound;
                throw new NullReferenceException();
            }

            return View(model);
        }
        catch (NullReferenceException e)
        {
            _logger.LogError(e, EditRemarkError, id);
            return StatusCode(StatusCodes.Status404NotFound);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, EditRemarkError, id);
            return StatusCode(StatusCodes.Status400BadRequest);
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> EditRemark(EditRemarkViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool result = await _service.EditRemarkAsync(model);

            if (!result)
            {
                TempData[TempDataError] = InvalidData;
                throw new InvalidOperationException();
            }

            TempData[TempDataSuccess] = RemarkEditSuccess;
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, EditRemarkError, model.Id);
            return StatusCode(StatusCodes.Status400BadRequest);
        }
    }
}