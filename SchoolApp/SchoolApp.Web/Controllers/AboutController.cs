using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SchoolApp.Web.Controllers;

[AllowAnonymous]
public class AboutController : BaseController
{
    public IActionResult History()
    {
        return View();
    }

    public IActionResult Mission()
    {
        return View();
    }

    public IActionResult Specialties()
    {
        return View();
    }

    public IActionResult Schedule()
    {
        return View();
    }
}