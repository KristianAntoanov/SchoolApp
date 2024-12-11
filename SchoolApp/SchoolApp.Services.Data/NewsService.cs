using Microsoft.EntityFrameworkCore;

using SchoolApp.Data.Models;
using SchoolApp.Data.Repository.Contracts;
using SchoolApp.Services.Data.Contrancts;
using SchoolApp.Web.ViewModels.News;

using static SchoolApp.Common.ApplicationConstants;
using static SchoolApp.Common.TempDataMessages.News;

namespace SchoolApp.Services.Data;

public class NewsService : INewsService
{
    private readonly IRepository _repository;
    private readonly IAzureBlobService _blobService;

    public NewsService(IRepository repository, IAzureBlobService blobService)
    {
        _repository = repository;
        _blobService = blobService;
    }

    public async Task<IEnumerable<NewsViewModel>> GetAllNewsAsync()
    {
        IEnumerable<NewsViewModel> allNews = await _repository
            .GetAllAttached<News>()
            .Where(n => !n.IsArchived && n.Category == NewsCategory.News)
            .OrderByDescending(n => n.PublicationDate)
            .Select(n => new NewsViewModel()
            {
                Id = n.Id,
                Title = n.Title,
                Content = n.Content,
                PublicationDate = n.PublicationDate,
                ImageUrl = n.ImageUrl ?? DefaultNewsImageUrl,
                Category = n.Category
            })
            .ToArrayAsync();

        return allNews;
    }

    public async Task<(bool success, string message)> AddNewsAsync(AddNewsViewModel model)
    {
        string imageUrl = DefaultNewsImageUrl;


        if (model.Image != null)
        {
            if (model.Image.Length > 2 * 1024 * 1024)
            {
                return (false, ImageSizeError);
            }

            string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };
            string extension = Path.GetExtension(model.Image.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
            {
                return (false, AllowedFormatsMessage);
            }


            News news = new News()
            {
                Title = model.Title,
                Content = model.Content,
                Category = model.Category,
                PublicationDate = DateTime.Now,
                IsArchived = false
            };

            await _repository.AddAsync(news);

            var (isSuccessful, errorMessage, uploadedImageUrl) =
                await _blobService.UploadNewsImageAsync(model.Image, $"{news.Title}-{news.Id}");

            if (!isSuccessful || string.IsNullOrEmpty(uploadedImageUrl))
            {
                return (false, errorMessage ?? ImageUploadError);
            }

            news.ImageUrl = uploadedImageUrl;
            await _repository.UpdateAsync(news);

            return (true, NewsCreateSuccess);
        }
        else
        {
            News news = new News()
            {
                Title = model.Title,
                Content = model.Content,
                Category = model.Category,
                PublicationDate = DateTime.Now,
                IsArchived = false,
                ImageUrl = imageUrl
            };

            await _repository.AddAsync(news);

            return (true, NewsCreateSuccess);
        }
    }

    public async Task<NewsViewModel?> GetNewsDetailsAsync(int id)
    {
        NewsViewModel? news = await _repository
            .GetAllAttached<News>()
            .Where(n => n.Id == id && !n.IsArchived)
            .Select(n => new NewsViewModel()
            {
                Id = n.Id,
                Title = n.Title,
                Content = n.Content,
                PublicationDate = n.PublicationDate,
                ImageUrl = n.ImageUrl,
                Category = n.Category
            })
            .FirstOrDefaultAsync();

        return news;
    }

    public async Task<(bool success, string message)> DeleteNewsAsync(int id)
    {
        News? news = await _repository
            .GetAllAttached<News>()
            .FirstOrDefaultAsync(n => n.Id == id && !n.IsArchived);

        if (news == null)
        {
            return (false, NotFoundMessage);
        }

        news.IsArchived = true;

        bool result = await _repository.UpdateAsync(news);

        if (result)
        {
            return (true, NewsDeleteSuccess);
        }

        return (false, NewsDeleteError);
    }

    public async Task<IEnumerable<AnnouncementViewModel>> GetAllImportantMessagesAsync()
    {
        IEnumerable<AnnouncementViewModel> announcements = await _repository
            .GetAllAttached<Announcement>()
            .OrderByDescending(a => a.PublicationDate)
            .Select(a => new AnnouncementViewModel()
            {
                Id = a.Id,
                Title = a.Title,
                Content = a.Content,
                PublicationDate = a.PublicationDate
            })
            .ToArrayAsync();

        return announcements;
    }

