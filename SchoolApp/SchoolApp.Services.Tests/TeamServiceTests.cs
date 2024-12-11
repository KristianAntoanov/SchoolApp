using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.Data.Models;
using SchoolApp.Data.Repository;
using SchoolApp.Data.Repository.Contracts;
using SchoolApp.Services.Data;

namespace SchoolApp.Services.Tests;

[TestFixture]
public class TeamServiceTests
{
    private ApplicationDbContext _dbContext;
    private IRepository _repository;
    private TeamService _teamService;

    private Teacher _testTeacher;
    private Subject _testSubject;
    private SubjectTeacher _testSubjectTeacher;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestTeamDb")
            .Options;

        _dbContext = new ApplicationDbContext(options);
        _repository = new BaseRepository(_dbContext);
        _teamService = new TeamService(_repository);

        // Setup test data
        _testSubject = new Subject
        {
            Id = 1,
            Name = "Mathematics"
        };

        _testTeacher = new Teacher
        {
            GuidId = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe",
            ImageUrl = "test-image-url",
            JobTitle = "Mathematics Teacher",
            ApplicationUserId = Guid.NewGuid()
        };

        _testSubjectTeacher = new SubjectTeacher
        {
            SubjectId = _testSubject.Id,
            Subject = _testSubject,
            TeacherId = _testTeacher.GuidId,
            Teacher = _testTeacher
        };

        _dbContext.Subjects.Add(_testSubject);
        _dbContext.Teachers.Add(_testTeacher);
        _dbContext.SubjectsTeachers.Add(_testSubjectTeacher);
        _dbContext.SaveChanges();
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    [Test]
    public async Task GetAllTeachers_ShouldReturnAllTeachers_WhenTeachersExist()
    {
        // Act
        var result = await _teamService.GetAllTeachers();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(1));

        var teacher = result.First();
        Assert.Multiple(() =>
        {
            Assert.That(teacher.FirstName, Is.EqualTo(_testTeacher.FirstName));
            Assert.That(teacher.LastName, Is.EqualTo(_testTeacher.LastName));
            Assert.That(teacher.JobTitle, Is.EqualTo(_testTeacher.JobTitle));
            Assert.That(teacher.Photo, Is.EqualTo(_testTeacher.ImageUrl));
            Assert.That(teacher.Subjects.Count(), Is.EqualTo(1));
        });

        var subject = teacher.Subjects.First();
        Assert.Multiple(() =>
        {
            Assert.That(subject.Id, Is.EqualTo(_testSubject.Id));
            Assert.That(subject.Name, Is.EqualTo(_testSubject.Name));
        });
    }

    [Test]
    public async Task GetAllTeachers_ShouldReturnEmptyCollection_WhenNoTeachersExist()
    {
        // Arrange
        _dbContext.SubjectsTeachers.RemoveRange(_dbContext.SubjectsTeachers);
        _dbContext.Teachers.RemoveRange(_dbContext.Teachers);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _teamService.GetAllTeachers();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task GetAllTeachers_ShouldReturnTeachersWithMultipleSubjects()
    {
        // Arrange
        var secondSubject = new Subject
        {
            Id = 2,
            Name = "Physics"
        };

        var secondSubjectTeacher = new SubjectTeacher
        {
            SubjectId = secondSubject.Id,
            Subject = secondSubject,
            TeacherId = _testTeacher.GuidId,
            Teacher = _testTeacher
        };

        await _dbContext.Subjects.AddAsync(secondSubject);
        await _dbContext.SubjectsTeachers.AddAsync(secondSubjectTeacher);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _teamService.GetAllTeachers();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(1));

        var teacher = result.First();
        Assert.That(teacher.Subjects.Count(), Is.EqualTo(2));

        var subjects = teacher.Subjects.ToList();
        Assert.Multiple(() =>
        {
            Assert.That(subjects.Any(s => s.Name == "Mathematics"), Is.True);
            Assert.That(subjects.Any(s => s.Name == "Physics"), Is.True);
        });
    }

    [Test]
    public async Task GetAllTeachers_ShouldReturnTeacherWithoutSubjects_WhenTeacherHasNoSubjects()
    {
        // Arrange
        var teacherWithoutSubjects = new Teacher
        {
            GuidId = Guid.NewGuid(),
            FirstName = "Jane",
            LastName = "Smith",
            ImageUrl = "another-image-url",
            JobTitle = "New Teacher",
            ApplicationUserId = Guid.NewGuid()
        };

        await _dbContext.Teachers.AddAsync(teacherWithoutSubjects);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _teamService.GetAllTeachers();

        // Assert
        Assert.That(result.Count(), Is.EqualTo(2));

        var teacherWithNoSubjects = result.First(t => t.FirstName == "Jane");
        Assert.Multiple(() =>
        {
            Assert.That(teacherWithNoSubjects.FirstName, Is.EqualTo(teacherWithoutSubjects.FirstName));
            Assert.That(teacherWithNoSubjects.LastName, Is.EqualTo(teacherWithoutSubjects.LastName));
            Assert.That(teacherWithNoSubjects.JobTitle, Is.EqualTo(teacherWithoutSubjects.JobTitle));
            Assert.That(teacherWithNoSubjects.Photo, Is.EqualTo(teacherWithoutSubjects.ImageUrl));
            Assert.That(teacherWithNoSubjects.Subjects, Is.Empty);
        });
    }

    [Test]
    public async Task GetAllTeachers_ShouldReturnMultipleTeachers_WhenMultipleTeachersExist()
    {
        // Arrange
        var secondTeacher = new Teacher
        {
            GuidId = Guid.NewGuid(),
            FirstName = "Jane",
            LastName = "Smith",
            ImageUrl = "another-image-url",
            JobTitle = "Physics Teacher",
            ApplicationUserId = Guid.NewGuid()
        };

        var secondSubject = new Subject
        {
            Id = 2,
            Name = "Physics"
        };

        var secondSubjectTeacher = new SubjectTeacher
        {
            SubjectId = secondSubject.Id,
            Subject = secondSubject,
            TeacherId = secondTeacher.GuidId,
            Teacher = secondTeacher
        };

        await _dbContext.Teachers.AddAsync(secondTeacher);
        await _dbContext.Subjects.AddAsync(secondSubject);
        await _dbContext.SubjectsTeachers.AddAsync(secondSubjectTeacher);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _teamService.GetAllTeachers();

        // Assert
        Assert.That(result.Count(), Is.EqualTo(2));

        var firstTeacher = result.First(t => t.FirstName == "John");
        var secondTeacherResult = result.First(t => t.FirstName == "Jane");

        Assert.Multiple(() =>
        {
            Assert.That(firstTeacher.Subjects.Count(), Is.EqualTo(1));
            Assert.That(secondTeacherResult.Subjects.Count(), Is.EqualTo(1));
            Assert.That(firstTeacher.Subjects.First().Name, Is.EqualTo("Mathematics"));
            Assert.That(secondTeacherResult.Subjects.First().Name, Is.EqualTo("Physics"));
        });
    }
}