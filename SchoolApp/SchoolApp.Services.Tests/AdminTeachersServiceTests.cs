using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using SchoolApp.Data;
using SchoolApp.Data.Models;
using SchoolApp.Data.Repository;
using SchoolApp.Data.Repository.Contracts;
using SchoolApp.Services.Data;
using SchoolApp.Services.Data.Contrancts;
using SchoolApp.Web.ViewModels.Admin.Teachers;
using static SchoolApp.Common.TempDataMessages.Teachers;

namespace SchoolApp.Services.Tests;

[TestFixture]
public class AdminTeachersServiceTests
{
    private ApplicationDbContext _dbContext;
    private IRepository _repository;
    private IAzureBlobService _azureBlobService;
    private AdminTeachersService _teachersService;

    private Teacher _testTeacher;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestTeachersDb")
            .Options;

        _dbContext = new ApplicationDbContext(options);
        _repository = new BaseRepository(_dbContext);
        _azureBlobService = new Mock<IAzureBlobService>().Object;
        _teachersService = new AdminTeachersService(_repository, _azureBlobService);

        _testTeacher = new Teacher
        {
            GuidId = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe",
            ImageUrl = "http://test.com/john.jpg",
            JobTitle = "Math Teacher",
            ApplicationUserId = null
        };

        _dbContext.Teachers.Add(_testTeacher);
        _dbContext.SaveChanges();
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    //----------------GetAllTeachersAsync---------------------
    [Test]
    public async Task GetAllTeachersAsync_ShouldReturnAllTeachers()
    {
        // Act
        var result = await _teachersService.GetAllTeachersAsync();

        // Assert
        Assert.That(result.Count(), Is.EqualTo(1));
        var teacher = result.First();
        Assert.Multiple(() =>
        {
            Assert.That(teacher.GuidId, Is.EqualTo(_testTeacher.GuidId.ToString()));
            Assert.That(teacher.FirstName, Is.EqualTo(_testTeacher.FirstName));
            Assert.That(teacher.LastName, Is.EqualTo(_testTeacher.LastName));
            Assert.That(teacher.ImageUrl, Is.EqualTo(_testTeacher.ImageUrl));
            Assert.That(teacher.JobTitle, Is.EqualTo(_testTeacher.JobTitle));
        });
    }

