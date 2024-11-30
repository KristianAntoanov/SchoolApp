using Microsoft.AspNetCore.Mvc;
using SchoolApp.Services.Data.Contrancts;

namespace SchoolApp.Web.Controllers
{
    public class GalleryController : BaseController
    {
        private readonly IGalleryService _service;

        public GalleryController(IGalleryService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var albums = await _service.GetAllAlbumsWithImagesAsync();

            return View(albums);
        }
    }
}

