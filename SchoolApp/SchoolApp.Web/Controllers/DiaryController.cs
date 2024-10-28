using Microsoft.AspNetCore.Mvc;

namespace SchoolApp.Web.Controllers
{
	public class DiaryController : BaseController
	{
        public IActionResult Index()
        {
            return View();
        }
    }
}