    [Test]
    public async Task GetAllTeachersAsync_ShouldReturnEmptyCollection_WhenNoTeachers()
    {
        // Arrange
        _dbContext.Teachers.RemoveRange(_dbContext.Teachers);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _teachersService.GetAllTeachersAsync();

        // Assert
        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task GetAllTeachersAsync_ShouldReturnTeachersSortedByFirstNameThenLastName()
    {
        // Arrange
        var additionalTeachers = new List<Teacher>
        {
            new Teacher
            {
                GuidId = Guid.NewGuid(),
                FirstName = "Alice",
                LastName = "Smith",
                ImageUrl = "http://test.com/alice.jpg",
                JobTitle = "English Teacher",
                ApplicationUserId = null
            },
            new Teacher
            {
                GuidId = Guid.NewGuid(),
                FirstName = "Alice",
                LastName = "Brown",
                ImageUrl = "http://test.com/alice2.jpg",
                JobTitle = "Science Teacher",
                ApplicationUserId = null
            }
        };

        await _dbContext.Teachers.AddRangeAsync(additionalTeachers);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = (await _teachersService.GetAllTeachersAsync()).ToList();

        // Assert
        Assert.That(result, Has.Count.EqualTo(3));
        Assert.Multiple(() =>
        {
            Assert.That(result[0].FirstName, Is.EqualTo("Alice"));
            Assert.That(result[0].LastName, Is.EqualTo("Brown"));
            Assert.That(result[1].FirstName, Is.EqualTo("Alice"));
            Assert.That(result[1].LastName, Is.EqualTo("Smith"));
            Assert.That(result[2].FirstName, Is.EqualTo("John"));
            Assert.That(result[2].LastName, Is.EqualTo("Doe"));
        });
    }
    //----------------GetAllTeachersAsync---------------------
    //----------------DeleteTeacherAsync---------------------
    [Test]
    public async Task DeleteTeacherAsync_WithInvalidGuid_ShouldReturnFalse()
    {
        // Arrange
        string invalidId = "not-a-guid";

        // Act
        bool result = await _teachersService.DeleteTeacherAsync(invalidId);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public async Task DeleteTeacherAsync_WithNonExistentTeacher_ShouldReturnFalse()
    {
        // Arrange
        string nonExistentId = Guid.NewGuid().ToString();

        // Act
        bool result = await _teachersService.DeleteTeacherAsync(nonExistentId);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public async Task DeleteTeacherAsync_WithValidTeacher_ShouldDeleteTeacherAndRelatedData()
    {
        // Arrange
        var subject = new Subject { Id = 1, Name = "Math" };
        await _dbContext.Subjects.AddAsync(subject);

        var teacher = new Teacher
        {
            GuidId = Guid.NewGuid(),
            FirstName = "Delete",
            LastName = "Test",
            ImageUrl = "test-image.jpg",
            JobTitle = "Teacher"
        };
        await _dbContext.Teachers.AddAsync(teacher);

        var subjectTeacher = new SubjectTeacher
        {
            TeacherId = teacher.GuidId,
            SubjectId = subject.Id
        };
        await _dbContext.SubjectsTeachers.AddAsync(subjectTeacher);
        await _dbContext.SaveChangesAsync();

        var mockBlobService = new Mock<IAzureBlobService>();
        mockBlobService.Setup(x => x.DeleteTeacherImageAsync(It.IsAny<string>()))
                       .ReturnsAsync(true);

        _teachersService = new AdminTeachersService(_repository, mockBlobService.Object);

        // Act
        bool result = await _teachersService.DeleteTeacherAsync(teacher.GuidId.ToString());

        // Assert
        Assert.That(result, Is.True);

        var deletedTeacher = await _dbContext.Teachers
            .FirstOrDefaultAsync(t => t.GuidId == teacher.GuidId);
        Assert.That(deletedTeacher, Is.Null);

        var relatedSubjectTeachers = await _dbContext.SubjectsTeachers
            .Where(st => st.TeacherId == teacher.GuidId)
            .ToListAsync();
        Assert.That(relatedSubjectTeachers, Is.Empty);

        mockBlobService.Verify(x => x.DeleteTeacherImageAsync(teacher.ImageUrl), Times.Once);
    }

    [Test]
    public async Task DeleteTeacherAsync_WhenImageDeleteFails_ShouldReturnFalse()
    {
        // Arrange
        var teacher = new Teacher
        {
            GuidId = Guid.NewGuid(),
            FirstName = "Delete",
            LastName = "Test",
            ImageUrl = "test-image.jpg",
            JobTitle = "Teacher"
        };
        await _dbContext.Teachers.AddAsync(teacher);
        await _dbContext.SaveChangesAsync();

        var mockBlobService = new Mock<IAzureBlobService>();
        mockBlobService.Setup(x => x.DeleteTeacherImageAsync(It.IsAny<string>()))
                       .ReturnsAsync(false);

        _teachersService = new AdminTeachersService(_repository, mockBlobService.Object);

        // Act
        bool result = await _teachersService.DeleteTeacherAsync(teacher.GuidId.ToString());

        // Assert
        Assert.That(result, Is.False);

        var teacherStillExists = await _dbContext.Teachers
            .AnyAsync(t => t.GuidId == teacher.GuidId);
        Assert.That(teacherStillExists, Is.True);
    }

    [Test]
    public async Task DeleteTeacherAsync_WithoutImage_ShouldNotCallBlobService()
    {
        // Arrange
        var teacher = new Teacher
        {
            GuidId = Guid.NewGuid(),
            FirstName = "Delete",
            LastName = "Test",
            ImageUrl = string.Empty,
            JobTitle = "Teacher"
        };
        await _dbContext.Teachers.AddAsync(teacher);
        await _dbContext.SaveChangesAsync();

        var mockBlobService = new Mock<IAzureBlobService>();
        _teachersService = new AdminTeachersService(_repository, mockBlobService.Object);

        // Act
        bool result = await _teachersService.DeleteTeacherAsync(teacher.GuidId.ToString());

        // Assert
        Assert.That(result, Is.True);
        mockBlobService.Verify(x => x.DeleteTeacherImageAsync(It.IsAny<string>()), Times.Never);
    }
    //----------------DeleteTeacherAsync---------------------
    //----------------GetAvailableSubjectsAsync---------------------
    [Test]
    public async Task GetAvailableSubjectsAsync_ShouldReturnAllSubjects()
    {
        // Arrange
        List<Subject> subjects = new List<Subject>
        {
            new Subject { Id = 1, Name = "Math" },
            new Subject { Id = 2, Name = "Physics" },
            new Subject { Id = 3, Name = "Chemistry" }
        };
        await _dbContext.Subjects.AddRangeAsync(subjects);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _teachersService.GetAvailableSubjectsAsync();

        // Assert
        Assert.That(result.Count(), Is.EqualTo(3));
    }

    [Test]
    public async Task GetAvailableSubjectsAsync_ShouldReturnEmptyCollection_WhenNoSubjects()
    {
        // Arrange
        _dbContext.Subjects.RemoveRange(_dbContext.Subjects);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _teachersService.GetAvailableSubjectsAsync();

        // Assert
        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task GetAvailableSubjectsAsync_ShouldMapPropertiesCorrectly()
    {
        // Arrange
        var subject = new Subject { Id = 1, Name = "Mathematics" };
        await _dbContext.Subjects.AddAsync(subject);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = (await _teachersService.GetAvailableSubjectsAsync()).First();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(subject.Id));
            Assert.That(result.Name, Is.EqualTo(subject.Name));
        });
    }

    [Test]
    public async Task GetAvailableSubjectsAsync_ShouldNotIncludeDeletedSubjects()
    {
        // Arrange
        List<Subject> subjects = new List<Subject>
        {
            new Subject { Id = 1, Name = "Math" },
            new Subject { Id = 2, Name = "Physics" }
        };
        await _dbContext.Subjects.AddRangeAsync(subjects);
        await _dbContext.SaveChangesAsync();

        _dbContext.Subjects.Remove(subjects[0]);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _teachersService.GetAvailableSubjectsAsync();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().Name, Is.EqualTo("Physics"));
        });
    }
    //----------------GetAvailableSubjectsAsync---------------------
    //----------------AddTeacherAsync---------------------
    [Test]
    public async Task AddTeacherAsync_WhenImageTooLarge_ShouldReturnError()
    {
        // Arrange
        var formFile = new Mock<IFormFile>();
        formFile.Setup(f => f.Length).Returns(3 * 1024 * 1024); // 3MB

        var model = new AddTeacherFormModel
        {
            FirstName = "Test",
            LastName = "Teacher",
            JobTitle = "Teacher",
            Image = formFile.Object,
            SelectedSubjects = new List<int> { 1 }
        };

        // Act
        var (isSuccessful, errorMessage) = await _teachersService.AddTeacherAsync(model);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(isSuccessful, Is.False);
            Assert.That(errorMessage, Is.EqualTo(ImageSizeErrorMessage));
        });
    }

    [Test]
    public async Task AddTeacherAsync_WhenInvalidImageFormat_ShouldReturnError()
    {
        // Arrange
        var formFile = new Mock<IFormFile>();
        formFile.Setup(f => f.Length).Returns(1 * 1024 * 1024); // 1MB
        formFile.Setup(f => f.FileName).Returns("test.gif");

        var model = new AddTeacherFormModel
        {
            FirstName = "Test",
            LastName = "Teacher",
            JobTitle = "Teacher",
            Image = formFile.Object,
            SelectedSubjects = new List<int> { 1 }
        };

        // Act
        var result = await _teachersService.AddTeacherAsync(model);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.isSuccessful, Is.False);
            Assert.That(result.errorMessage, Is.EqualTo(AllowedFormatsMessage));
        });
    }

    [Test]
    public async Task AddTeacherAsync_WhenImageUploadFails_ShouldReturnError()
    {
        // Arrange
        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(f => f.Length).Returns(1 * 1024 * 1024);
        mockFile.Setup(f => f.FileName).Returns("test.jpg");

        var model = new AddTeacherFormModel
        {
            FirstName = "Test",
            LastName = "Teacher",
            JobTitle = "Teacher",
            Image = mockFile.Object,
            SelectedSubjects = new List<int> { 1 }
        };

        var mockBlobService = new Mock<IAzureBlobService>();
        mockBlobService.Setup(x => x.UploadTeacherImageAsync(
                It.IsAny<IFormFile>(),
                It.IsAny<string>(),
                It.IsAny<string>()))
            .ReturnsAsync((true, "Upload failed", null));

        _teachersService = new AdminTeachersService(_repository, mockBlobService.Object);

        // Act
        var (isSuccessful, errorMessage) = await _teachersService.AddTeacherAsync(model);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(isSuccessful, Is.False);
            Assert.That(errorMessage, Is.EqualTo("Upload failed"));
        });
    }

    [Test]
    public async Task AddTeacherAsync_WhenValidData_ShouldCreateTeacherWithSubjects()
    {
        // Arrange
        var formFile = new Mock<IFormFile>();
        formFile.Setup(f => f.Length).Returns(1 * 1024 * 1024);
        formFile.Setup(f => f.FileName).Returns("test.jpg");

        List<Subject> subjects = new List<Subject>
        {
            new Subject { Id = 1, Name = "Math" },
            new Subject { Id = 2, Name = "Physics" }
        };
        await _dbContext.Subjects.AddRangeAsync(subjects);
        await _dbContext.SaveChangesAsync();

        var model = new AddTeacherFormModel
        {
            FirstName = "Test",
            LastName = "Teacher",
            JobTitle = "Math Teacher",
            Image = formFile.Object,
            SelectedSubjects = new List<int> { 1, 2 }
        };

        var mockBlobService = new Mock<IAzureBlobService>();
        mockBlobService.Setup(x => x.UploadTeacherImageAsync(
                It.IsAny<IFormFile>(),
                It.IsAny<string>(),
                It.IsAny<string>()))
            .ReturnsAsync((true, null, "test-url.jpg"));

        _teachersService = new AdminTeachersService(_repository, mockBlobService.Object);

        // Act
        var result = await _teachersService.AddTeacherAsync(model);

        // Assert
        Assert.That(result.isSuccessful, Is.True);
        Assert.That(result.errorMessage, Is.Null);

        var addedTeacher = await _dbContext.Teachers
            .Include(t => t.SubjectTeachers)
            .FirstOrDefaultAsync(t => t.FirstName == model.FirstName && t.LastName == model.LastName);

        Assert.That(addedTeacher, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(addedTeacher!.FirstName, Is.EqualTo(model.FirstName));
            Assert.That(addedTeacher.LastName, Is.EqualTo(model.LastName));
            Assert.That(addedTeacher.JobTitle, Is.EqualTo(model.JobTitle));
            Assert.That(addedTeacher.ImageUrl, Is.EqualTo("test-url.jpg"));
            Assert.That(addedTeacher.SubjectTeachers.Count, Is.EqualTo(2));
        });
    }

    [Test]
    public async Task AddTeacherAsync_WithNullModel_ShouldReturnFalse()
    {
        // Act
        var result = await _teachersService.AddTeacherAsync(null!);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.isSuccessful, Is.False);
            Assert.That(result.errorMessage, Is.EqualTo(TeacherNotFoundMessage));
        });
    }
    //----------------AddTeacherAsync---------------------
    //----------------GetTeacherForEditAsync---------------------
    [Test]
    public async Task GetTeacherForEditAsync_WithInvalidId_ShouldReturnNull()
    {
        // Arrange
        string invalidId = "not-a-guid";

        // Act
        var result = await _teachersService.GetTeacherForEditAsync(invalidId);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetTeacherForEditAsync_WithNonExistentTeacher_ShouldReturnNull()
    {
        // Arrange
        string nonExistentId = Guid.NewGuid().ToString();

        // Act
        var result = await _teachersService.GetTeacherForEditAsync(nonExistentId);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetTeacherForEditAsync_ShouldReturnCorrectModel()
    {
        // Arrange
        var subject1 = new Subject { Id = 1, Name = "Math" };
        var subject2 = new Subject { Id = 2, Name = "Physics" };
        await _dbContext.Subjects.AddRangeAsync(subject1, subject2);

        var teacher = new Teacher
        {
            GuidId = Guid.NewGuid(),
            FirstName = "Edit",
            LastName = "Test",
            ImageUrl = "http://test.com/images/test.jpg",
            JobTitle = "Teacher"
        };
        await _dbContext.Teachers.AddAsync(teacher);

        var subjectTeacher = new SubjectTeacher
        {
            TeacherId = teacher.GuidId,
            SubjectId = subject1.Id
        };
        await _dbContext.SubjectsTeachers.AddAsync(subjectTeacher);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _teachersService.GetTeacherForEditAsync(teacher.GuidId.ToString());

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result!.Id, Is.EqualTo(teacher.GuidId.ToString()));
            Assert.That(result.FirstName, Is.EqualTo(teacher.FirstName));
            Assert.That(result.LastName, Is.EqualTo(teacher.LastName));
            Assert.That(result.JobTitle, Is.EqualTo(teacher.JobTitle));
            Assert.That(result.CurrentImageUrl, Is.EqualTo(teacher.ImageUrl));
            Assert.That(result.CurrentImageFileName, Is.EqualTo("test.jpg"));
            Assert.That(result.SelectedSubjects.Count(), Is.EqualTo(1));
            Assert.That(result.SelectedSubjects.First(), Is.EqualTo(subject1.Id));
            Assert.That(result.AvailableSubjects.Count(), Is.EqualTo(2));
        });
    }
    //----------------GetTeacherForEditAsync---------------------
    //----------------EditTeacherAsync---------------------
    [Test]
    public async Task EditTeacherAsync_WithInvalidId_ShouldReturnError()
    {
        // Arrange
        var model = new EditTeacherFormModel
        {
            Id = "not-a-guid",
            FirstName = "Test",
            LastName = "Teacher",
            JobTitle = "Teacher"
        };

        // Act
        var (isSuccessful, errorMessage) = await _teachersService.EditTeacherAsync(model);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(isSuccessful, Is.False);
            Assert.That(errorMessage, Is.EqualTo(InvalidTeacherIdMessage));
        });
    }

    [Test]
    public async Task EditTeacherAsync_WithNonExistentTeacher_ShouldReturnError()
    {
        // Arrange
        var model = new EditTeacherFormModel
        {
            Id = Guid.NewGuid().ToString(),
            FirstName = "Test",
            LastName = "Teacher",
            JobTitle = "Teacher"
        };

        // Act
        var (isSuccessful, errorMessage) = await _teachersService.EditTeacherAsync(model);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(isSuccessful, Is.False);
            Assert.That(errorMessage, Is.EqualTo(TeacherNotFoundMessage));
        });
    }

    [Test]
    public async Task EditTeacherAsync_WhenImageTooLarge_ShouldReturnError()
    {
        // Arrange
        var teacher = await SetupTestTeacher();

        var formFile = new Mock<IFormFile>();
        formFile.Setup(f => f.Length).Returns(3 * 1024 * 1024); // 3MB
        formFile.Setup(f => f.FileName).Returns("test.jpg");

        var model = new EditTeacherFormModel
        {
            Id = teacher.GuidId.ToString(),
            FirstName = "Updated",
            LastName = "Teacher",
            JobTitle = "Updated Job",
            Image = formFile.Object
        };

        // Act
        var result = await _teachersService.EditTeacherAsync(model);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.isSuccessful, Is.False);
            Assert.That(result.errorMessage, Is.EqualTo(ImageSizeErrorMessage));
        });
    }

    [Test]
    public async Task EditTeacherAsync_WhenInvalidImageFormat_ShouldReturnError()
    {
        // Arrange
        var teacher = await SetupTestTeacher();

        var formFile = new Mock<IFormFile>();
        formFile.Setup(f => f.Length).Returns(1 * 1024 * 1024);
        formFile.Setup(f => f.FileName).Returns("test.gif");

        var model = new EditTeacherFormModel
        {
            Id = teacher.GuidId.ToString(),
            FirstName = "Updated",
            LastName = "Teacher",
            JobTitle = "Updated Job",
            Image = formFile.Object
        };

        // Act
        var (isSuccessful, errorMessage) = await _teachersService.EditTeacherAsync(model);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(isSuccessful, Is.False);
            Assert.That(errorMessage, Is.EqualTo(AllowedFormatsMessage));
        });
    }

    [Test]
    public async Task EditTeacherAsync_WhenDeleteOldImageFails_ShouldReturnError()
    {
        // Arrange
        var teacher = await SetupTestTeacher();

        var formFile = new Mock<IFormFile>();
        formFile.Setup(f => f.Length).Returns(1 * 1024 * 1024);
        formFile.Setup(f => f.FileName).Returns("test.jpg");

        var mockBlobService = new Mock<IAzureBlobService>();
        mockBlobService.Setup(x => x.DeleteTeacherImageAsync(It.IsAny<string>()))
                       .ReturnsAsync(false);

        var model = new EditTeacherFormModel
        {
            Id = teacher.GuidId.ToString(),
            FirstName = "Updated",
            LastName = "Teacher",
            JobTitle = "Updated Job",
            Image = formFile.Object
        };

        // Act
        var (isSuccessful, errorMessage) = await _teachersService.EditTeacherAsync(model);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(isSuccessful, Is.False);
            Assert.That(errorMessage, Is.EqualTo(DeleteOldImageErrorMessage));
        });
    }

    [Test]
    public async Task EditTeacherAsync_WithValidDataAndNoImage_ShouldUpdateTeacher()
    {
        // Arrange
        var subjects = new List<Subject>
        {
            new Subject { Id = 1, Name = "Math" },
            new Subject { Id = 2, Name = "Physics" }
        };
        await _dbContext.Subjects.AddRangeAsync(subjects);

        var teacher = new Teacher
        {
            GuidId = Guid.NewGuid(),
            FirstName = "Original",
            LastName = "Teacher",
            ImageUrl = "existing-image.jpg",
            JobTitle = "Original Job"
        };
        await _dbContext.Teachers.AddAsync(teacher);

        var subjectTeacher = new SubjectTeacher
        {
            TeacherId = teacher.GuidId,
            SubjectId = 1
        };
        await _dbContext.SubjectsTeachers.AddAsync(subjectTeacher);
        await _dbContext.SaveChangesAsync();

        var model = new EditTeacherFormModel
        {
            Id = teacher.GuidId.ToString(),
            FirstName = "Updated",
            LastName = "Name",
            JobTitle = "New Job",
            SelectedSubjects = new List<int> { 1, 2 }
        };

        // Act
        var result = await _teachersService.EditTeacherAsync(model);

        // Assert
        Assert.That(result.isSuccessful, Is.True);

        var updatedTeacher = await _dbContext.Teachers
            .Include(t => t.SubjectTeachers)
            .FirstOrDefaultAsync(t => t.GuidId == teacher.GuidId);

        Assert.That(updatedTeacher, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(updatedTeacher!.FirstName, Is.EqualTo("Updated"));
            Assert.That(updatedTeacher.LastName, Is.EqualTo("Name"));
            Assert.That(updatedTeacher.JobTitle, Is.EqualTo("New Job"));
            Assert.That(updatedTeacher.ImageUrl, Is.EqualTo("existing-image.jpg"));
            Assert.That(updatedTeacher.SubjectTeachers, Has.Count.EqualTo(2));
        });
    }

    [Test]
    public async Task EditTeacherAsync_WithNewImage_ShouldUpdateTeacherAndImage()
    {
        // Arrange
        var teacher = await SetupTestTeacher();

        var formFile = new Mock<IFormFile>();
        formFile.Setup(f => f.Length).Returns(1 * 1024 * 1024);
        formFile.Setup(f => f.FileName).Returns("test.jpg");

        var mockBlobService = new Mock<IAzureBlobService>();
        mockBlobService.Setup(x => x.DeleteTeacherImageAsync(It.IsAny<string>()))
                       .ReturnsAsync(true);
        mockBlobService.Setup(x => x.UploadTeacherImageAsync(
                It.IsAny<IFormFile>(),
                It.IsAny<string>(),
                It.IsAny<string>()))
            .ReturnsAsync((true, null, "new-image-url.jpg"));

        _teachersService = new AdminTeachersService(_repository, mockBlobService.Object);

        var model = new EditTeacherFormModel
        {
            Id = teacher.GuidId.ToString(),
            FirstName = "Updated",
            LastName = "Teacher",
            JobTitle = "Updated Job",
            Image = formFile.Object,
            SelectedSubjects = new List<int> { 1 }
        };

        // Act
        var (isSuccessful, errorMessage) = await _teachersService.EditTeacherAsync(model);

        // Assert
        Assert.That(isSuccessful, Is.True);

        var updatedTeacher = await _dbContext.Teachers.FindAsync(teacher.GuidId);
        Assert.That(updatedTeacher, Is.Not.Null);
        Assert.That(updatedTeacher!.ImageUrl, Is.EqualTo("new-image-url.jpg"));
    }

    private async Task<Teacher> SetupTestTeacher()
    {
        var subject = new Subject { Id = 1, Name = "Math" };
        await _dbContext.Subjects.AddAsync(subject);

        var teacher = new Teacher
        {
            GuidId = Guid.NewGuid(),
            FirstName = "Test",
            LastName = "Teacher",
            ImageUrl = "old-image.jpg",
            JobTitle = "Teacher"
        };
        await _dbContext.Teachers.AddAsync(teacher);

        var subjectTeacher = new SubjectTeacher
        {
            TeacherId = teacher.GuidId,
            SubjectId = subject.Id
        };
        await _dbContext.SubjectsTeachers.AddAsync(subjectTeacher);
        await _dbContext.SaveChangesAsync();

        return teacher;
    }
    //----------------EditTeacherAsync---------------------
}