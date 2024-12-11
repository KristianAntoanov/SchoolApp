using Microsoft.AspNetCore.Http;
using Moq;

using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

using SchoolApp.Services.Data;
using static SchoolApp.Common.ErrorMessages;
using Azure;

namespace SchoolApp.Services.Tests;

[TestFixture]
public class AzureBlobServiceTests
{
    private Mock<BlobServiceClient> _blobServiceClientMock;
    private Mock<BlobContainerClient> _teacherContainerClientMock;
    private Mock<BlobClient> _blobClientMock;
    private AzureBlobService _azureBlobService;

    [SetUp]
    public void Setup()
    {
        _blobServiceClientMock = new Mock<BlobServiceClient>();
        _teacherContainerClientMock = new Mock<BlobContainerClient>();
        _blobClientMock = new Mock<BlobClient>();

        _blobServiceClientMock
            .Setup(x => x.GetBlobContainerClient(It.IsAny<string>()))
            .Returns(_teacherContainerClientMock.Object);

        _teacherContainerClientMock
            .Setup(x => x.GetBlobClient(It.IsAny<string>()))
            .Returns(_blobClientMock.Object);

        _blobClientMock
            .Setup(x => x.Uri)
            .Returns(new Uri("https://test.blob.core.windows.net/teachers/test-image.jpg"));

        _azureBlobService = new AzureBlobService(_blobServiceClientMock.Object);
    }

    [Test]
    public async Task UploadTeacherImageAsync_ShouldReturnSuccessResult_WhenFileIsValid()
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();
        var fileName = "test.jpg";
        var ms = new MemoryStream();

        fileMock.Setup(f => f.FileName).Returns(fileName);
        fileMock.Setup(f => f.Length).Returns(ms.Length);
        fileMock.Setup(f => f.OpenReadStream()).Returns(ms);

