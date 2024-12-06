using Microsoft.AspNetCore.Mvc;

using SchoolApp.Services.Data.Contrancts;
using SchoolApp.Web.ViewModels.Admin.Gallery;
using SchoolApp.Web.ViewModels.Admin.Gallery.Components;

namespace SchoolApp.Web.Areas.Admin.Controllers;

public class GalleryController : AdminBaseController
{
    private readonly IAdminGalleryService _service;

    public GalleryController(IAdminGalleryService service)
    {
        _service = service;
    }

    public async Task<IActionResult> Index()
    {
        var albums = await _service.GetAllAlbumsWithImagesAsync();

        return View(albums);
    }

    [HttpGet]
    public IActionResult AddAlbum()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddAlbum(AddAlbumViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var (success, message) = await _service.AddAlbumAsync(model);

        if (success)
        {
            TempData["SuccessMessage"] = message;

            return RedirectToAction(nameof(Index));
        }
        else
        {
            TempData["ErrorMessage"] = message;
        }

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Album(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            TempData["ErrorMessage"] = "Албумът не съществува.";

            return RedirectToAction(nameof(Index));
        }

        MenageAlbumsViewModel? model = await _service.GetDetailsForAlbumAsync(id);

        if (model == null)
        {
            TempData["ErrorMessage"] = "Албумът не съществува.";

            return RedirectToAction(nameof(Index));
        }

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> AddImage(AddAlbumImageFormModel model)
    {
        if (model == null)
        {
            TempData["ErrorMessage"] = "Невалидни данни.";
            return RedirectToAction(nameof(Index));
        }

        if (!ModelState.IsValid)
        {
            TempData["ErrorMessage"] = "Моля, изберете валидна снимка.";
            return RedirectToAction(nameof(Album), new { id = model.AlbumId });
        }

        var (success, message) = await _service.AddImagesAsync(model);

        if (success)
        {
            TempData["SuccessMessage"] = message;
        }
        else
        {
            TempData["ErrorMessage"] = message;
        }

        return RedirectToAction(nameof(Album), new { id = model.AlbumId });
    }

    [HttpPost]
    public async Task<IActionResult> DeleteImage(string id, string albumId)
    {
        if (string.IsNullOrEmpty(id))
        {
            TempData["ErrorMessage"] = "Невалидно ID на снимка.";
            return RedirectToAction(nameof(Index));
        }

        var (success, message) = await _service.DeleteImageAsync(id);

        if (success)
        {
            TempData["SuccessMessage"] = message;
        }
        else
        {
            TempData["ErrorMessage"] = message;
        }

        return RedirectToAction(nameof(Album), new { id = albumId });
    }

    [HttpPost]
    public async Task<IActionResult> Delete(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            TempData["ErrorMessage"] = "Невалидно ID на албум.";
            return RedirectToAction(nameof(Index));
        }

        var (success, message) = await _service.DeleteAlbumAsync(id);

        if (success)
        {
            TempData["SuccessMessage"] = message;
        }
        else
        {
            TempData["ErrorMessage"] = message;
        }

        return RedirectToAction(nameof(Index));
    }
}