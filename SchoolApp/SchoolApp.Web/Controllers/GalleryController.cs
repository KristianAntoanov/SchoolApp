using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SchoolApp.Services.Data.Contrancts;

using static SchoolApp.Common.TempDataMessages.Gallery;

namespace SchoolApp.Web.Controllers;

[AllowAnonymous]
public class GalleryController : BaseController
{
    private readonly IGalleryService _service;
    private readonly ILogger<GalleryController> _logger;

    public GalleryController(IGalleryService service, ILogger<GalleryController> logger)
    {
        _service = service;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var albums = await _service.GetAllAlbumsWithImagesAsync();

            return View(albums);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, InvalidLoadGallery);
            return StatusCode(StatusCodes.Status400BadRequest);
        }
    }
}