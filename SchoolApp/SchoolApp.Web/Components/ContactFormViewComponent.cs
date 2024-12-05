using Microsoft.AspNetCore.Mvc;
using SchoolApp.Web.ViewModels.Home;

namespace SchoolApp.Web.Components
{
	public class ContactFormViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var model = new ContactFormModel();
            return View(model);
        }
    }
}