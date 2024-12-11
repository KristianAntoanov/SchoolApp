using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

using Moq;

using SchoolApp.Data;
using SchoolApp.Data.Models;
using SchoolApp.Data.Repository;
using SchoolApp.Data.Repository.Contracts;
using SchoolApp.Services.Data.Contrancts;
using SchoolApp.Services.Data;
using SchoolApp.Web.ViewModels.Admin.Gallery;
using SchoolApp.Web.ViewModels.Admin.Gallery.Components;

using static SchoolApp.Common.TempDataMessages.Gallery;

namespace SchoolApp.Services.AdminGalleryServiceTests;

[TestFixture]
public class AdminGalleryServiceTests
{
    private ApplicationDbContext _dbContext;
    private IRepository _repository;
    private IAzureBlobService _blobServiceMock;
    private AdminGalleryService _galleryService;

    private Album _testAlbum;
    private GalleryImage _testImage;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestGalleryDb")
            .Options;

        _dbContext = new ApplicationDbContext(options);
        _repository = new BaseRepository(_dbContext);

        var blobServiceMock = new Mock<IAzureBlobService>();
        _blobServiceMock = blobServiceMock.Object;

        _galleryService = new AdminGalleryService(_repository, _blobServiceMock);

        // Setup test data
        _testAlbum = new Album
        {
            Id = Guid.NewGuid(),
            Title = "Test Album",
            Description = "Test Description"
        };

        _testImage = new GalleryImage
        {
            Id = Guid.NewGuid(),
            ImageUrl = "test-url",
            AlbumId = _testAlbum.Id
        };

        _dbContext.Albums.Add(_testAlbum);
        _dbContext.GalleryImages.Add(_testImage);
        _dbContext.SaveChanges();
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    [Test]
    public async Task GetAllAlbumsWithImagesAsync_ShouldReturnAllAlbums()
    {
        // Act
        var result = await _galleryService.GetAllAlbumsWithImagesAsync();

        // Assert
        Assert.That(result.Count(), Is.EqualTo(1));
        var album = result.First();
        Assert.Multiple(() =>
        {
            Assert.That(album.Title, Is.EqualTo(_testAlbum.Title));
            Assert.That(album.Description, Is.EqualTo(_testAlbum.Description));
            Assert.That(album.Images.Count(), Is.EqualTo(1));
            Assert.That(album.Images.First().ImageUrl, Is.EqualTo(_testImage.ImageUrl));
        });
    }

