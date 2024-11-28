using Microsoft.AspNetCore.Mvc;
using SchoolApp.Services.Data.Contrancts;
using SchoolApp.Web.ViewModels.Team;

namespace SchoolApp.Web.Controllers
{
    public class TeamController : BaseController
    {
        private readonly ITeamService _service;

        public TeamController(ITeamService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<TeachersViewModel> model =
                await _service.GetAllTeachers();

            return View(model);
        }
    }
}

