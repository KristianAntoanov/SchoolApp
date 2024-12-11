using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;

using SchoolApp.Data;
using SchoolApp.Data.Models;
using SchoolApp.Data.Repository;
using SchoolApp.Data.Repository.Contracts;
using SchoolApp.Services.Data;
using SchoolApp.Services.Data.Contrancts;
using SchoolApp.Web.ViewModels.News;

using static SchoolApp.Common.ApplicationConstants;
using static SchoolApp.Common.TempDataMessages.News;

namespace SchoolApp.Services.Tests;

[TestFixture]
public class NewsServiceTests
{
    private ApplicationDbContext _dbContext;
    private IRepository _repository;
    private Mock<IAzureBlobService> _blobServiceMock;
    private NewsService _newsService;

    private News _testNews;
    private News _testArchivedNews;
    private News _testAchievementNews;
    private Announcement _testAnnouncement;
    private Announcement _testOldAnnouncement;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestNewsDb")
            .Options;

        _dbContext = new ApplicationDbContext(options);
        _repository = new BaseRepository(_dbContext);
        _blobServiceMock = new Mock<IAzureBlobService>();
        _newsService = new NewsService(_repository, _blobServiceMock.Object);

        _testNews = new News
        {
            Id = 1,
            Title = "Test News",
            Content = "Test Content",
            PublicationDate = DateTime.Now.AddDays(-1),
            Category = NewsCategory.News,
            IsArchived = false,
            ImageUrl = "test-image-url"
        };

        _testArchivedNews = new News
        {
            Id = 2,
            Title = "Archived News",
            Content = "Archived Content",
            PublicationDate = DateTime.Now.AddDays(-2),
            Category = NewsCategory.News,
            IsArchived = true,
            ImageUrl = null
        };

        _testAchievementNews = new News
        {
            Id = 3,
            Title = "Achievement News",
            Content = "Achievement Content",
            PublicationDate = DateTime.Now.AddDays(-3),
            Category = NewsCategory.Achievement,
            IsArchived = false,
            ImageUrl = "achievement-image-url"
        };

        SetupAnnouncements();

        _dbContext.News.AddRange(_testNews, _testArchivedNews, _testAchievementNews);
        _dbContext.SaveChanges();
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    private void SetupAnnouncements()
    {
        _testAnnouncement = new Announcement
        {
            Id = 1,
            Title = "Test Announcement",
            Content = "Test Content",
            PublicationDate = DateTime.Now.AddHours(-1)
        };

        _testOldAnnouncement = new Announcement
        {
            Id = 2,
            Title = "Old Announcement",
            Content = "Old Content",
            PublicationDate = DateTime.Now.AddDays(-1)
        };

        _dbContext.Announcements.AddRange(_testAnnouncement, _testOldAnnouncement);
        _dbContext.SaveChanges();
    }

    //----------------GetAllNewsAsync---------------------
    [Test]
    public async Task GetAllNewsAsync_ShouldReturnOnlyNewsCategory()
    {
        // Act
        var result = await _newsService.GetAllNewsAsync();

        // Assert
        Assert.That(result.Count(), Is.EqualTo(1));
        var news = result.First();
        Assert.Multiple(() =>
        {
            Assert.That(news.Id, Is.EqualTo(_testNews.Id));
            Assert.That(news.Title, Is.EqualTo(_testNews.Title));
            Assert.That(news.Category, Is.EqualTo(NewsCategory.News));
        });
    }

    [Test]
    public async Task GetAllNewsAsync_ShouldNotReturnArchivedNews()
    {
        // Act
        var result = await _newsService.GetAllNewsAsync();

        // Assert
        Assert.That(result.Any(n => n.Title == _testArchivedNews.Title), Is.False);
    }

    [Test]
    public async Task GetAllNewsAsync_ShouldOrderByPublicationDateDescending()
    {
        // Arrange
        var recentNews = new News
        {
            Id = 4,
            Title = "Recent News",
            Content = "Recent Content",
            PublicationDate = DateTime.Now,
            Category = NewsCategory.News,
            IsArchived = false
        };
        await _dbContext.News.AddAsync(recentNews);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _newsService.GetAllNewsAsync();

        // Assert
        var orderedNews = result.ToList();
        Assert.Multiple(() =>
        {
            Assert.That(orderedNews, Has.Count.EqualTo(2));
            Assert.That(orderedNews[0].Title, Is.EqualTo("Recent News"));
            Assert.That(orderedNews[1].Title, Is.EqualTo("Test News"));
        });
    }

    [Test]
    public async Task GetAllNewsAsync_ShouldUseDefaultImageUrl_WhenImageUrlIsNull()
    {
        // Arrange
        var newsWithoutImage = new News
        {
            Id = 5,
            Title = "No Image News",
            Content = "No Image Content",
            PublicationDate = DateTime.Now,
            Category = NewsCategory.News,
            IsArchived = false,
            ImageUrl = null
        };
        await _dbContext.News.AddAsync(newsWithoutImage);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _newsService.GetAllNewsAsync();

        // Assert
        var newsWithDefaultImage = result.First(n => n.Title == "No Image News");
        Assert.That(newsWithDefaultImage.ImageUrl, Is.EqualTo(DefaultNewsImageUrl));
    }

    [Test]
    public async Task GetAllNewsAsync_ShouldReturnEmptyCollection_WhenNoNewsExists()
    {
        // Arrange
        _dbContext.News.RemoveRange(_dbContext.News);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _newsService.GetAllNewsAsync();

        // Assert
        Assert.That(result, Is.Empty);
    }
    //----------------GetAllNewsAsync---------------------

