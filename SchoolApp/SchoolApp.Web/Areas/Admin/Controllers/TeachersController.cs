using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SchoolApp.Data.Models;
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

        public async Task<IActionResult> Index()
        {
            //TODO Remember to implement soft deletion for teachers, if i hava enough time!
            IEnumerable<TeacherViewModel> model = await _service.GetAllTeachersAsync();

            return View(model);
        }
    }
}