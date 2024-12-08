using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SchoolApp.Services.Data.Contrancts;
using SchoolApp.Web.ViewModels.News;

using static SchoolApp.Common.LoggerMessageConstants.News;
using static SchoolApp.Common.TempDataMessages.News;
using static SchoolApp.Common.TempDataMessages;

namespace SchoolApp.Web.Controllers;

public class NewsController : BaseController
{
    private readonly INewsService _newsService;
    private readonly ILogger<NewsController> _logger;

    public NewsController(INewsService newsService, ILogger<NewsController> logger)
    {
        _newsService = newsService;
        _logger = logger;
    }

    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        try
        {
            IEnumerable<NewsViewModel> news = await _newsService.GetAllNewsAsync();

            return View(news);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, LoadAllError);
            return BadRequest();
        }
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Teacher")]
    public IActionResult Add()
    {
        return View();
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> Add(AddNewsViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var (success, message) = await _newsService.AddNewsAsync(model);

            if (success)
            {
                TempData[TempDataSuccess] = message;
                return RedirectToAction(nameof(Index));
            }

            TempData[TempDataError] = message;
            return View(model);
        }
        catch (IOException ex)
        {
            _logger.LogError(ex, ImageProcessError);
            return BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, CreateError);
            return BadRequest();
        }
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Teacher,Parent")]
    public async Task<IActionResult> Details(int id)
    {
        try
        {
            var news = await _newsService.GetNewsDetailsAsync(id);

            if (news == null)
            {
                TempData[TempDataError] = NotFoundMessage;
                return RedirectToAction(nameof(Index));
            }

            return View(news);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, LoadDetailsError, id);
            return BadRequest();
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            if (id <= 0)
            {
                TempData[TempDataError] = InvalidIdMessage;
                return RedirectToAction(nameof(Index));
            }

            var (success, message) = await _newsService.DeleteNewsAsync(id);

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
        catch (Exception ex)
        {
            _logger.LogError(ex, DeleteError, id);
            return BadRequest();
        }
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Teacher,Parent")]
    public async Task<IActionResult> ImportantMessages()
    {
        try
        {
            IEnumerable<AnnouncementViewModel> announcements =
            await _newsService.GetAllImportantMessagesAsync();

            return View(announcements);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, LoadMessagesError);
            return BadRequest();
        }
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Teacher")]
    public IActionResult AddAnnouncement()
    {
        return View();
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> AddAnnouncement(AddAnnouncementViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var (success, message) = await _newsService.AddAnnouncementAsync(model);

            if (success)
            {
                TempData[TempDataSuccess] = message;
                return RedirectToAction(nameof(ImportantMessages));
            }

            TempData[TempDataError] = message;
            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, AddMessageError);
            return BadRequest();
        }
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> EditAnnouncement(int id)
    {
        try
        {
            var announcement = await _newsService.GetAnnouncementForEditAsync(id);

            if (announcement == null)
            {
                TempData[TempDataError] = AnnouncementNotFoundMessage;
                return RedirectToAction(nameof(ImportantMessages));
            }

            return View(announcement);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, EditMessageError, id);
            return BadRequest();
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> EditAnnouncement(int id, AddAnnouncementViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var (success, message) = await _newsService.EditAnnouncementAsync(id, model);

            if (success)
            {
                TempData[TempDataSuccess] = message;
                return RedirectToAction(nameof(ImportantMessages));
            }

            TempData[TempDataError] = message;
            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, EditMessageError, id);
            return BadRequest();
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> DeleteAnnouncement(int id)
    {
        try
        {
            var (success, message) = await _newsService.DeleteAnnouncementAsync(id);

            if (success)
            {
                TempData[TempDataSuccess] = message;
            }
            else
            {
                TempData[TempDataError] = message;
            }

            return RedirectToAction(nameof(ImportantMessages));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, DeleteMessageError, id);
            return BadRequest();
        }
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Achievements()
    {
        try
        {
            IEnumerable<NewsViewModel> achievements =
            await _newsService.GetAllAchievementsAsync();

            return View(achievements);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, LoadAchievementsError);
            return BadRequest();
        }
    }
}