        _blobClientMock
            .Setup(x => x.UploadAsync(It.IsAny<Stream>(), true, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Mock<Azure.Response<BlobContentInfo>>().Object);

        // Act
        var (isSuccessful, errorMessage, imageUrl) = await _azureBlobService.UploadTeacherImageAsync(fileMock.Object, "John", "Doe");

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(isSuccessful, Is.True);
            Assert.That(errorMessage, Is.Empty);
            Assert.That(imageUrl, Is.Not.Null);
            Assert.That(imageUrl, Does.Contain("test.blob.core.windows.net"));
        });

        _blobClientMock.Verify(
            x => x.UploadAsync(It.IsAny<Stream>(), true, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task UploadTeacherImageAsync_ShouldGenerateCorrectBlobName()
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();
        var fileName = "test.jpg";
        var ms = new MemoryStream();
        var firstName = "John";
        var lastName = "Doe";

        fileMock.Setup(f => f.FileName).Returns(fileName);
        fileMock.Setup(f => f.Length).Returns(ms.Length);
        fileMock.Setup(f => f.OpenReadStream()).Returns(ms);

        string capturedBlobName = null;
        _teacherContainerClientMock
            .Setup(x => x.GetBlobClient(It.IsAny<string>()))
            .Callback<string>(blobName => capturedBlobName = blobName)
            .Returns(_blobClientMock.Object);

        // Act
        await _azureBlobService.UploadTeacherImageAsync(fileMock.Object, firstName, lastName);

        // Assert
        Assert.That(capturedBlobName, Is.EqualTo($"teacher-{firstName.ToLower()}-{lastName.ToLower()}.jpg"));
    }

    [Test]
    public async Task UploadTeacherImageAsync_ShouldHandleDifferentFileExtensions()
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();
        var extensions = new[] { ".jpg", ".png", ".gif", ".jpeg" };

        foreach (var extension in extensions)
        {
            var fileName = $"test{extension}";
            fileMock.Setup(f => f.FileName).Returns(fileName);
            fileMock.Setup(f => f.OpenReadStream()).Returns(new MemoryStream());

            // Act
            var (isSuccessful, errorMessage, imageUrl) = await _azureBlobService.UploadTeacherImageAsync(fileMock.Object, "John", "Doe");

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(isSuccessful, Is.True);
                Assert.That(errorMessage, Is.Empty);
                Assert.That(imageUrl, Is.Not.Null);
            });
        }
    }

    [Test]
    public async Task UploadTeacherImageAsync_ShouldReturnFalse_WhenFileIsNull()
    {
        // Arrange
        IFormFile file = null;

        // Act
        var (isSuccessful, errorMessage, imageUrl) = await _azureBlobService.UploadTeacherImageAsync(file, "John", "Doe");

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(isSuccessful, Is.False);
            Assert.That(errorMessage, Is.EqualTo(TeacherImageRequiredMessage));
            Assert.That(imageUrl, Is.EqualTo(string.Empty));
        });
    }

    [TestCase("", "Doe")]
    [TestCase("John", "")]
    [TestCase(" ", "Doe")]
    [TestCase("John", " ")]
    [TestCase(null, "Doe")]
    [TestCase("John", null)]
    public async Task UploadTeacherImageAsync_ShouldReturnError_WhenNameIsInvalid(string? firstName, string? lastName)
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.FileName).Returns("test.jpg");
        fileMock.Setup(f => f.OpenReadStream()).Returns(new MemoryStream());

        // Act
        var (isSuccessful, errorMessage, imageUrl) = await _azureBlobService.UploadTeacherImageAsync(fileMock.Object, firstName, lastName);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(isSuccessful, Is.False);
            Assert.That(errorMessage, Is.EqualTo(TeacherNameRequiredMessage));
            Assert.That(imageUrl, Is.EqualTo(string.Empty));
        });
    }

    [Test]
    public async Task UploadTeacherImageAsync_ShouldOverwriteExistingFile()
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.FileName).Returns("test.jpg");
        fileMock.Setup(f => f.OpenReadStream()).Returns(new MemoryStream());

        // Act
        var (isSuccessful, errorMessage, imageUrl) = await _azureBlobService.UploadTeacherImageAsync(fileMock.Object, "John", "Doe");

        // Assert
        _blobClientMock.Verify(
            x => x.UploadAsync(It.IsAny<Stream>(), true, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    //----------------DeleteTeacherImageAsync---------------------
    [Test]
    public async Task DeleteTeacherImageAsync_ShouldReturnTrue_WhenImageExists()
    {
        // Arrange
        var imageUrl = "https://test.blob.core.windows.net/teachers/teacher-john-doe.jpg";

        _blobClientMock
            .Setup(x => x.ExistsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Response.FromValue(true, new Mock<Response>().Object));

        // Act
        var result = await _azureBlobService.DeleteTeacherImageAsync(imageUrl);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public async Task DeleteTeacherImageAsync_ShouldReturnFalse_WhenImageDoesNotExist()
    {
        // Arrange
        var imageUrl = "https://test.blob.core.windows.net/teachers/nonexistent.jpg";

        _blobClientMock
            .Setup(x => x.ExistsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Response.FromValue(false, new Mock<Response>().Object));

        // Act
        var result = await _azureBlobService.DeleteTeacherImageAsync(imageUrl);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public async Task DeleteTeacherImageAsync_ShouldThrowException_WhenUrlIsNull()
    {
        // Arrange
        string imageUrl = null;

        // Act
        bool result = await _azureBlobService.DeleteTeacherImageAsync(imageUrl);

        // Assert
        Assert.That(result, Is.False);
    }

    [TestCase("")]
    [TestCase(" ")]
    public async Task DeleteTeacherImageAsync_ShouldReturnFalse_WhenUrlIsEmptyOrWhitespace(string imageUrl)
    {
        // Act
        bool result = await _azureBlobService.DeleteTeacherImageAsync(imageUrl);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void DeleteTeacherImageAsync_ShouldThrowException_WhenUrlIsInvalid()
    {
        // Arrange
        var imageUrl = "not-a-valid-url";

        // Act & Assert
        Assert.ThrowsAsync<UriFormatException>(async () =>
            await _azureBlobService.DeleteTeacherImageAsync(imageUrl));
    }

    [Test]
    public async Task DeleteTeacherImageAsync_ShouldExtractCorrectBlobName()
    {
        // Arrange
        var fileName = "teacher-john-doe.jpg";
        var imageUrl = $"https://test.blob.core.windows.net/teachers/{fileName}";
        string capturedBlobName = null;

        _teacherContainerClientMock
            .Setup(x => x.GetBlobClient(It.IsAny<string>()))
            .Callback<string>(blobName => capturedBlobName = blobName)
            .Returns(_blobClientMock.Object);

        _blobClientMock
            .Setup(x => x.ExistsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Response.FromValue(true, new Mock<Response>().Object));

        // Act
        await _azureBlobService.DeleteTeacherImageAsync(imageUrl);

        // Assert
        Assert.That(capturedBlobName, Is.EqualTo(fileName));
    }
    //----------------DeleteTeacherImageAsync---------------------
    //----------------UploadGalleryImageAsync---------------------
    [Test]
    public async Task UploadGalleryImageAsync_ShouldReturnSuccessResult_WhenFileIsValid()
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();
        var fileName = "test.jpg";
        var ms = new MemoryStream();

        fileMock.Setup(f => f.FileName).Returns(fileName);
        fileMock.Setup(f => f.Length).Returns(ms.Length);
        fileMock.Setup(f => f.OpenReadStream()).Returns(ms);

        _blobClientMock
            .Setup(x => x.UploadAsync(It.IsAny<Stream>(), true, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Mock<Response<BlobContentInfo>>().Object);

        // Act
        var (isSuccessful, errorMessage, imageUrl) = await _azureBlobService.UploadGalleryImageAsync(fileMock.Object, Guid.NewGuid());

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(isSuccessful, Is.True);
            Assert.That(errorMessage, Is.Empty);
            Assert.That(imageUrl, Is.Not.Null);
            Assert.That(imageUrl, Does.Contain("test.blob.core.windows.net"));
        });

        _blobClientMock.Verify(
            x => x.UploadAsync(It.IsAny<Stream>(), true, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task UploadGalleryImageAsync_ShouldGenerateCorrectBlobName()
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();
        var fileName = "test.jpg";
        var ms = new MemoryStream();
        Guid imageId = Guid.NewGuid();
        string extension = Path.GetExtension(fileName).ToLowerInvariant();

        fileMock.Setup(f => f.FileName).Returns(fileName);
        fileMock.Setup(f => f.Length).Returns(ms.Length);
        fileMock.Setup(f => f.OpenReadStream()).Returns(ms);

        string capturedBlobName = null;
        _teacherContainerClientMock
            .Setup(x => x.GetBlobClient(It.IsAny<string>()))
            .Callback<string>(blobName => capturedBlobName = blobName)
            .Returns(_blobClientMock.Object);

        // Act
        await _azureBlobService.UploadGalleryImageAsync(fileMock.Object, imageId);

        // Assert
        Assert.That(capturedBlobName, Is.EqualTo($"image-{imageId}{extension}"));
    }

    [Test]
    public async Task UploadGalleryImageAsync_ShouldHandleDifferentFileExtensions()
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();
        var extensions = new[] { ".jpg", ".png", ".gif", ".jpeg" };

        foreach (var extension in extensions)
        {
            var fileName = $"test{extension}";
            fileMock.Setup(f => f.FileName).Returns(fileName);
            fileMock.Setup(f => f.OpenReadStream()).Returns(new MemoryStream());

            // Act
            var (isSuccessful, errorMessage, imageUrl) = await _azureBlobService.UploadGalleryImageAsync(fileMock.Object, Guid.NewGuid());

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(isSuccessful, Is.True);
                Assert.That(errorMessage, Is.Empty);
                Assert.That(imageUrl, Is.Not.Null);
            });
        }
    }

    [Test]
    public async Task UploadGalleryImageAsync_ShouldReturnFalse_WhenFileIsNull()
    {
        // Arrange
        IFormFile file = null;

        // Act
        var (isSuccessful, errorMessage, imageUrl) = await _azureBlobService.UploadGalleryImageAsync(file, Guid.NewGuid());

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(isSuccessful, Is.False);
            Assert.That(errorMessage, Is.EqualTo(GalleryImageRequiredMessage));
            Assert.That(imageUrl, Is.EqualTo(string.Empty));
        });
    }

    [Test]
    public async Task UploadGalleryImageAsync_ShouldReturnError_WhenImageIdIsInvalid()
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.FileName).Returns("test.jpg");
        fileMock.Setup(f => f.OpenReadStream()).Returns(new MemoryStream());

        // Act
        var (isSuccessful, errorMessage, imageUrl) = await _azureBlobService.UploadGalleryImageAsync(fileMock.Object, Guid.Empty);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(isSuccessful, Is.False);
            Assert.That(errorMessage, Is.EqualTo(GalleryImageRequiredMessage));
            Assert.That(imageUrl, Is.EqualTo(string.Empty));
        });
    }

    [Test]
    public async Task UploadGalleryImageAsync_ShouldOverwriteExistingFile()
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.FileName).Returns("test.jpg");
        fileMock.Setup(f => f.OpenReadStream()).Returns(new MemoryStream());

        // Act
        var (isSuccessful, errorMessage, imageUrl) = await _azureBlobService.UploadGalleryImageAsync(fileMock.Object, Guid.NewGuid());

        // Assert
        _blobClientMock.Verify(
            x => x.UploadAsync(It.IsAny<Stream>(), true, It.IsAny<CancellationToken>()),
            Times.Once);
    }
    //----------------UploadGalleryImageAsync---------------------
    //----------------DeleteGalleryImageAsync---------------------
    [Test]
    public async Task DeleteGalleryImageAsync_ShouldReturnTrue_WhenImageExists()
    {
        // Arrange
        var imageUrl = "https://test.blob.core.windows.net/image/image-D846Df-daswd-dwad2.jpg";

        _blobClientMock
            .Setup(x => x.ExistsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Response.FromValue(true, new Mock<Response>().Object));

        // Act
        var result = await _azureBlobService.DeleteGalleryImageAsync(imageUrl);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public async Task DeleteGalleryImageAsync_ShouldReturnFalse_WhenImageDoesNotExist()
    {
        // Arrange
        var imageUrl = "https://test.blob.core.windows.net/image/nonexistent.jpg";

        _blobClientMock
            .Setup(x => x.ExistsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Response.FromValue(false, new Mock<Response>().Object));

        // Act
        var result = await _azureBlobService.DeleteGalleryImageAsync(imageUrl);

        // Assert
        Assert.That(result, Is.False);
    }

    [TestCase("")]
    [TestCase(" ")]
    [TestCase(null)]
    public async Task DeleteGalleryImageAsync_ShouldThrowException_WhenUrlIsNull(string? imageUrl)
    {
        // Act
        bool result = await _azureBlobService.DeleteGalleryImageAsync(imageUrl);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void DeleteGalleryImageAsync_ShouldThrowException_WhenUrlIsInvalid()
    {
        // Arrange
        var imageUrl = "not-a-valid-url";

        // Act & Assert
        Assert.ThrowsAsync<UriFormatException>(async () =>
            await _azureBlobService.DeleteGalleryImageAsync(imageUrl));
    }

    [Test]
    public async Task DeleteGalleryImageAsync_ShouldExtractCorrectBlobName()
    {
        // Arrange
        var fileName = "image-DE78D7eda-dwaw2d3-2daw.jpg";
        var imageUrl = $"https://test.blob.core.windows.net/images/{fileName}";
        string capturedBlobName = null;

        _teacherContainerClientMock
            .Setup(x => x.GetBlobClient(It.IsAny<string>()))
            .Callback<string>(blobName => capturedBlobName = blobName)
            .Returns(_blobClientMock.Object);

        _blobClientMock
            .Setup(x => x.ExistsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Response.FromValue(true, new Mock<Response>().Object));

        // Act
        await _azureBlobService.DeleteGalleryImageAsync(imageUrl);

        // Assert
        Assert.That(capturedBlobName, Is.EqualTo(fileName));
    }
    //----------------DeleteGalleryImageAsync---------------------
    //----------------UploadNewsImageAsync---------------------

    [Test]
    public async Task UploadNewsImageAsync_ShouldReturnSuccessResult_WhenFileIsValid()
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();
        var fileName = "test.jpg";
        var ms = new MemoryStream();
        var title = "Test News";

        fileMock.Setup(f => f.FileName).Returns(fileName);
        fileMock.Setup(f => f.Length).Returns(ms.Length);
        fileMock.Setup(f => f.OpenReadStream()).Returns(ms);

        _blobClientMock
            .Setup(x => x.UploadAsync(It.IsAny<Stream>(), true, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Mock<Response<BlobContentInfo>>().Object);

        // Act
        var (isSuccessful, errorMessage, imageUrl) = await _azureBlobService.UploadNewsImageAsync(fileMock.Object, title);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(isSuccessful, Is.True);
            Assert.That(errorMessage, Is.Empty);
            Assert.That(imageUrl, Is.Not.Null);
            Assert.That(imageUrl, Does.Contain("test.blob.core.windows.net"));
        });

        _blobClientMock.Verify(
            x => x.UploadAsync(It.IsAny<Stream>(), true, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task UploadNewsImageAsync_ShouldGenerateCorrectBlobName()
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();
        var fileName = "test.jpg";
        var ms = new MemoryStream();
        var title = "Test News";
        string extension = Path.GetExtension(fileName).ToLowerInvariant();

        fileMock.Setup(f => f.FileName).Returns(fileName);
        fileMock.Setup(f => f.Length).Returns(ms.Length);
        fileMock.Setup(f => f.OpenReadStream()).Returns(ms);

        string capturedBlobName = null;
        _teacherContainerClientMock
            .Setup(x => x.GetBlobClient(It.IsAny<string>()))
            .Callback<string>(blobName => capturedBlobName = blobName)
            .Returns(_blobClientMock.Object);

        // Act
        await _azureBlobService.UploadNewsImageAsync(fileMock.Object, title);

        // Assert
        Assert.That(capturedBlobName, Is.EqualTo($"news-{title.ToLower()}{extension}"));
    }

    [Test]
    public async Task UploadNewsImageAsync_ShouldHandleDifferentFileExtensions()
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();
        var extensions = new[] { ".jpg", ".png", ".gif", ".jpeg" };
        var title = "Test News";

        foreach (var extension in extensions)
        {
            var fileName = $"test{extension}";
            fileMock.Setup(f => f.FileName).Returns(fileName);
            fileMock.Setup(f => f.OpenReadStream()).Returns(new MemoryStream());

            // Act
            var (isSuccessful, errorMessage, imageUrl) = await _azureBlobService.UploadNewsImageAsync(fileMock.Object, title);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(isSuccessful, Is.True);
                Assert.That(errorMessage, Is.Empty);
                Assert.That(imageUrl, Is.Not.Null);
            });
        }
    }

    [Test]
    public async Task UploadNewsImageAsync_ShouldOverwriteExistingFile()
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();
        var title = "Test News";
        fileMock.Setup(f => f.FileName).Returns("test.jpg");
        fileMock.Setup(f => f.OpenReadStream()).Returns(new MemoryStream());

        // Act
        var (isSuccessful, errorMessage, imageUrl) = await _azureBlobService.UploadNewsImageAsync(fileMock.Object, title);

        // Assert
        _blobClientMock.Verify(
            x => x.UploadAsync(It.IsAny<Stream>(), true, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    //----------------DeleteNewsImageAsync---------------------
    [Test]
    public async Task DeleteNewsImageAsync_ShouldReturnTrue_WhenImageExists()
    {
        // Arrange
        var imageUrl = "https://test.blob.core.windows.net/news/news-test-title.jpg";

        _blobClientMock
            .Setup(x => x.ExistsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Response.FromValue(true, new Mock<Response>().Object));

        // Act
        var result = await _azureBlobService.DeleteNewsImageAsync(imageUrl);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public async Task DeleteNewsImageAsync_ShouldReturnFalse_WhenImageDoesNotExist()
    {
        // Arrange
        var imageUrl = "https://test.blob.core.windows.net/news/nonexistent.jpg";

        _blobClientMock
            .Setup(x => x.ExistsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Response.FromValue(false, new Mock<Response>().Object));

        // Act
        var result = await _azureBlobService.DeleteNewsImageAsync(imageUrl);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void DeleteNewsImageAsync_ShouldThrowException_WhenUrlIsInvalid()
    {
        // Arrange
        var imageUrl = "not-a-valid-url";

        // Act & Assert
        Assert.ThrowsAsync<UriFormatException>(async () =>
            await _azureBlobService.DeleteNewsImageAsync(imageUrl));
    }

    [Test]
    public async Task DeleteNewsImageAsync_ShouldExtractCorrectBlobName()
    {
        // Arrange
        var fileName = "news-test-title.jpg";
        var imageUrl = $"https://test.blob.core.windows.net/news/{fileName}";
        string capturedBlobName = null;

        _teacherContainerClientMock
            .Setup(x => x.GetBlobClient(It.IsAny<string>()))
            .Callback<string>(blobName => capturedBlobName = blobName)
            .Returns(_blobClientMock.Object);

        _blobClientMock
            .Setup(x => x.ExistsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Response.FromValue(true, new Mock<Response>().Object));

        // Act
        await _azureBlobService.DeleteNewsImageAsync(imageUrl);

        // Assert
        Assert.That(capturedBlobName, Is.EqualTo(fileName));
    }
}