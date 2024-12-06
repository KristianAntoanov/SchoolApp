using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SchoolApp.Services.Data.Contrancts;
using SchoolApp.Web.ViewModels.News;

namespace SchoolApp.Web.Controllers;

public class NewsController : BaseController
{
    private readonly INewsService _newsService;

    public NewsController(INewsService newsService)
    {
        _newsService = newsService;
    }

    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        IEnumerable<NewsViewModel> news = await _newsService.GetAllNewsAsync();

        return View(news);
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
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var (success, message) = await _newsService.AddNewsAsync(model);

        if (success)
        {
            TempData["SuccessMessage"] = message;
            return RedirectToAction(nameof(Index));
        }

        TempData["ErrorMessage"] = message;
        return View(model);
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Teacher,Parent")]
    public async Task<IActionResult> Details(int id)
    {
        var news = await _newsService.GetNewsDetailsAsync(id);

        if (news == null)
        {
            TempData["ErrorMessage"] = "Новината не беше намерена.";
            return RedirectToAction(nameof(Index));
        }

        return View(news);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> Delete(int id)
    {
        if (id <= 0)
        {
            TempData["ErrorMessage"] = "Невалидно ID на новина.";
            return RedirectToAction(nameof(Index));
        }

        var (success, message) = await _newsService.DeleteNewsAsync(id);

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

    [HttpGet]
    [Authorize(Roles = "Admin,Teacher,Parent")]
    public async Task<IActionResult> ImportantMessages()
    {
        IEnumerable<AnnouncementViewModel> announcements =
            await _newsService.GetAllImportantMessagesAsync();

        return View(announcements);
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
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var (success, message) = await _newsService.AddAnnouncementAsync(model);

        if (success)
        {
            TempData["SuccessMessage"] = message;
            return RedirectToAction(nameof(ImportantMessages));
        }

        TempData["ErrorMessage"] = message;
        return View(model);
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> EditAnnouncement(int id)
    {
        var announcement = await _newsService.GetAnnouncementForEditAsync(id);

        if (announcement == null)
        {
            TempData["ErrorMessage"] = "Съобщението не беше намерено.";
            return RedirectToAction(nameof(ImportantMessages));
        }

        return View(announcement);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> EditAnnouncement(int id, AddAnnouncementViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var (success, message) = await _newsService.EditAnnouncementAsync(id, model);

        if (success)
        {
            TempData["SuccessMessage"] = message;
            return RedirectToAction(nameof(ImportantMessages));
        }

        TempData["ErrorMessage"] = message;
        return View(model);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> DeleteAnnouncement(int id)
    {
        var (success, message) = await _newsService.DeleteAnnouncementAsync(id);

        if (success)
        {
            TempData["SuccessMessage"] = message;
        }
        else
        {
            TempData["ErrorMessage"] = message;
        }

        return RedirectToAction(nameof(ImportantMessages));
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Achievements()
    {
        IEnumerable<NewsViewModel> achievements =
            await _newsService.GetAllAchievementsAsync();

        return View(achievements);
    }
}