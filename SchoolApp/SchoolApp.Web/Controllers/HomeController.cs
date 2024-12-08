using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SchoolApp.Services.Data.Contrancts;
using SchoolApp.Web.ViewModels.Home;

using static SchoolApp.Common.LoggerMessageConstants.Home;
using static SchoolApp.Common.TempDataMessages.Home;
using static SchoolApp.Common.TempDataMessages;

namespace SchoolApp.Web.Controllers;

[AllowAnonymous]
public class HomeController : BaseController
{
    private readonly ILogger<HomeController> _logger;
    private readonly IContactService _contactService;

    public HomeController(ILogger<HomeController> logger, IContactService contactService)
    {
        _logger = logger;
        _contactService = contactService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error(int? statusCode)
    {
        if (statusCode.HasValue)
        {
            if (statusCode == 400)
            {
                return View("Error400");
            }
            else if (statusCode == 401)
            {
                return RedirectToAction(nameof(AccessDenied));
            }
            else if (statusCode == 403)
            {
                return View("Error403");
            }
            else if (statusCode == 404)
            {
                return View("Error404");
            }
            else if (statusCode == 405)
            {
                return View("Error405");
            }
            else if (statusCode == 500)
            {
                return View("Error500");
            }
        }

        return View();
    }

    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View("Error401");
    }

    [HttpPost]
    public async Task<IActionResult> SubmitContact(ContactFormModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                TempData[TempDataError] = MissingRequiredFields;
                return View("Index", model);
            }

            var result = await _contactService.SubmitContactFormAsync(model);

            if (result)
            {
                TempData[TempDataSuccess] = SubmitSuccess;
            }
            else
            {
                TempData[TempDataError] = SubmitError;
            }

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ContactFormError);
            return BadRequest();
        }
    }
}