using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SchoolApp.Services.Data.Contrancts;
using SchoolApp.Web.ViewModels.Team;

using static SchoolApp.Common.LoggerMessageConstants.Team;

namespace SchoolApp.Web.Controllers;

[AllowAnonymous]
public class TeamController : BaseController
{
    private readonly ITeamService _service;
    private readonly ILogger<TeamController> _logger;

    public TeamController(ITeamService service, ILogger<TeamController> logger)
    {
        _service = service;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            IEnumerable<TeachersViewModel> model =
                await _service.GetAllTeachers();

            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, LoadAllError);
            return StatusCode(StatusCodes.Status400BadRequest);
        }
    }
}