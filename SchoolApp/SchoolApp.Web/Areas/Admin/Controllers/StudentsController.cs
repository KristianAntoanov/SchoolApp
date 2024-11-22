using Microsoft.AspNetCore.Mvc;


namespace SchoolApp.Web.Areas.Admin.Controllers
{
    public class StudentsController : AdminBaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}