    //----------------GetAllAlbumsWithImagesAsync---------------------
    //----------------AddAlbumAsync---------------------
    [Test]
    public async Task AddAlbumAsync_WithExistingTitle_ShouldReturnError()
    {
        // Arrange
        var model = new AddAlbumViewModel
        {
            Title = _testAlbum.Title,
            Description = "New Description"
        };

        // Act
        var (success, message) = await _galleryService.AddAlbumAsync(model);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(success, Is.False);
            Assert.That(message, Is.EqualTo(AlbumExistsError));
        });

        var albumsCount = await _dbContext.Albums.CountAsync();
        Assert.That(albumsCount, Is.EqualTo(1));
    }

    [Test]
    public async Task AddAlbumAsync_WithNullAlbum_ShouldReturnError()
    {
        // Arrange
        AddAlbumViewModel model = new AddAlbumViewModel
        {
            Title = null!,
            Description = null!
        };
        model = null;

        // Act
        var (success, message) = await _galleryService.AddAlbumAsync(model);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(success, Is.False);
            Assert.That(message, Is.EqualTo(AlbumCreateError));
        });

        var albumsCount = await _dbContext.Albums.CountAsync();
        Assert.That(albumsCount, Is.EqualTo(1));
    }

    [Test]
    public async Task AddAlbumAsync_WithValidData_ShouldCreateNewAlbum()
    {
        // Arrange
        var model = new AddAlbumViewModel
        {
            Title = "Unique Title",
            Description = "Test Description"
        };

        // Act
        var (success, message) = await _galleryService.AddAlbumAsync(model);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(success, Is.True);
            Assert.That(message, Is.EqualTo(AlbumCreateSuccess));
        });

        var addedAlbum = await _dbContext.Albums
            .FirstOrDefaultAsync(a => a.Title == model.Title);

        Assert.That(addedAlbum, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(addedAlbum.Title, Is.EqualTo(model.Title));
            Assert.That(addedAlbum.Description, Is.EqualTo(model.Description));
            Assert.That(addedAlbum.Id, Is.Not.EqualTo(Guid.Empty));
        });

        var albumsCount = await _dbContext.Albums.CountAsync();
        Assert.That(albumsCount, Is.EqualTo(2));
    }
    //----------------AddAlbumAsync---------------------
    //----------------GetDetailsForAlbumAsync---------------------

    [Test]
    public async Task GetDetailsForAlbumAsync_WithValidId_ShouldReturnAlbum()
    {
        // Arrange
        string validId = _testAlbum.Id.ToString();

        // Act
        var result = await _galleryService.GetDetailsForAlbumAsync(validId);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result!.Id, Is.EqualTo(_testAlbum.Id.ToString()));
            Assert.That(result.Title, Is.EqualTo(_testAlbum.Title));
            Assert.That(result.Description, Is.EqualTo(_testAlbum.Description));
            Assert.That(result.Images.Count(), Is.EqualTo(1));
            Assert.That(result.Images.First().ImageUrl, Is.EqualTo(_testImage.ImageUrl));
        });
    }

    [Test]
    public async Task GetDetailsForAlbumAsync_WithInvalidGuid_ShouldReturnNull()
    {
        // Arrange
        string invalidId = "invalid-guid";

        // Act
        var result = await _galleryService.GetDetailsForAlbumAsync(invalidId);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetDetailsForAlbumAsync_WithNonExistingId_ShouldReturnNull()
    {
        // Arrange
        string nonExistingId = Guid.NewGuid().ToString();

        // Act
        var result = await _galleryService.GetDetailsForAlbumAsync(nonExistingId);

        // Assert
        Assert.That(result, Is.Null);
    }

    //----------------GetDetailsForAlbumAsync---------------------
    //----------------AddImagesAsync---------------------
    [Test]
    public async Task AddImagesAsync_WithInvalidGuid_ShouldReturnError()
    {
        // Arrange
        var model = new AddAlbumImageFormModel
        {
            AlbumId = "invalid-guid",
            Image = Mock.Of<IFormFile>()
        };

        // Act
        var (success, message) = await _galleryService.AddImagesAsync(model);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(success, Is.False);
            Assert.That(message, Is.EqualTo(InvalidAlbumId));
        });
    }

    [Test]
    public async Task AddImagesAsync_WithNonExistentAlbum_ShouldReturnError()
    {
        // Arrange
        var model = new AddAlbumImageFormModel
        {
            AlbumId = Guid.NewGuid().ToString(),
            Image = Mock.Of<IFormFile>()
        };

        // Act
        var (success, message) = await _galleryService.AddImagesAsync(model);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(success, Is.False);
            Assert.That(message, Is.EqualTo(AlbumNotFound));
        });
    }

    [Test]
    public async Task AddImagesAsync_WithOversizedImage_ShouldReturnError()
    {
        // Arrange
        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(f => f.Length).Returns(3 * 1024 * 1024); // 3MB

        var model = new AddAlbumImageFormModel
        {
            AlbumId = _testAlbum.Id.ToString(),
            Image = mockFile.Object
        };

        // Act
        var (success, message) = await _galleryService.AddImagesAsync(model);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(success, Is.False);
            Assert.That(message, Is.EqualTo(ImageSizeError));
        });
    }

    [Test]
    public async Task AddImagesAsync_WithInvalidExtension_ShouldReturnError()
    {
        // Arrange
        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(f => f.Length).Returns(1 * 1024 * 1024); // 1MB
        mockFile.Setup(f => f.FileName).Returns("test.gif");

        var model = new AddAlbumImageFormModel
        {
            AlbumId = _testAlbum.Id.ToString(),
            Image = mockFile.Object
        };

        // Act
        var (success, message) = await _galleryService.AddImagesAsync(model);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(success, Is.False);
            Assert.That(message, Is.EqualTo(AllowedFormatsMessage));
        });
    }

    [Test]
    public async Task AddImagesAsync_WithValidData_ShouldCreateNewImage()
    {
        // Arrange
        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(f => f.Length).Returns(1 * 1024 * 1024); // 1MB
        mockFile.Setup(f => f.FileName).Returns("test.jpg");

        var model = new AddAlbumImageFormModel
        {
            AlbumId = _testAlbum.Id.ToString(),
            Image = mockFile.Object
        };

        var blobServiceMock = new Mock<IAzureBlobService>();
        blobServiceMock
            .Setup(b => b.UploadGalleryImageAsync(It.IsAny<IFormFile>(), It.IsAny<Guid>()))
            .ReturnsAsync((true, null, "https://test-url.com/image.jpg"));

        _galleryService = new AdminGalleryService(_repository, blobServiceMock.Object);

        // Act
        var (success, message) = await _galleryService.AddImagesAsync(model);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(success, Is.True);
            Assert.That(message, Is.EqualTo(ImageUploadSuccess));
        });

        var addedImage = await _dbContext.GalleryImages
            .FirstOrDefaultAsync(i => i.ImageUrl == "https://test-url.com/image.jpg");

        Assert.That(addedImage, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(addedImage.ImageUrl, Is.EqualTo("https://test-url.com/image.jpg"));
            Assert.That(addedImage.Id, Is.Not.EqualTo(Guid.Empty));
        });
    }

    [Test]
    public async Task AddImagesAsync_WhenBlobUploadFails_ShouldReturnError()
    {
        // Arrange
        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(f => f.Length).Returns(1 * 1024 * 1024);
        mockFile.Setup(f => f.FileName).Returns("test.jpg");

        var model = new AddAlbumImageFormModel
        {
            AlbumId = _testAlbum.Id.ToString(),
            Image = mockFile.Object
        };

        var blobServiceMock = new Mock<IAzureBlobService>();
        blobServiceMock
            .Setup(b => b.UploadGalleryImageAsync(It.IsAny<IFormFile>(), It.IsAny<Guid>()))
            .ReturnsAsync((false, ImageUploadError, (string)null));

        _galleryService = new AdminGalleryService(_repository, blobServiceMock.Object);

        // Act
        var (success, message) = await _galleryService.AddImagesAsync(model);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(success, Is.False);
            Assert.That(message, Is.EqualTo(ImageUploadError));
        });

        var imagesCount = await _dbContext.GalleryImages.CountAsync();
        Assert.That(imagesCount, Is.EqualTo(1));
    }

    [Test]
    public async Task AddImagesAsync_WithNullImage_ShouldReturnError()
    {
        // Arrange
        var model = new AddAlbumImageFormModel
        {
            AlbumId = _testAlbum.Id.ToString(),
            Image = null!
        };

        // Act
        var (success, message) = await _galleryService.AddImagesAsync(model);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(success, Is.False);
            Assert.That(message, Is.EqualTo(InvalidImage));
        });
    }
    //----------------AddImagesAsync---------------------
    //----------------DeleteImageAsync---------------------
    [Test]
    public async Task DeleteImageAsync_WithInvalidGuid_ShouldReturnError()
    {
        // Arrange
        string invalidId = "invalid-guid";

        // Act
        var (success, message) = await _galleryService.DeleteImageAsync(invalidId);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(success, Is.False);
            Assert.That(message, Is.EqualTo(InvalidImageId));
        });
    }

    [Test]
    public async Task DeleteImageAsync_WithNonExistingImage_ShouldReturnError()
    {
        // Arrange
        string nonExistingId = Guid.NewGuid().ToString();

        // Act
        var (success, message) = await _galleryService.DeleteImageAsync(nonExistingId);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(success, Is.False);
            Assert.That(message, Is.EqualTo(ImageNotFound));
        });
    }

    [Test]
    public async Task DeleteImageAsync_WhenBlobDeleteFails_ShouldReturnError()
    {
        // Arrange
        string imageId = _testImage.Id.ToString();

        var blobServiceMock = new Mock<IAzureBlobService>();
        blobServiceMock
            .Setup(b => b.DeleteGalleryImageAsync(It.IsAny<string>()))
            .ReturnsAsync(false);

        _galleryService = new AdminGalleryService(_repository, blobServiceMock.Object);

        // Act
        var (success, message) = await _galleryService.DeleteImageAsync(imageId);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(success, Is.False);
            Assert.That(message, Is.EqualTo(ImageDeleteBlobError));
        });

        var imageStillExists = await _dbContext.GalleryImages
            .AnyAsync(i => i.Id == _testImage.Id);
        Assert.That(imageStillExists, Is.True);
    }

    [Test]
    public async Task DeleteImageAsync_WithValidData_ShouldDeleteImage()
    {
        // Arrange
        string imageId = _testImage.Id.ToString();

        var blobServiceMock = new Mock<IAzureBlobService>();
        blobServiceMock
            .Setup(b => b.DeleteGalleryImageAsync(It.IsAny<string>()))
            .ReturnsAsync(true);

        _galleryService = new AdminGalleryService(_repository, blobServiceMock.Object);

        // Act
        var (success, message) = await _galleryService.DeleteImageAsync(imageId);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(success, Is.True);
            Assert.That(message, Is.EqualTo(ImageDeleteSuccess));
        });

        var imageExists = await _dbContext.GalleryImages
            .AnyAsync(i => i.Id == _testImage.Id);
        Assert.That(imageExists, Is.False);

        blobServiceMock.Verify(
            b => b.DeleteGalleryImageAsync(_testImage.ImageUrl),
            Times.Once);
    }
    //----------------DeleteImageAsync---------------------
    //----------------DeleteAlbumAsync---------------------
    [Test]
    public async Task DeleteAlbumAsync_WithInvalidGuid_ShouldReturnError()
    {
        // Arrange
        string invalidId = "invalid-guid";

        // Act
        var (success, message) = await _galleryService.DeleteAlbumAsync(invalidId);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(success, Is.False);
            Assert.That(message, Is.EqualTo(InvalidAlbumId));
        });
    }

    [Test]
    public async Task DeleteAlbumAsync_WithNonExistingAlbum_ShouldReturnError()
    {
        // Arrange
        string nonExistingId = Guid.NewGuid().ToString();

        // Act
        var (success, message) = await _galleryService.DeleteAlbumAsync(nonExistingId);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(success, Is.False);
            Assert.That(message, Is.EqualTo(AlbumNotFound));
        });
    }

    [Test]
    public async Task DeleteAlbumAsync_WhenBlobDeleteFails_ShouldReturnError()
    {
        // Arrange
        string albumId = _testAlbum.Id.ToString();

        var blobServiceMock = new Mock<IAzureBlobService>();
        blobServiceMock
            .Setup(b => b.DeleteGalleryImageAsync(It.IsAny<string>()))
            .ReturnsAsync(false);

        _galleryService = new AdminGalleryService(_repository, blobServiceMock.Object);

        // Act
        var (success, message) = await _galleryService.DeleteAlbumAsync(albumId);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(success, Is.False);
            Assert.That(message, Is.EqualTo(ImagesDeleteBlobError));
        });

        var albumExists = await _dbContext.Albums
            .AnyAsync(a => a.Id == _testAlbum.Id);
        var imageExists = await _dbContext.GalleryImages
            .AnyAsync(i => i.Id == _testImage.Id);

        Assert.Multiple(() =>
        {
            Assert.That(albumExists, Is.True);
            Assert.That(imageExists, Is.True);
        });
    }

    [Test]
    public async Task DeleteAlbumAsync_WithEmptyAlbum_ShouldDeleteAlbum()
    {
        // Arrange
        var emptyAlbum = new Album
        {
            Id = Guid.NewGuid(),
            Title = "Empty Album",
            Description = "Test Description"
        };
        await _dbContext.Albums.AddAsync(emptyAlbum);
        await _dbContext.SaveChangesAsync();

        var blobServiceMock = new Mock<IAzureBlobService>();
        _galleryService = new AdminGalleryService(_repository, blobServiceMock.Object);

        // Act
        var (success, message) = await _galleryService.DeleteAlbumAsync(emptyAlbum.Id.ToString());

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(success, Is.True);
            Assert.That(message, Is.EqualTo(AlbumDeleteSuccess));
        });

        var albumExists = await _dbContext.Albums
            .AnyAsync(a => a.Id == emptyAlbum.Id);
        Assert.That(albumExists, Is.False);

        blobServiceMock.Verify(
            b => b.DeleteGalleryImageAsync(It.IsAny<string>()),
            Times.Never);
    }

    [Test]
    public async Task DeleteAlbumAsync_WithImagesAlbum_ShouldDeleteAlbumAndImages()
    {
        // Arrange
        string albumId = _testAlbum.Id.ToString();

        var blobServiceMock = new Mock<IAzureBlobService>();
        blobServiceMock
            .Setup(b => b.DeleteGalleryImageAsync(It.IsAny<string>()))
            .ReturnsAsync(true);

        _galleryService = new AdminGalleryService(_repository, blobServiceMock.Object);

        // Act
        var (success, message) = await _galleryService.DeleteAlbumAsync(albumId);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(success, Is.True);
            Assert.That(message, Is.EqualTo(AlbumDeleteSuccess));
        });

        var albumExists = await _dbContext.Albums
            .AnyAsync(a => a.Id == _testAlbum.Id);
        var imagesExist = await _dbContext.GalleryImages
            .AnyAsync(i => i.AlbumId == _testAlbum.Id);

        Assert.Multiple(() =>
        {
            Assert.That(albumExists, Is.False);
            Assert.That(imagesExist, Is.False);
        });

        // Verify blob service was called for each image
        blobServiceMock.Verify(
            b => b.DeleteGalleryImageAsync(_testImage.ImageUrl),
            Times.Once);
    }
    //----------------DeleteAlbumAsync---------------------
}