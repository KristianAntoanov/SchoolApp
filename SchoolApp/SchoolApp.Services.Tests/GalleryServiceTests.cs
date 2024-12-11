using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.Data.Models;
using SchoolApp.Data.Repository;
using SchoolApp.Data.Repository.Contracts;
using SchoolApp.Services.Data;

namespace SchoolApp.Services.Tests;

[TestFixture]
public class GalleryServiceTests
{
    private ApplicationDbContext _dbContext;
    private IRepository _repository;
    private GalleryService _galleryService;

    private Album _testAlbum;
    private GalleryImage _testGalleryImage;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestGalleryDb")
            .Options;

        _dbContext = new ApplicationDbContext(options);
        _repository = new BaseRepository(_dbContext);
        _galleryService = new GalleryService(_repository);

        _testAlbum = new Album
        {
            Id = Guid.NewGuid(),
            Title = "Test Album",
            Description = "Test Album Description"
        };

        _testGalleryImage = new GalleryImage
        {
            Id = Guid.NewGuid(),
            ImageUrl = "test-image-url",
            AlbumId = _testAlbum.Id,
            Album = _testAlbum
        };

        _dbContext.Albums.Add(_testAlbum);
        _dbContext.GalleryImages.Add(_testGalleryImage);
        _dbContext.SaveChanges();
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    [Test]
    public async Task GetAllAlbumsWithImagesAsync_ShouldReturnAllAlbums_WhenAlbumsExist()
    {
        // Act
        var result = await _galleryService.GetAllAlbumsWithImagesAsync();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(1));

        var album = result.First();
        Assert.Multiple(() =>
        {
            Assert.That(album.Id, Is.EqualTo(_testAlbum.Id));
            Assert.That(album.Title, Is.EqualTo(_testAlbum.Title));
            Assert.That(album.Description, Is.EqualTo(_testAlbum.Description));
            Assert.That(album.Images, Has.Count.EqualTo(1));
        });

        var image = album.Images.First();
        Assert.That(image.ImageUrl, Is.EqualTo(_testGalleryImage.ImageUrl));
    }

    [Test]
    public async Task GetAllAlbumsWithImagesAsync_ShouldReturnEmptyCollection_WhenNoAlbumsExist()
    {
        // Arrange
        _dbContext.GalleryImages.RemoveRange(_dbContext.GalleryImages);
        _dbContext.Albums.RemoveRange(_dbContext.Albums);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _galleryService.GetAllAlbumsWithImagesAsync();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task GetAllAlbumsWithImagesAsync_ShouldReturnAlbumWithMultipleImages_WhenAlbumHasMultipleImages()
    {
        // Arrange
        var secondImage = new GalleryImage
        {
            Id = Guid.NewGuid(),
            ImageUrl = "second-test-image-url",
            AlbumId = _testAlbum.Id,
            Album = _testAlbum
        };

        await _dbContext.GalleryImages.AddAsync(secondImage);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _galleryService.GetAllAlbumsWithImagesAsync();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(1));

        var album = result.First();
        Assert.That(album.Images, Has.Count.EqualTo(2));

        var images = album.Images.ToList();
        Assert.Multiple(() =>
        {
            Assert.That(images.Any(i => i.ImageUrl == "test-image-url"), Is.True);
            Assert.That(images.Any(i => i.ImageUrl == "second-test-image-url"), Is.True);
        });
    }

    [Test]
    public async Task GetAllAlbumsWithImagesAsync_ShouldReturnAlbumWithoutImages_WhenAlbumHasNoImages()
    {
        // Arrange
        var albumWithoutImages = new Album
        {
            Id = Guid.NewGuid(),
            Title = "Empty Album",
            Description = "Album without images"
        };

        await _dbContext.Albums.AddAsync(albumWithoutImages);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _galleryService.GetAllAlbumsWithImagesAsync();

        // Assert
        Assert.That(result.Count(), Is.EqualTo(2));

        var emptyAlbum = result.First(a => a.Title == "Empty Album");
        Assert.Multiple(() =>
        {
            Assert.That(emptyAlbum.Id, Is.EqualTo(albumWithoutImages.Id));
            Assert.That(emptyAlbum.Title, Is.EqualTo(albumWithoutImages.Title));
            Assert.That(emptyAlbum.Description, Is.EqualTo(albumWithoutImages.Description));
            Assert.That(emptyAlbum.Images, Is.Empty);
        });
    }

    [Test]
    public async Task GetAllAlbumsWithImagesAsync_ShouldReturnMultipleAlbums_WhenMultipleAlbumsExist()
    {
        // Arrange
        var secondAlbum = new Album
        {
            Id = Guid.NewGuid(),
            Title = "Second Album",
            Description = "Second Album Description"
        };

        var secondAlbumImage = new GalleryImage
        {
            Id = Guid.NewGuid(),
            ImageUrl = "second-album-image-url",
            AlbumId = secondAlbum.Id,
            Album = secondAlbum
        };

        await _dbContext.Albums.AddAsync(secondAlbum);
        await _dbContext.GalleryImages.AddAsync(secondAlbumImage);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _galleryService.GetAllAlbumsWithImagesAsync();

        // Assert
        Assert.That(result.Count(), Is.EqualTo(2));

        var firstAlbum = result.First(a => a.Title == "Test Album");
        var secondAlbumResult = result.First(a => a.Title == "Second Album");

        Assert.Multiple(() =>
        {
            Assert.That(firstAlbum.Images, Has.Count.EqualTo(1));
            Assert.That(secondAlbumResult.Images, Has.Count.EqualTo(1));
            Assert.That(firstAlbum.Images.First().ImageUrl, Is.EqualTo("test-image-url"));
            Assert.That(secondAlbumResult.Images.First().ImageUrl, Is.EqualTo("second-album-image-url"));
        });
    }
}