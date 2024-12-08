using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using SchoolApp.Services.Data.Contrancts;
using SchoolApp.Data.Models;
using SchoolApp.Web.ViewModels;
using SchoolApp.Web.ViewModels.Diary.AddForms;
using SchoolApp.Web.ViewModels.Diary.Remarks;

namespace SchoolApp.Web.Controllers;

public class DiaryController : BaseController
{
    private readonly IDiaryService _service;
    private readonly UserManager<ApplicationUser> _userManager;

    public DiaryController(IDiaryService service, UserManager<ApplicationUser> userManager)
    {
        _service = service;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        IEnumerable<DiaryIndexViewModel> model = await _service
            .IndexGetAllClassesAsync();

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> LoadClassAndContent(int classId)
    {
        IEnumerable<SubjectViewModel> model = await _service
            .GetClassContentAsync(classId);

        return PartialView("Content", model);
    }

    [HttpGet]
    public async Task<IActionResult> LoadGradeContent(int classId, int subjectId)
    {
        IEnumerable<StudentGradesViewModel> model = await _service
            .GetGradeContentAsync(classId, subjectId);

        return PartialView("Grades", model);
    }

    [HttpGet]
    public async Task<IActionResult> LoadRemarksContent(int classId)
    {
        IEnumerable<StudentRemarksViewModel> model = await _service
            .GetRemarksContentAsync(classId);

        return PartialView("Remarks", model);
    }

    [HttpGet]
    public async Task<IActionResult> LoadAbsencesContent(int classId)
    {
        IEnumerable<StudentAbsencesViewModel> model = await _service
            .GetAbsencesContentAsync(classId);

        return PartialView("Absences", model);
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> AddGrades(int classId, int subjectId)
    {
        GradeFormModel model = await _service
            .GetClassStudentForAddAsync<GradeFormModel>(classId, subjectId);

        model.GradeTypes = _service.GetGradeTypes();

        return View(model);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> AddGrades(GradeFormModel model)
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
            TempData["ErrorMessage"] = "Невалидни данни.";
            return BadRequest();
        }

        TempData["SuccessMessage"] = "Успешно добавихте оценки.";
        return RedirectToAction(nameof(Index), new
        {
            selectedSubjectId = model.SubjectId
        });
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> AddAbsence(int classId, int subjectId)
    {
        AbsenceFormModel model = await _service
            .GetClassStudentForAddAsync<AbsenceFormModel>(classId, subjectId);

        return View(model);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> AddAbsence(AbsenceFormModel model)
    {
        if (!ModelState.IsValid)
        {
            model.Subjects = await _service.GetSubjectsAsync();
            return View(model);
        }

        bool result = await _service.AddAbsenceAsync(model);

        if (!result)
        {
            TempData["ErrorMessage"] = "Невалидни данни.";
            return BadRequest();
        }

        TempData["SuccessMessage"] = "Успешно добавихте отсъствия.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> AddRemark(int classId, int subjectId)
    {
        RemarkFormModel model = await _service
            .GetClassStudentForAddAsync<RemarkFormModel>(classId, subjectId);

        return View(model);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> AddRemark(RemarkFormModel model)
    {
        if (!ModelState.IsValid)
        {
            model.Subjects = await _service.GetSubjectsAsync();
            model.Students = await _service.GetStudentsAsync(model);
            return View(model);
        }

        string userId = _userManager.GetUserId(User)!;
        if (String.IsNullOrWhiteSpace(userId))
        {
            return Redirect("/Identity/Account/Login");
        }

        bool result = await _service.AddRemarkAsync(userId, model);

        if (!result)
        {
            TempData["ErrorMessage"] = "Невалидни данни.";
            return BadRequest();
        }

        TempData["SuccessMessage"] = "Успешно добавихте забележка.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> ExcuseAbsence(int id)
    {
        bool result = await _service.ExcuseAbsenceAsync(id);

        if (!result)
        {
            TempData["ErrorMessage"] = "Невалидни данни.";
            return BadRequest();
        }

        TempData["SuccessMessage"] = "Успешно извинихте отсъствието.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> DeleteAbsence(int id)
    {
        bool result = await _service.DeleteAbsenceAsync(id);

        if (!result)
        {
            TempData["ErrorMessage"] = "Невалидни данни.";
            return BadRequest();
        }

        TempData["SuccessMessage"] = "Успешно изтрихте отсъствието.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> DeleteRemark(int id)
    {
        bool result = await _service.DeleteRemarkAsync(id);

        if (!result)
        {
            TempData["ErrorMessage"] = "Невалидни данни.";
            return BadRequest();
        }

        TempData["SuccessMessage"] = "Успешно извинихте отсъствието.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> EditRemark(int id)
    {
        EditRemarkViewModel? model = await _service.GetRemarkByIdAsync(id);

        if (model == null)
        {
            TempData["ErrorMessage"] = "Забележката не е намерена.";
            return NotFound();
        }

        return View(model);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> EditRemark(EditRemarkViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        bool result = await _service.EditRemarkAsync(model);

        if (!result)
        {
            TempData["ErrorMessage"] = "Невалидни данни.";
            return BadRequest();
        }

        TempData["SuccessMessage"] = "Успешно редактирахте забележката.";
        return RedirectToAction(nameof(Index));
    }
}