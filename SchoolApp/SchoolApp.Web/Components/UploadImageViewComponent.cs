using Microsoft.AspNetCore.Mvc;
using SchoolApp.Web.ViewModels.Admin.Gallery.Components;

namespace SchoolApp.Web.Components
{
	public class UploadImageViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(string albumId)
        {
            AddAlbumImageFormModel model = new AddAlbumImageFormModel()
            {
                AlbumId = albumId
            };
            return View(model);
        }
    }
}