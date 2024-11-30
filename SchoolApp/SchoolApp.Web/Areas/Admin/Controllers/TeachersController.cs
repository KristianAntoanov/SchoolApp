using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SchoolApp.Services.Data.Contrancts;
using SchoolApp.Web.ViewModels.Admin.Teachers;

namespace SchoolApp.Web.Areas.Admin.Controllers
{
    public class TeachersController : AdminBaseController
    {
        private readonly IAdminTeachersService _service;

        public TeachersController(IAdminTeachersService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            //TODO Remember to implement soft deletion and get only deleted teachers, if i hava enough time!
            IEnumerable<TeacherViewModel> model = await _service.GetAllTeachersAsync();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var model = new AddTeacherFormModel
            {
                AvailableSubjects = await _service.GetAvailableSubjectsAsync()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddTeacherFormModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AvailableSubjects = await _service.GetAvailableSubjectsAsync();
                return View(model);
            }

            var (isSuccessful, errorMessage) = await _service.AddTeacherAsync(model);

            if (!isSuccessful)
            {
                TempData["ErrorMessage"] = errorMessage;

                model.AvailableSubjects = await _service.GetAvailableSubjectsAsync();
                return View(model);
            }

            TempData["SuccessMessage"] = "Учителят беше успешно добавен.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var model = await _service.GetTeacherForEditAsync(id);

            if (model == null)
            {
                TempData["ErrorMessage"] = "Учителят не беше намерен.";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditTeacherFormModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AvailableSubjects = await _service.GetAvailableSubjectsAsync();
                return View(model);
            }

            var (isSuccessful, errorMessage) = await _service.EditTeacherAsync(model);

            if (isSuccessful)
            {
                TempData["SuccessMessage"] = "Учителят беше успешно редактиран.";
                return RedirectToAction(nameof(Index));
            }

            if (!string.IsNullOrEmpty(errorMessage))
            {
                ModelState.AddModelError(string.Empty, errorMessage);
            }

            model.AvailableSubjects = await _service.GetAvailableSubjectsAsync();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            bool result = await _service.DeleteTeacherAsync(id);

            if (result)
            {
                TempData["SuccessMessage"] = "Учителя беше успешно изтрит.";
            }
            else
            {
                TempData["ErrorMessage"] = "Възникна проблем при изтриването на учителя.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}