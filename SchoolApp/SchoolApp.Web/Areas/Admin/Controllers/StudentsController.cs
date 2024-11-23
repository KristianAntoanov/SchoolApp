using Microsoft.AspNetCore.Mvc;
using SchoolApp.Services.Data.Contrancts;
using SchoolApp.Web.ViewModels.Admin;

namespace SchoolApp.Web.Areas.Admin.Controllers
{
    public class StudentsController : AdminBaseController
    {
        private readonly IAdminService _service;

        public StudentsController(IAdminService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index(int page = 1, string search = null)
        {
            const int PageSize = 10;

            var result = await _service.GetAllStudentsAsync(page, PageSize, search);

            ViewBag.CurrentPage = result.PageNumber;
            ViewBag.TotalPages = result.TotalPages;
            ViewBag.Search = search;

            return View(result.Items);
        }

        public async Task<IActionResult> Delete(int id)
        {
            bool result = await _service.DeleteStudent(id);

            if (result)
            {
                TempData["SuccessMessage"] = "Студентът беше успешно изтрит.";
            }
            else
            {
                TempData["ErrorMessage"] = "Възникна проблем при изтриването на студента.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var student = await _service.GetStudentForEditAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditStudentFormModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AvailableClasses = await _service.GetAvailableClassesAsync();
                return View(model);
            }

            bool result = await _service.UpdateStudentAsync(model);

            if (result)
            {
                TempData["SuccessMessage"] = "Студентът беше успешно редактиран.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = "Възникна грешка при редактирането на студента.";
                model.AvailableClasses = await _service.GetAvailableClassesAsync();
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ManageGrades(int id)
        {
            var model = await _service.GetStudentGradesAsync(id);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteGrade(int gradeId, int studentId)
        {
            var result = await _service.DeleteGradeAsync(gradeId);

            if (result)
            {
                TempData["SuccessMessage"] = "Оценката беше успешно изтрита.";
                return RedirectToAction(nameof(ManageGrades), new { id = studentId });
            }

            TempData["ErrorMessage"] = "Възникна грешка при изтриване на оценката.";
            return RedirectToAction(nameof(ManageGrades));
        }
    }
}