    //----------------AddNewsAsync---------------------
    [Test]
    public async Task AddNewsAsync_WithoutImage_ShouldCreateNewsWithDefaultImage()
    {
        // Arrange
        var model = new AddNewsViewModel
        {
            Title = "New News",
            Content = "New Content",
            Category = NewsCategory.News,
            Image = null
        };

        // Act
        var (success, message) = await _newsService.AddNewsAsync(model);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(success, Is.True);
            Assert.That(message, Is.EqualTo(NewsCreateSuccess));
        });

        var addedNews = await _dbContext.News
            .FirstOrDefaultAsync(n => n.Title == model.Title);

        Assert.That(addedNews, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(addedNews!.Content, Is.EqualTo(model.Content));
            Assert.That(addedNews.Category, Is.EqualTo(model.Category));
            Assert.That(addedNews.ImageUrl, Is.EqualTo(DefaultNewsImageUrl));
            Assert.That(addedNews.IsArchived, Is.False);
        });
    }

    [Test]
    public async Task AddNewsAsync_WithOversizedImage_ShouldReturnError()
    {
        // Arrange
        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(f => f.Length).Returns(3 * 1024 * 1024); // 3MB
        mockFile.Setup(f => f.FileName).Returns("test.jpg");

        var model = new AddNewsViewModel
        {
            Title = "News with Big Image",
            Content = "Content",
            Category = NewsCategory.News,
            Image = mockFile.Object
        };

        // Act
        var (success, message) = await _newsService.AddNewsAsync(model);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(success, Is.False);
            Assert.That(message, Is.EqualTo(ImageSizeError));
        });

        var newsExists = await _dbContext.News
            .AnyAsync(n => n.Title == model.Title);
        Assert.That(newsExists, Is.False);
    }

    [Test]
    public async Task AddNewsAsync_WithInvalidImageExtension_ShouldReturnError()
    {
        // Arrange
        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(f => f.Length).Returns(1 * 1024 * 1024);
        mockFile.Setup(f => f.FileName).Returns("test.gif");

        var model = new AddNewsViewModel
        {
            Title = "News with Invalid Image",
            Content = "Content",
            Category = NewsCategory.News,
            Image = mockFile.Object
        };

        // Act
        var (success, message) = await _newsService.AddNewsAsync(model);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(success, Is.False);
            Assert.That(message, Is.EqualTo(AllowedFormatsMessage));
        });

        var newsExists = await _dbContext.News
            .AnyAsync(n => n.Title == model.Title);
        Assert.That(newsExists, Is.False);
    }

    [Test]
    public async Task AddNewsAsync_WithValidImage_ShouldUploadAndCreateNews()
    {
        // Arrange
        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(f => f.Length).Returns(1 * 1024 * 1024);
        mockFile.Setup(f => f.FileName).Returns("test.jpg");

        const string uploadedImageUrl = "http://test-storage/image.jpg";
        _blobServiceMock.Setup(b => b.UploadNewsImageAsync(
                It.IsAny<IFormFile>(),
                It.IsAny<string>()))
            .ReturnsAsync((true, null, uploadedImageUrl));

        var model = new AddNewsViewModel
        {
            Title = "News with Image",
            Content = "Content",
            Category = NewsCategory.News,
            Image = mockFile.Object
        };

        // Act
        var (success, message) = await _newsService.AddNewsAsync(model);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(success, Is.True);
            Assert.That(message, Is.EqualTo(NewsCreateSuccess));
        });

        var addedNews = await _dbContext.News
            .FirstOrDefaultAsync(n => n.Title == model.Title);

        Assert.That(addedNews, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(addedNews!.ImageUrl, Is.EqualTo(uploadedImageUrl));
            Assert.That(addedNews.IsArchived, Is.False);
        });
    }

    [Test]
    public async Task AddNewsAsync_WhenImageUploadFails_ShouldReturnError()
    {
        // Arrange
        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(f => f.Length).Returns(1 * 1024 * 1024);
        mockFile.Setup(f => f.FileName).Returns("test.jpg");

        _blobServiceMock.Setup(b => b.UploadNewsImageAsync(
                It.IsAny<IFormFile>(),
                It.IsAny<string>()))
            .ReturnsAsync((false, "Upload failed", null as string));

        var model = new AddNewsViewModel
        {
            Title = "News with Failed Image",
            Content = "Content",
            Category = NewsCategory.News,
            Image = mockFile.Object
        };

        // Act
        var (success, message) = await _newsService.AddNewsAsync(model);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(success, Is.False);
            Assert.That(message, Is.EqualTo("Upload failed"));
        });
    }

    [Test]
    public async Task AddNewsAsync_ShouldSetCorrectPublicationDate()
    {
        // Arrange
        var model = new AddNewsViewModel
        {
            Title = "News with Date",
            Content = "Content",
            Category = NewsCategory.News
        };

        var beforeAdd = DateTime.Now;

        // Act
        await _newsService.AddNewsAsync(model);

        // Assert
        var addedNews = await _dbContext.News
            .FirstOrDefaultAsync(n => n.Title == model.Title);

        Assert.That(addedNews, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(addedNews!.PublicationDate, Is.GreaterThanOrEqualTo(beforeAdd));
            Assert.That(addedNews.PublicationDate, Is.LessThanOrEqualTo(DateTime.Now));
        });
    }

    //----------------GetNewsDetailsAsync---------------------
    [Test]
    public async Task GetNewsDetailsAsync_WithValidId_ShouldReturnCorrectNews()
    {
        // Act
        var result = await _newsService.GetNewsDetailsAsync(_testNews.Id);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result!.Id, Is.EqualTo(_testNews.Id));
            Assert.That(result.Title, Is.EqualTo(_testNews.Title));
            Assert.That(result.Content, Is.EqualTo(_testNews.Content));
            Assert.That(result.PublicationDate, Is.EqualTo(_testNews.PublicationDate));
            Assert.That(result.ImageUrl, Is.EqualTo(_testNews.ImageUrl));
            Assert.That(result.Category, Is.EqualTo(_testNews.Category));
        });
    }

    [Test]
    public async Task GetNewsDetailsAsync_WithInvalidId_ShouldReturnNull()
    {
        // Arrange
        int invalidId = 999;

        // Act
        var result = await _newsService.GetNewsDetailsAsync(invalidId);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetNewsDetailsAsync_WithArchivedNews_ShouldReturnNull()
    {
        // Act
        var result = await _newsService.GetNewsDetailsAsync(_testArchivedNews.Id);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetNewsDetailsAsync_ShouldReturnNews_RegardlessOfCategory()
    {
        // Act
        var result = await _newsService.GetNewsDetailsAsync(_testAchievementNews.Id);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result!.Id, Is.EqualTo(_testAchievementNews.Id));
            Assert.That(result.Category, Is.EqualTo(NewsCategory.Achievement));
        });
    }

    [Test]
    public async Task GetNewsDetailsAsync_WithNullImageUrl_ShouldReturnNullImageUrl()
    {
        // Arrange
        var newsWithoutImage = new News
        {
            Id = 4,
            Title = "No Image News",
            Content = "Content",
            PublicationDate = DateTime.Now,
            Category = NewsCategory.News,
            IsArchived = false,
            ImageUrl = null
        };
        await _dbContext.News.AddAsync(newsWithoutImage);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _newsService.GetNewsDetailsAsync(newsWithoutImage.Id);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.ImageUrl, Is.Null);
    }

    //----------------DeleteNewsAsync---------------------
    [Test]
    public async Task DeleteNewsAsync_WithValidId_ShouldArchiveNews()
    {
        // Act
        var (success, message) = await _newsService.DeleteNewsAsync(_testNews.Id);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(success, Is.True);
            Assert.That(message, Is.EqualTo(NewsDeleteSuccess));
        });

        var archivedNews = await _dbContext.News
            .FirstOrDefaultAsync(n => n.Id == _testNews.Id);

        Assert.That(archivedNews, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(archivedNews!.IsArchived, Is.True);
            Assert.That(archivedNews.Title, Is.EqualTo(_testNews.Title));
            Assert.That(archivedNews.Content, Is.EqualTo(_testNews.Content));
            Assert.That(archivedNews.ImageUrl, Is.EqualTo(_testNews.ImageUrl));
        });
    }

    [Test]
    public async Task DeleteNewsAsync_WithInvalidId_ShouldReturnError()
    {
        // Arrange
        int invalidId = 999;

        // Act
        var (success, message) = await _newsService.DeleteNewsAsync(invalidId);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(success, Is.False);
            Assert.That(message, Is.EqualTo(NotFoundMessage));
        });
    }

    [Test]
    public async Task DeleteNewsAsync_WithAlreadyArchivedNews_ShouldReturnError()
    {
        // Act
        var (success, message) = await _newsService.DeleteNewsAsync(_testArchivedNews.Id);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(success, Is.False);
            Assert.That(message, Is.EqualTo(NotFoundMessage));
        });
    }

    [Test]
    public async Task DeleteNewsAsync_WhenUpdateFails_ShouldReturnError()
    {
        // Arrange
        var mockRepository = new Mock<IRepository>();
        mockRepository
            .Setup(r => r.GetAllAttached<News>())
            .Returns(_dbContext.News);
        mockRepository
            .Setup(r => r.UpdateAsync(It.IsAny<News>()))
            .ReturnsAsync(false);

        var serviceWithMockRepo = new NewsService(mockRepository.Object, _blobServiceMock.Object);

        // Act
        var (success, message) = await serviceWithMockRepo.DeleteNewsAsync(_testNews.Id);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(success, Is.False);
            Assert.That(message, Is.EqualTo(NewsDeleteError));
        });
    }

    [Test]
    public async Task DeleteNewsAsync_ShouldNotAffectOtherNews()
    {
        // Arrange
        var initialNewsCount = await _dbContext.News.CountAsync();

        // Act
        await _newsService.DeleteNewsAsync(_testNews.Id);

        // Assert
        var allNews = await _dbContext.News.ToListAsync();
        Assert.Multiple(() =>
        {
            Assert.That(allNews, Has.Count.EqualTo(initialNewsCount));

            var achievementNews = allNews.First(n => n.Id == _testAchievementNews.Id);
            Assert.That(achievementNews.IsArchived, Is.False);

            var alreadyArchivedNews = allNews.First(n => n.Id == _testArchivedNews.Id);
            Assert.That(alreadyArchivedNews.IsArchived, Is.True);
        });
    }

    [Test]
    public async Task DeleteNewsAsync_ShouldPreserveAllNewsProperties_ExceptIsArchived()
    {
        // Act
        await _newsService.DeleteNewsAsync(_testNews.Id);

        // Assert
        var archivedNews = await _dbContext.News
            .FirstOrDefaultAsync(n => n.Id == _testNews.Id);

        Assert.That(archivedNews, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(archivedNews!.Title, Is.EqualTo(_testNews.Title));
            Assert.That(archivedNews.Content, Is.EqualTo(_testNews.Content));
            Assert.That(archivedNews.Category, Is.EqualTo(_testNews.Category));
            Assert.That(archivedNews.ImageUrl, Is.EqualTo(_testNews.ImageUrl));
            Assert.That(archivedNews.PublicationDate, Is.EqualTo(_testNews.PublicationDate));

            Assert.That(archivedNews.IsArchived, Is.True);
        });
    }

    //----------------GetAllImportantMessagesAsync---------------------
    [Test]
    public async Task GetAllImportantMessagesAsync_ShouldReturnAllAnnouncements()
    {
        // Act
        var result = await _newsService.GetAllImportantMessagesAsync();

        // Assert
        Assert.That(result.Count(), Is.EqualTo(2));
    }

    [Test]
    public async Task GetAllImportantMessagesAsync_ShouldOrderByPublicationDateDescending()
    {
        // Act
        var result = await _newsService.GetAllImportantMessagesAsync();
        var announcements = result.ToList();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(announcements[0].Id, Is.EqualTo(_testAnnouncement.Id));
            Assert.That(announcements[1].Id, Is.EqualTo(_testOldAnnouncement.Id));
        });
    }

    [Test]
    public async Task GetAllImportantMessagesAsync_ShouldMapAllPropertiesCorrectly()
    {
        // Act
        var result = await _newsService.GetAllImportantMessagesAsync();
        var announcement = result.First();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(announcement.Id, Is.EqualTo(_testAnnouncement.Id));
            Assert.That(announcement.Title, Is.EqualTo(_testAnnouncement.Title));
            Assert.That(announcement.Content, Is.EqualTo(_testAnnouncement.Content));
            Assert.That(announcement.PublicationDate, Is.EqualTo(_testAnnouncement.PublicationDate));
        });
    }

    [Test]
    public async Task GetAllImportantMessagesAsync_ShouldReturnEmptyCollection_WhenNoAnnouncementsExist()
    {
        // Arrange
        _dbContext.Announcements.RemoveRange(_dbContext.Announcements);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _newsService.GetAllImportantMessagesAsync();

        // Assert
        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task GetAllImportantMessagesAsync_ShouldHandleMultipleAnnouncements_WithSamePublicationDate()
    {
        // Arrange
        var sameTimeAnnouncements = new[]
        {
            new Announcement
            {
                Id = 3,
                Title = "First Same Time",
                Content = "Content 1",
                PublicationDate = DateTime.Now
            },
            new Announcement
            {
                Id = 4,
                Title = "Second Same Time",
                Content = "Content 2",
                PublicationDate = DateTime.Now
            }
        };

        await _dbContext.Announcements.AddRangeAsync(sameTimeAnnouncements);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _newsService.GetAllImportantMessagesAsync();

        // Assert
        Assert.That(result.Count(), Is.EqualTo(4));

        var firstTwo = result.Take(2).ToList();
        Assert.Multiple(() =>
        {
            Assert.That(firstTwo[0].PublicationDate.ToString("dd.MM.yyyy HH:mm"), Is.EqualTo(sameTimeAnnouncements[0].PublicationDate.ToString("dd.MM.yyyy HH:mm")));
            Assert.That(firstTwo.Select(a => a.Title),
                Does.Contain("First Same Time")
                .And.Contain("Second Same Time"));
        });
    }

    //----------------AddAnnouncementAsync---------------------
    [Test]
    public async Task AddAnnouncementAsync_ShouldCreateAnnouncement_WithCorrectData()
    {
        // Arrange
        var model = new AddAnnouncementViewModel
        {
            Title = "New Announcement",
            Content = "New Content"
        };

        var beforeAdd = DateTime.Now;

        // Act
        var (success, message) = await _newsService.AddAnnouncementAsync(model);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(success, Is.True);
            Assert.That(message, Is.EqualTo(AnnouncementCreateSuccess));
        });

        var addedAnnouncement = await _dbContext.Announcements
            .FirstOrDefaultAsync(a => a.Title == model.Title);

        Assert.That(addedAnnouncement, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(addedAnnouncement!.Content, Is.EqualTo(model.Content));
            Assert.That(addedAnnouncement.PublicationDate, Is.GreaterThanOrEqualTo(beforeAdd));
            Assert.That(addedAnnouncement.PublicationDate, Is.LessThanOrEqualTo(DateTime.Now));
        });
    }

    [Test]
    public async Task AddAnnouncementAsync_ShouldAddToExistingAnnouncements()
    {
        // Arrange
        var initialCount = await _dbContext.Announcements.CountAsync();
        var model = new AddAnnouncementViewModel
        {
            Title = "Additional Announcement",
            Content = "Additional Content"
        };

        // Act
        await _newsService.AddAnnouncementAsync(model);

        // Assert
        var newCount = await _dbContext.Announcements.CountAsync();
        Assert.That(newCount, Is.EqualTo(initialCount + 1));
    }

    [Test]
    public async Task AddAnnouncementAsync_WithNullModel_ShouldStillReturnSuccess()
    {
        // Act
        var (success, message) = await _newsService.AddAnnouncementAsync(null!);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(success, Is.False);
            Assert.That(message, Is.EqualTo(AnnouncementErrorMessage));
        });
    }

    //----------------GetAnnouncementForEditAsync---------------------
    [Test]
    public async Task GetAnnouncementForEditAsync_WithValidId_ShouldReturnCorrectAnnouncement()
    {
        // Act
        var result = await _newsService.GetAnnouncementForEditAsync(_testAnnouncement.Id);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result!.Title, Is.EqualTo(_testAnnouncement.Title));
            Assert.That(result.Content, Is.EqualTo(_testAnnouncement.Content));
        });
    }

    [Test]
    public async Task GetAnnouncementForEditAsync_WithInvalidId_ShouldReturnNull()
    {
        // Arrange
        int invalidId = 999;

        // Act
        var result = await _newsService.GetAnnouncementForEditAsync(invalidId);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetAnnouncementForEditAsync_ShouldReturnCorrectType()
    {
        // Act
        var result = await _newsService.GetAnnouncementForEditAsync(_testAnnouncement.Id);

        // Assert
        Assert.That(result, Is.TypeOf<AddAnnouncementViewModel>());
    }

    [Test]
    public async Task GetAnnouncementForEditAsync_ShouldMapRequiredPropertiesOnly()
    {
        // Act
        var result = await _newsService.GetAnnouncementForEditAsync(_testAnnouncement.Id);

        // Assert
        var properties = typeof(AddAnnouncementViewModel).GetProperties();
        Assert.Multiple(() =>
        {
            Assert.That(properties, Has.Length.EqualTo(2));
            Assert.That(properties.Select(p => p.Name),
                Does.Contain("Title")
                .And.Contain("Content"));
        });
    }

    //----------------EditAnnouncementAsync---------------------
    [Test]
    public async Task EditAnnouncementAsync_WithValidData_ShouldUpdateAnnouncement()
    {
        // Arrange
        var model = new AddAnnouncementViewModel
        {
            Title = "Updated Title",
            Content = "Updated Content"
        };

        var beforeEdit = DateTime.Now;

        // Act
        var (success, message) = await _newsService.EditAnnouncementAsync(_testAnnouncement.Id, model);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(success, Is.True);
            Assert.That(message, Is.EqualTo(AnnouncementEditSuccess));
        });

        var updatedAnnouncement = await _dbContext.Announcements
            .FirstOrDefaultAsync(a => a.Id == _testAnnouncement.Id);

        Assert.That(updatedAnnouncement, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(updatedAnnouncement!.Title, Is.EqualTo(model.Title));
            Assert.That(updatedAnnouncement.Content, Is.EqualTo(model.Content));
            Assert.That(updatedAnnouncement.PublicationDate, Is.GreaterThanOrEqualTo(beforeEdit));
            Assert.That(updatedAnnouncement.PublicationDate, Is.LessThanOrEqualTo(DateTime.Now));
        });
    }

    [Test]
    public async Task EditAnnouncementAsync_WithInvalidId_ShouldReturnError()
    {
        // Arrange
        int invalidId = 999;
        var model = new AddAnnouncementViewModel
        {
            Title = "Updated Title",
            Content = "Updated Content"
        };

        // Act
        var (success, message) = await _newsService.EditAnnouncementAsync(invalidId, model);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(success, Is.False);
            Assert.That(message, Is.EqualTo(AnnouncementNotFoundMessage));
        });
    }

    [Test]
    public async Task EditAnnouncementAsync_WithNullModel_ShouldReturnError()
    {
        // Act
        var (success, message) = await _newsService.EditAnnouncementAsync(_testAnnouncement.Id, null!);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(success, Is.False);
            Assert.That(message, Is.EqualTo(AnnouncementNotFoundMessage));
        });

        var unchangedAnnouncement = await _dbContext.Announcements
            .FirstOrDefaultAsync(a => a.Id == _testAnnouncement.Id);

        Assert.That(unchangedAnnouncement!.Title, Is.EqualTo(_testAnnouncement.Title));
    }

    [Test]
    public async Task EditAnnouncementAsync_WhenUpdateFails_ShouldReturnError()
    {
        // Arrange
        var mockRepository = new Mock<IRepository>();
        mockRepository
            .Setup(r => r.GetByIdAsync<Announcement>(_testAnnouncement.Id))
            .ReturnsAsync(_testAnnouncement);
        mockRepository
            .Setup(r => r.UpdateAsync(It.IsAny<Announcement>()))
            .ReturnsAsync(false);

        var serviceWithMockRepo = new NewsService(mockRepository.Object, _blobServiceMock.Object);

        var model = new AddAnnouncementViewModel
        {
            Title = "Updated Title",
            Content = "Updated Content"
        };

        // Act
        var (success, message) = await serviceWithMockRepo.EditAnnouncementAsync(_testAnnouncement.Id, model);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(success, Is.False);
            Assert.That(message, Is.EqualTo(AnnouncementNotFoundMessage));
        });
    }

    [Test]
    public async Task EditAnnouncementAsync_ShouldNotAffectOtherAnnouncements()
    {
        // Arrange
        var model = new AddAnnouncementViewModel
        {
            Title = "Updated Title",
            Content = "Updated Content"
        };

        var otherAnnouncementBeforeEdit = await _dbContext.Announcements
            .FirstOrDefaultAsync(a => a.Id == _testOldAnnouncement.Id);

        // Act
        await _newsService.EditAnnouncementAsync(_testAnnouncement.Id, model);

        // Assert
        var otherAnnouncementAfterEdit = await _dbContext.Announcements
            .FirstOrDefaultAsync(a => a.Id == _testOldAnnouncement.Id);

        Assert.Multiple(() =>
        {
            Assert.That(otherAnnouncementAfterEdit!.Title, Is.EqualTo(otherAnnouncementBeforeEdit!.Title));
            Assert.That(otherAnnouncementAfterEdit.Content, Is.EqualTo(otherAnnouncementBeforeEdit.Content));
            Assert.That(otherAnnouncementAfterEdit.PublicationDate, Is.EqualTo(otherAnnouncementBeforeEdit.PublicationDate));
        });
    }

    [Test]
    public async Task EditAnnouncementAsync_WithSameData_ShouldStillUpdatePublicationDate()
    {
        // Arrange
        var model = new AddAnnouncementViewModel
        {
            Title = _testAnnouncement.Title,
            Content = _testAnnouncement.Content
        };

        var originalPublicationDate = _testAnnouncement.PublicationDate;
        var beforeEdit = DateTime.Now;

        // Act
        var (success, message) = await _newsService.EditAnnouncementAsync(_testAnnouncement.Id, model);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(success, Is.True);
            Assert.That(message, Is.EqualTo(AnnouncementEditSuccess));
        });

        var updatedAnnouncement = await _dbContext.Announcements
            .FirstOrDefaultAsync(a => a.Id == _testAnnouncement.Id);

        Assert.That(updatedAnnouncement!.PublicationDate, Is.GreaterThan(originalPublicationDate));
    }

    //----------------DeleteAnnouncementAsync---------------------
    [Test]
    public async Task DeleteAnnouncementAsync_WithValidId_ShouldDeleteAnnouncement()
    {
        // Act
        var (success, message) = await _newsService.DeleteAnnouncementAsync(_testAnnouncement.Id);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(success, Is.True);
            Assert.That(message, Is.EqualTo(AnnouncementDeleteSuccess));
        });

        var deletedAnnouncement = await _dbContext.Announcements
            .FirstOrDefaultAsync(a => a.Id == _testAnnouncement.Id);
        Assert.That(deletedAnnouncement, Is.Null);
    }

    [Test]
    public async Task DeleteAnnouncementAsync_WithInvalidId_ShouldReturnError()
    {
        // Arrange
        int invalidId = 999;

        // Act
        var (success, message) = await _newsService.DeleteAnnouncementAsync(invalidId);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(success, Is.False);
            Assert.That(message, Is.EqualTo(AnnouncementNotFoundMessage));
        });
    }

    [Test]
    public async Task DeleteAnnouncementAsync_ShouldNotAffectOtherAnnouncements()
    {
        // Arrange
        var initialCount = await _dbContext.Announcements.CountAsync();

        // Act
        await _newsService.DeleteAnnouncementAsync(_testAnnouncement.Id);

        // Assert
        var remainingCount = await _dbContext.Announcements.CountAsync();
        Assert.That(remainingCount, Is.EqualTo(initialCount - 1));

        var otherAnnouncement = await _dbContext.Announcements
            .FirstOrDefaultAsync(a => a.Id == _testOldAnnouncement.Id);
        Assert.That(otherAnnouncement, Is.Not.Null);
    }

    //----------------GetAllAchievementsAsync---------------------
    [Test]
    public async Task GetAllAchievementsAsync_ShouldReturnOnlyAchievements()
    {
        // Act
        var result = await _newsService.GetAllAchievementsAsync();

        // Assert
        Assert.That(result.Count(), Is.EqualTo(1));
        var achievement = result.First();
        Assert.Multiple(() =>
        {
            Assert.That(achievement.Id, Is.EqualTo(_testAchievementNews.Id));
            Assert.That(achievement.Category, Is.EqualTo(NewsCategory.Achievement));
        });
    }

    [Test]
    public async Task GetAllAchievementsAsync_ShouldNotReturnArchivedAchievements()
    {
        // Arrange
        var archivedAchievement = new News
        {
            Id = 5,
            Title = "Archived Achievement",
            Content = "Content",
            Category = NewsCategory.Achievement,
            PublicationDate = DateTime.Now,
            IsArchived = true
        };
        await _dbContext.News.AddAsync(archivedAchievement);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _newsService.GetAllAchievementsAsync();

        // Assert
        Assert.That(result.Any(n => n.Title == archivedAchievement.Title), Is.False);
    }

    [Test]
    public async Task GetAllAchievementsAsync_ShouldOrderByPublicationDateDescending()
    {
        // Arrange
        var recentAchievement = new News
        {
            Id = 5,
            Title = "Recent Achievement",
            Content = "Content",
            Category = NewsCategory.Achievement,
            PublicationDate = DateTime.Now,
            IsArchived = false
        };
        await _dbContext.News.AddAsync(recentAchievement);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _newsService.GetAllAchievementsAsync();

        // Assert
        var achievements = result.ToList();
        Assert.Multiple(() =>
        {
            Assert.That(achievements, Has.Count.EqualTo(2));
            Assert.That(achievements[0].Title, Is.EqualTo("Recent Achievement"));
            Assert.That(achievements[1].Title, Is.EqualTo(_testAchievementNews.Title));
        });
    }

    [Test]
    public async Task GetAllAchievementsAsync_ShouldUseDefaultImageUrl_WhenImageUrlIsNull()
    {
        // Arrange
        var achievementWithoutImage = new News
        {
            Id = 5,
            Title = "No Image Achievement",
            Content = "Content",
            Category = NewsCategory.Achievement,
            PublicationDate = DateTime.Now,
            IsArchived = false,
            ImageUrl = null
        };
        await _dbContext.News.AddAsync(achievementWithoutImage);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _newsService.GetAllAchievementsAsync();

        // Assert
        var achievement = result.First(n => n.Title == achievementWithoutImage.Title);
        Assert.That(achievement.ImageUrl, Is.EqualTo(DefaultNewsImageUrl));
    }

    [Test]
    public async Task GetAllAchievementsAsync_ShouldReturnEmptyCollection_WhenNoAchievementsExist()
    {
        // Arrange
        News newsToDelete = await _dbContext.News.FirstAsync(n => n.Category == NewsCategory.Achievement);
        _dbContext.Remove(newsToDelete);
        await _dbContext.SaveChangesAsync();
        // Act
        var result = await _newsService.GetAllAchievementsAsync();

        // Assert
        Assert.That(result, Is.Empty);
    }

    //----------------GetNewsForEditAsync---------------------
    [Test]
    public async Task GetNewsForEditAsync_WithValidId_ShouldReturnCorrectNews()
    {
        // Act
        var result = await _newsService.GetNewsForEditAsync(_testNews.Id);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result!.Title, Is.EqualTo(_testNews.Title));
            Assert.That(result.Content, Is.EqualTo(_testNews.Content));
            Assert.That(result.Category, Is.EqualTo(_testNews.Category));
        });
    }

    [Test]
    public async Task GetNewsForEditAsync_WithInvalidId_ShouldReturnNull()
    {
        // Arrange
        int invalidId = 999;

        // Act
        var result = await _newsService.GetNewsForEditAsync(invalidId);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetNewsForEditAsync_WithArchivedNews_ShouldReturnNull()
    {
        // Act
        var result = await _newsService.GetNewsForEditAsync(_testArchivedNews.Id);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetNewsForEditAsync_ShouldReturnCorrectModelType()
    {
        // Act
        var result = await _newsService.GetNewsForEditAsync(_testNews.Id);

        // Assert
        Assert.That(result, Is.TypeOf<AddNewsViewModel>());
    }

    [Test]
    public async Task GetNewsForEditAsync_ShouldNotIncludeImageInViewModel()
    {
        // Act
        var result = await _newsService.GetNewsForEditAsync(_testNews.Id);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Image, Is.Null);
    }

    [Test]
    public async Task GetNewsForEditAsync_ShouldWorkForBothCategories()
    {
        // Act
        var achievementResult = await _newsService.GetNewsForEditAsync(_testAchievementNews.Id);

        // Assert
        Assert.That(achievementResult, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(achievementResult!.Title, Is.EqualTo(_testAchievementNews.Title));
            Assert.That(achievementResult.Category, Is.EqualTo(NewsCategory.Achievement));
        });
    }

    [Test]
    public async Task GetNewsForEditAsync_ShouldMapRequiredPropertiesOnly()
    {
        // Act
        var result = await _newsService.GetNewsForEditAsync(_testNews.Id);

        // Assert
        Assert.That(result, Is.Not.Null);
        var properties = typeof(AddNewsViewModel).GetProperties();

        var expectedProperties = new[] { "Title", "Content", "Category", "Image" };
        var actualProperties = properties.Select(p => p.Name);

        Assert.That(actualProperties, Is.EquivalentTo(expectedProperties));
    }

    //----------------EditNewsAsync---------------------
    [Test]
    public async Task EditNewsAsync_WithValidData_ShouldUpdateNews()
    {
        // Arrange
        var model = new AddNewsViewModel
        {
            Title = "Updated Title",
            Content = "Updated Content",
            Category = NewsCategory.Achievement
        };

        var beforeEdit = DateTime.Now;

        // Act
        var (success, message) = await _newsService.EditNewsAsync(_testNews.Id, model);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(success, Is.True);
            Assert.That(message, Is.EqualTo(NewsEditSuccess));
        });

        var updatedNews = await _dbContext.News
            .FirstOrDefaultAsync(n => n.Id == _testNews.Id);

        Assert.That(updatedNews, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(updatedNews!.Title, Is.EqualTo(model.Title));
            Assert.That(updatedNews.Content, Is.EqualTo(model.Content));
            Assert.That(updatedNews.Category, Is.EqualTo(model.Category));
            Assert.That(updatedNews.PublicationDate, Is.GreaterThanOrEqualTo(beforeEdit));
            Assert.That(updatedNews.PublicationDate, Is.LessThanOrEqualTo(DateTime.Now));
        });
    }

    [Test]
    public async Task EditNewsAsync_WithInvalidId_ShouldReturnError()
    {
        // Arrange
        int invalidId = 999;
        var model = new AddNewsViewModel
        {
            Title = "Updated Title",
            Content = "Updated Content",
            Category = NewsCategory.News
        };

        // Act
        var (success, message) = await _newsService.EditNewsAsync(invalidId, model);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(success, Is.False);
            Assert.That(message, Is.EqualTo(NotFoundMessage));
        });
    }

    [Test]
    public async Task EditNewsAsync_WithNoChanges_ShouldReturnSuccess()
    {
        // Arrange
        var model = new AddNewsViewModel
        {
            Title = _testNews.Title,
            Content = _testNews.Content,
            Category = _testNews.Category
        };

        // Act
        var (success, message) = await _newsService.EditNewsAsync(_testNews.Id, model);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(success, Is.True);
            Assert.That(message, Is.EqualTo(NewsEditSuccess));
        });
    }

    [Test]
    public async Task EditNewsAsync_WithOversizedImage_ShouldReturnError()
    {
        // Arrange
        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(f => f.Length).Returns(3 * 1024 * 1024); // 3MB
        mockFile.Setup(f => f.FileName).Returns("test.jpg");

        var model = new AddNewsViewModel
        {
            Title = "Updated Title",
            Content = "Updated Content",
            Category = NewsCategory.News,
            Image = mockFile.Object
        };

        // Act
        var (success, message) = await _newsService.EditNewsAsync(_testNews.Id, model);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(success, Is.False);
            Assert.That(message, Is.EqualTo(ImageSizeError));
        });

        var unchangedNews = await _dbContext.News
            .FirstOrDefaultAsync(n => n.Id == _testNews.Id);
        Assert.That(unchangedNews!.Title, Is.EqualTo(_testNews.Title));
    }

    [Test]
    public async Task EditNewsAsync_WithInvalidImageExtension_ShouldReturnError()
    {
        // Arrange
        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(f => f.Length).Returns(1 * 1024 * 1024);
        mockFile.Setup(f => f.FileName).Returns("test.gif");

        var model = new AddNewsViewModel
        {
            Title = "Updated Title",
            Content = "Updated Content",
            Category = NewsCategory.News,
            Image = mockFile.Object
        };

        // Act
        var (success, message) = await _newsService.EditNewsAsync(_testNews.Id, model);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(success, Is.False);
            Assert.That(message, Is.EqualTo(AllowedFormatsMessage));
        });
    }

    [Test]
    public async Task EditNewsAsync_WithValidImage_ShouldUpdateImageUrl()
    {
        // Arrange
        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(f => f.Length).Returns(1 * 1024 * 1024);
        mockFile.Setup(f => f.FileName).Returns("test.jpg");

        const string newImageUrl = "http://test-storage/new-image.jpg";
        _blobServiceMock.Setup(b => b.UploadNewsImageAsync(
                It.IsAny<IFormFile>(),
                It.IsAny<string>()))
            .ReturnsAsync((true, null, newImageUrl));

        var model = new AddNewsViewModel
        {
            Title = "Updated Title",
            Content = "Updated Content",
            Category = NewsCategory.News,
            Image = mockFile.Object
        };

        // Act
        var (success, message) = await _newsService.EditNewsAsync(_testNews.Id, model);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(success, Is.True);
            Assert.That(message, Is.EqualTo(NewsEditSuccess));
        });

        var updatedNews = await _dbContext.News
            .FirstOrDefaultAsync(n => n.Id == _testNews.Id);
        Assert.That(updatedNews!.ImageUrl, Is.EqualTo(newImageUrl));

        _blobServiceMock.Verify(b => b.DeleteNewsImageAsync(It.IsAny<string>()), Times.Once);
    }

    [Test]
    public async Task EditNewsAsync_WhenImageUploadFails_ShouldReturnError()
    {
        // Arrange
        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(f => f.Length).Returns(1 * 1024 * 1024);
        mockFile.Setup(f => f.FileName).Returns("test.jpg");

        _blobServiceMock.Setup(b => b.UploadNewsImageAsync(
                It.IsAny<IFormFile>(),
                It.IsAny<string>()))
            .ReturnsAsync((false, "Upload failed", null as string));

        var model = new AddNewsViewModel
        {
            Title = "Updated Title",
            Content = "Updated Content",
            Category = NewsCategory.News,
            Image = mockFile.Object
        };

        // Act
        var (success, message) = await _newsService.EditNewsAsync(_testNews.Id, model);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(success, Is.False);
            Assert.That(message, Is.EqualTo("Upload failed"));
        });

        var unchangedNews = await _dbContext.News
            .FirstOrDefaultAsync(n => n.Id == _testNews.Id);
        Assert.That(unchangedNews!.ImageUrl, Is.EqualTo(_testNews.ImageUrl));
    }

    [Test]
    public async Task EditNewsAsync_WithNewImage_AndDefaultOldImage_ShouldNotDeleteOldImage()
    {
        // Arrange
        var newsWithDefaultImage = new News
        {
            Id = 5,
            Title = "Default Image News",
            Content = "Content",
            Category = NewsCategory.News,
            ImageUrl = DefaultNewsImageUrl,
            IsArchived = false
        };
        await _dbContext.News.AddAsync(newsWithDefaultImage);
        await _dbContext.SaveChangesAsync();

        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(f => f.Length).Returns(1 * 1024 * 1024);
        mockFile.Setup(f => f.FileName).Returns("test.jpg");

        const string newImageUrl = "http://test-storage/new-image.jpg";
        _blobServiceMock.Setup(b => b.UploadNewsImageAsync(
                It.IsAny<IFormFile>(),
                It.IsAny<string>()))
            .ReturnsAsync((true, null, newImageUrl));

        var model = new AddNewsViewModel
        {
            Title = "Updated Title",
            Content = "Updated Content",
            Category = NewsCategory.News,
            Image = mockFile.Object
        };

        // Act
        await _newsService.EditNewsAsync(newsWithDefaultImage.Id, model);

        // Assert
        _blobServiceMock.Verify(b => b.DeleteNewsImageAsync(It.IsAny<string>()), Times.Never);
    }
}