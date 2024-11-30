using Microsoft.EntityFrameworkCore;
using SchoolApp.Data.Models;
using SchoolApp.Data.Repository.Contracts;
using SchoolApp.Services.Data.Contrancts;
using SchoolApp.Web.ViewModels.News;

namespace SchoolApp.Services.Data
{
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
                    ImageUrl = n.ImageUrl ?? "/img/default-news-image.jpg",
                    Category = n.Category
                })
                .ToArrayAsync();

            return allNews;
        }

        public async Task<(bool success, string message)> AddNewsAsync(AddNewsViewModel model)
        {
            string imageUrl = "/img/default-news-image.jpeg";


            if (model.Image != null)
            {
                if (model.Image.Length > 2 * 1024 * 1024)
                {
                    return (false, "Снимката трябва да е по-малка от 2MB!");
                }

                string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };
                string extension = Path.GetExtension(model.Image.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(extension))
                {
                    return (false, "Позволените формати са само JPG, JPEG и PNG!");
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
                    return (false, errorMessage ?? "Възникна грешка при качването на снимката.");
                }

                news.ImageUrl = uploadedImageUrl;
                await _repository.UpdateAsync(news);

                return (true, "Новината беше създадена успешно!");
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

                return (true, "Новината беше създадена успешно!");
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
                return (false, "Новината не беше намерена.");
            }

            if (!string.IsNullOrEmpty(news.ImageUrl) && news.ImageUrl != "/img/default-news-image.jpeg")
            {
                bool isImageDeleted = await _blobService.DeleteNewsImageAsync(news.ImageUrl);
                if (!isImageDeleted)
                {
                    return (false, "Възникна грешка при изтриването на снимката от хранилището!");
                }
            }

            bool result = await _repository.DeleteAsync<News>(id);
            if (result)
            {
                return (true, "Новината беше изтрита успешно!");
            }

            return (false, "Възникна грешка при изтриването на новината!");
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
            Announcement announcement = new Announcement()
            {
                Title = model.Title,
                Content = model.Content,
                PublicationDate = DateTime.Now
            };

            await _repository.AddAsync(announcement);

            return (true, "Съобщението беше създадено успешно!");
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
            Announcement? announcement = await _repository.GetByIdAsync<Announcement>(id);

            if (announcement == null)
            {
                return (false, "Съобщението не беше намерено!");
            }

            announcement.Title = model.Title;
            announcement.Content = model.Content;
            announcement.PublicationDate = DateTime.Now;

            await _repository.UpdateAsync(announcement);

            return (true, "Съобщението беше редактирано успешно!");
        }

        public async Task<(bool success, string message)> DeleteAnnouncementAsync(int id)
        {
            bool deleted = await _repository.DeleteAsync<Announcement>(id);

            if (!deleted)
            {
                return (false, "Съобщението не беше намерено!");
            }

            return (true, "Съобщението беше изтрито успешно!");
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
                    ImageUrl = n.ImageUrl ?? "/img/default-news-image.jpg",
                    Category = n.Category
                })
                .ToArrayAsync();

            return achievements;
        }
    }
}