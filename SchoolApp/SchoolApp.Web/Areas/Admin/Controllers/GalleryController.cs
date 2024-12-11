using Microsoft.AspNetCore.Mvc;

using SchoolApp.Services.Data.Contrancts;
using SchoolApp.Web.ViewModels.Admin.Gallery;
using SchoolApp.Web.ViewModels.Admin.Gallery.Components;

using static SchoolApp.Common.LoggerMessageConstants.Gallery;
using static SchoolApp.Common.TempDataMessages;
using static SchoolApp.Common.TempDataMessages.Gallery;

namespace SchoolApp.Web.Areas.Admin.Controllers;

public class GalleryController : AdminBaseController
{
    private readonly IAdminGalleryService _service;
    private readonly ILogger<GalleryController> _logger;

    public GalleryController(IAdminGalleryService service, ILogger<GalleryController> logger)
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
            _logger.LogError(ex, LoadAllError);
            return StatusCode(StatusCodes.Status400BadRequest);
        }
        
    }

    [HttpGet]
    public IActionResult AddAlbum()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddAlbum(AddAlbumViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var (success, message) = await _service.AddAlbumAsync(model);

            if (success)
            {
                TempData[TempDataSuccess] = message;

                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData[TempDataError] = message;
            }

            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, CreateError, model.Title);
            return StatusCode(StatusCodes.Status400BadRequest);
        }
        
    }

    [HttpGet]
    public async Task<IActionResult> Album(string id)
    {
        try
        {
            if (string.IsNullOrEmpty(id))
            {
                TempData[TempDataError] = AlbumNotFound;

                return RedirectToAction(nameof(Index));
            }

            MenageAlbumsViewModel? model = await _service.GetDetailsForAlbumAsync(id);

            if (model == null)
            {
                TempData[TempDataError] = AlbumNotFound;

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }
        catch (NullReferenceException e)
        {
            _logger.LogError(e, LoadAlbumError, id);
            return StatusCode(StatusCodes.Status404NotFound);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, LoadAlbumError, id);
            return StatusCode(StatusCodes.Status400BadRequest);
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddImage(AddAlbumImageFormModel model)
    {
        try
        {
            if (model == null)
            {
                TempData[TempDataError] = InvalidData;
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
            {
                TempData[TempDataError] = InvalidImage;
                return RedirectToAction(nameof(Album), new { id = model.AlbumId });
            }

            var (success, message) = await _service.AddImagesAsync(model);

            if (success)
            {
                TempData[TempDataSuccess] = message;
            }
            else
            {
                TempData[TempDataError] = message;
            }

            return RedirectToAction(nameof(Album), new { id = model.AlbumId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, AddImageError, model?.AlbumId);
            return StatusCode(StatusCodes.Status400BadRequest);
        }
        
    }

    [HttpPost]
    public async Task<IActionResult> DeleteImage(string id, string albumId)
    {
        try
        {
            if (string.IsNullOrEmpty(id))
            {
                TempData[TempDataError] = InvalidImageError;
                return RedirectToAction(nameof(Index));
            }

            var (success, message) = await _service.DeleteImageAsync(id);

            if (success)
            {
                TempData[TempDataSuccess] = message;
            }
            else
            {
                TempData[TempDataError] = message;
            }

            return RedirectToAction(nameof(Album), new { id = albumId });
        }
        catch (NullReferenceException e)
        {
            _logger.LogError(e, DeleteImageError, id, albumId);
            return StatusCode(StatusCodes.Status404NotFound);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, DeleteImageError, id, albumId);
            return StatusCode(StatusCodes.Status400BadRequest);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            if (string.IsNullOrEmpty(id))
            {
                TempData[TempDataError] = InvalidAlbum;
                return RedirectToAction(nameof(Index));
            }

            var (success, message) = await _service.DeleteAlbumAsync(id);

            if (success)
            {
                TempData[TempDataSuccess] = message;
            }
            else
            {
                TempData[TempDataError] = message;
            }

            return RedirectToAction(nameof(Index));
        }
        catch (NullReferenceException e)
        {
            _logger.LogError(e, DeleteAlbumError, id);
            return StatusCode(StatusCodes.Status404NotFound);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, DeleteAlbumError, id);
            return StatusCode(StatusCodes.Status404NotFound);
        }
    }
}