    public async Task<(bool success, string message)> AddAnnouncementAsync(AddAnnouncementViewModel model)
    {
        if (model == null)
        {
            return (false, AnnouncementErrorMessage);
        }

        Announcement announcement = new Announcement()
        {
            Title = model.Title,
            Content = model.Content,
            PublicationDate = DateTime.Now
        };

        await _repository.AddAsync(announcement);

        return (true, AnnouncementCreateSuccess);
    }

    public async Task<AddAnnouncementViewModel?> GetAnnouncementForEditAsync(int id)
    {
        AddAnnouncementViewModel? announcement = await _repository
            .GetAllAttached<Announcement>()
            .Where(a => a.Id == id)
            .Select(a => new AddAnnouncementViewModel()
            {
                Title = a.Title,
                Content = a.Content
            })
            .FirstOrDefaultAsync();

        return announcement;
    }

    public async Task<(bool success, string message)> EditAnnouncementAsync(int id, AddAnnouncementViewModel model)
    {
        if (model == null)
        {
            return (false, AnnouncementNotFoundMessage);
        }

        Announcement? announcement = await _repository.GetByIdAsync<Announcement>(id);

        if (announcement == null)
        {
            return (false, AnnouncementNotFoundMessage);
        }

        announcement.Title = model.Title;
        announcement.Content = model.Content;
        announcement.PublicationDate = DateTime.Now;

        bool result = await _repository.UpdateAsync(announcement);

        if (result)
        {
            return (result, AnnouncementEditSuccess);
        }
        return (result, AnnouncementNotFoundMessage);
    }

    public async Task<(bool success, string message)> DeleteAnnouncementAsync(int id)
    {
        bool deleted = await _repository.DeleteAsync<Announcement>(id);

        if (!deleted)
        {
            return (false, AnnouncementNotFoundMessage);
        }

        return (true, AnnouncementDeleteSuccess);
    }

    public async Task<IEnumerable<NewsViewModel>> GetAllAchievementsAsync()
    {
        IEnumerable<NewsViewModel> achievements = await _repository
            .GetAllAttached<News>()
            .Where(n => !n.IsArchived && n.Category == NewsCategory.Achievement)
            .OrderByDescending(n => n.PublicationDate)
            .Select(n => new NewsViewModel()
            {
                Id = n.Id,
                Title = n.Title,
                Content = n.Content,
                PublicationDate = n.PublicationDate,
                ImageUrl = n.ImageUrl ?? DefaultNewsImageUrl,
                Category = n.Category
            })
            .ToArrayAsync();

        return achievements;
    }

    public async Task<AddNewsViewModel?> GetNewsForEditAsync(int id)
    {
        AddNewsViewModel? news = await _repository
            .GetAllAttached<News>()
            .Where(n => n.Id == id && !n.IsArchived)
            .Select(n => new AddNewsViewModel()
            {
                Title = n.Title,
                Content = n.Content,
                Category = n.Category
            })
            .FirstOrDefaultAsync();

        return news;
    }

    public async Task<(bool success, string message)> EditNewsAsync(int id, AddNewsViewModel model)
    {
        News? news = await _repository.GetByIdAsync<News>(id);

        if (news == null)
        {
            return (false, NotFoundMessage);
        }
        if (news.Title == model.Title && news.Content == model.Content &&
            news.Category == model.Category && model.Image == null)
        {
            return (true, NewsEditSuccess);
        }


        if (model.Image != null)
        {
            if (model.Image.Length > 2 * 1024 * 1024)
            {
                return (false, ImageSizeError);
            }

            string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };
            string extension = Path.GetExtension(model.Image.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
            {
                return (false, AllowedFormatsMessage);
            }

            if (!string.IsNullOrEmpty(news.ImageUrl) && news.ImageUrl != DefaultNewsImageUrl)
            {
                await _blobService.DeleteNewsImageAsync(news.ImageUrl);
            }

            var (isSuccessful, errorMessage, uploadedImageUrl) =
                await _blobService.UploadNewsImageAsync(model.Image, $"{model.Title}-{id}");

            if (!isSuccessful || string.IsNullOrEmpty(uploadedImageUrl))
            {
                return (false, errorMessage ?? ImageUploadError);
            }

            news.ImageUrl = uploadedImageUrl;
        }

        news.Title = model.Title;
        news.Content = model.Content;
        news.Category = model.Category;
        news.PublicationDate = DateTime.Now;

        await _repository.UpdateAsync(news);

        return (true, NewsEditSuccess);
    }
}