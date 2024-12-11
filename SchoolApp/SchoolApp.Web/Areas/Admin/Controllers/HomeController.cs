using Microsoft.AspNetCore.Mvc;

namespace SchoolApp.Web.Areas.Admin.Controllers;

public class HomeController : AdminBaseController
{
    public HomeController()
    {

    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
}