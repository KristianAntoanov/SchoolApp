using Microsoft.AspNetCore.Mvc;
using SchoolApp.Services.Data;
using SchoolApp.Web.ViewModels;

namespace SchoolApp.Web.Controllers
{
	public class DiaryController : BaseController
	{
        private readonly DiaryService _service;

        public DiaryController(DiaryService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<DiaryIndexViewModel> model = await _service
                .IndexGetAllClasses();

            return View(model);
        }
    }
}