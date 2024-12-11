using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.Data.Models;
using SchoolApp.Data.Repository;
using SchoolApp.Data.Repository.Contracts;
using SchoolApp.Services.Data;
using SchoolApp.Web.ViewModels.Admin.Students;

namespace SchoolApp.Services.Tests;

[TestFixture]
public class AdminStudentsServiceTests
{
    private ApplicationDbContext _dbContext;
    private IRepository _repository;
    private AdminStudentsService _studentsService;

    private Student _testStudent;
    private Class _testClass;
    private Subject _testSubject;
    private Grade _testGrade;
    private Teacher _testTeacher;
    private Section _testSection;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestStudentsDb")
            .Options;

        _dbContext = new ApplicationDbContext(options);
        _repository = new BaseRepository(_dbContext);
        _studentsService = new AdminStudentsService(_repository);

        // Setup test data
        _testSection = new Section
        {
            Id = 1,
            Name = "A"
        };

        _testClass = new Class
        {
            Id = 1,
            GradeLevel = 10,
            Section = _testSection,
            SectionId = _testSection.Id
        };

        _testTeacher = new Teacher
        {
            GuidId = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe",
            ImageUrl = "TestUrl",
            JobTitle = "TestJobTitle",
            ApplicationUserId = Guid.NewGuid()
        };

        _testSubject = new Subject
        {
            Id = 1,
            Name = "Mathematics"
        };

        _testStudent = new Student
        {
            Id = 1,
            FirstName = "Test",
            MiddleName = "Middle",
            LastName = "Student",
            ClassId = _testClass.Id,
            Class = _testClass,
            ApplicationUserId = Guid.NewGuid()
        };

        _testGrade = new Grade
        {
            Id = 1,
            GradeValue = 5,
            AddedOn = DateTime.UtcNow,
            StudentId = _testStudent.Id,
            TeacherId = _testTeacher.GuidId,
            Teacher = _testTeacher,
            SubjectId = _testSubject.Id,
            Subject = _testSubject,
            GradeType = GradeType.Current
        };

        var subjectStudent = new SubjectStudent
        {
            StudentId = _testStudent.Id,
            Student = _testStudent,
            SubjectId = _testSubject.Id,
            Subject = _testSubject
        };

        _dbContext.Sections.Add(_testSection);
        _dbContext.Classes.Add(_testClass);
        _dbContext.Teachers.Add(_testTeacher);
        _dbContext.Subjects.Add(_testSubject);
        _dbContext.Students.Add(_testStudent);
        _dbContext.Grades.Add(_testGrade);
        _dbContext.SubjectsStudents.Add(subjectStudent);
        _dbContext.SaveChanges();
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    //----------------GetAllStudentsAsync---------------------
    [Test]
    public async Task GetAllStudentsAsync_ShouldReturnAllStudents_WhenNoSearchTerm()
    {
        // Act
        var result = await _studentsService.GetAllStudentsAsync(1, 10);

        // Assert
        Assert.That(result.Items.Count, Is.EqualTo(1));
        var student = result.Items.First();
        Assert.Multiple(() =>
        {
            Assert.That(student.FirstName, Is.EqualTo(_testStudent.FirstName));
            Assert.That(student.MiddleName, Is.EqualTo(_testStudent.MiddleName));
            Assert.That(student.LastName, Is.EqualTo(_testStudent.LastName));
            Assert.That(student.Grades.Count(), Is.EqualTo(1));
        });
    }

    [Test]
    public async Task GetAllStudentsAsync_ShouldReturnFilteredStudents_WhenSearchTermProvided()
    {
        // Arrange
        var searchTerm = "Test";

        // Act
        var result = await _studentsService.GetAllStudentsAsync(1, 10, searchTerm);

        // Assert
        Assert.That(result.Items.Count, Is.EqualTo(1));

        searchTerm = "NonExistent";
        result = await _studentsService.GetAllStudentsAsync(1, 10, searchTerm);
        Assert.That(result.Items.Count, Is.EqualTo(0));
    }

    [Test]
    public async Task GetAllStudentsAsync_ShouldHandlePagination_Correctly()
    {
        // Arrange
        for (int i = 0; i < 15; i++)
        {
            var student = new Student
            {
                FirstName = $"Student{i}",
                MiddleName = "TestMiddleName",
                LastName = "TestLastName",
                ClassId = _testClass.Id,
                ApplicationUserId = Guid.NewGuid()
            };
            await _dbContext.Students.AddAsync(student);
        }
        await _dbContext.SaveChangesAsync();

        // Act
        var firstPage = await _studentsService.GetAllStudentsAsync(1, 10);
        var secondPage = await _studentsService.GetAllStudentsAsync(2, 10);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(firstPage.Items.Count, Is.EqualTo(10));
            Assert.That(secondPage.Items.Count, Is.EqualTo(6));
            Assert.That(firstPage.TotalPages, Is.EqualTo(2));
            Assert.That(firstPage.TotalItems, Is.EqualTo(16));
        });
    }

    [Test]
    public async Task GetAllStudentsAsync_ShouldHandleSearchByFullName_Correctly()
    {
        // Arrange
        var searchTerm = "Test Middle Student";

        // Act
        var result = await _studentsService.GetAllStudentsAsync(1, 10, searchTerm);

        // Assert
        Assert.That(result.Items.Count, Is.EqualTo(1));
        var student = result.Items.First();
        Assert.Multiple(() =>
        {
            Assert.That(student.FirstName, Is.EqualTo(_testStudent.FirstName));
            Assert.That(student.MiddleName, Is.EqualTo(_testStudent.MiddleName));
            Assert.That(student.LastName, Is.EqualTo(_testStudent.LastName));
        });
    }
    //----------------GetAllStudentsAsync---------------------
    //----------------DeleteStudent---------------------
    [Test]
    public async Task DeleteStudent_WhenStudentExists_AndDeleteAllRelatedData_ShouldReturnTrue()
    {
        // Arrange
        int studentId = _testStudent.Id;

        // Act
        bool result = await _studentsService.DeleteStudent(studentId);

        // Assert
        Assert.That(result, Is.True);

        var deletedStudent = await _dbContext.Students
            .FirstOrDefaultAsync(s => s.Id == studentId);
        Assert.That(deletedStudent, Is.Null);

        var relatedGrades = await _dbContext.Grades
            .Where(g => g.StudentId == studentId)
            .ToListAsync();
        Assert.That(relatedGrades, Is.Empty);

        var subjectStudents = await _dbContext.SubjectsStudents
            .Where(ss => ss.StudentId == studentId)
            .ToListAsync();
        Assert.That(subjectStudents, Is.Empty);
    }

    [Test]
    public async Task DeleteStudent_WithNonExistentStudent_ShouldReturnFalse()
    {
        // Arrange
        int nonExistentId = 999;

        // Act
        bool result = await _studentsService.DeleteStudent(nonExistentId);

        // Assert
        Assert.That(result, Is.False);

        Assert.Multiple(async () =>
        {
            Assert.That(await _dbContext.Students.CountAsync(), Is.EqualTo(1));
            Assert.That(await _dbContext.Grades.CountAsync(), Is.EqualTo(1));
            Assert.That(await _dbContext.SubjectsStudents.CountAsync(), Is.EqualTo(1));
        });
    }
    //----------------DeleteStudent---------------------
    //----------------GetStudentForEditAsync---------------------
    [Test]
    public async Task GetStudentForEditAsync_WhenStudentDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        int nonExistentId = 999;

        // Act
        var result = await _studentsService.GetStudentForEditAsync(nonExistentId);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetStudentForEditAsync_WhenStudentExists_ShouldReturnCorrectModel()
    {
        // Arrange
        int studentId = _testStudent.Id;

        // Act
        var result = await _studentsService.GetStudentForEditAsync(studentId);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result!.Id, Is.EqualTo(_testStudent.Id));
            Assert.That(result.FirstName, Is.EqualTo(_testStudent.FirstName));
            Assert.That(result.MiddleName, Is.EqualTo(_testStudent.MiddleName));
            Assert.That(result.LastName, Is.EqualTo(_testStudent.LastName));
            Assert.That(result.ClassId, Is.EqualTo(_testStudent.ClassId));
        });
    }

    [Test]
    public async Task GetStudentForEditAsync_ShouldIncludeAvailableClasses()
    {
        // Arrange
        int studentId = _testStudent.Id;

        var additionalClass = new Class
        {
            Id = 2,
            GradeLevel = 11,
            Section = _testSection,
            SectionId = _testSection.Id
        };
        await _dbContext.Classes.AddAsync(additionalClass);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _studentsService.GetStudentForEditAsync(studentId);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.AvailableClasses, Is.Not.Null);
        Assert.That(result.AvailableClasses.Count, Is.EqualTo(2));

        var firstClass = result.AvailableClasses.First();
        Assert.Multiple(() =>
        {
            Assert.That(firstClass.Id, Is.EqualTo(_testClass.Id));
            Assert.That(firstClass.Name, Is.EqualTo($"{_testClass.GradeLevel} {_testClass.Section.Name}"));
        });
    }
    //----------------GetStudentForEditAsync---------------------
    //----------------UpdateStudentAsync---------------------
    [Test]
    public async Task UpdateStudentAsync_WhenStudentDoesNotExist_ShouldReturnFalse()
    {
        // Arrange
        var model = new EditStudentFormModel
        {
            Id = 999,
            FirstName = "Updated",
            LastName = "Name",
            ClassId = _testClass.Id
        };

        // Act
        bool result = await _studentsService.UpdateStudentAsync(model);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public async Task UpdateStudentAsync_WhenModelIsNull_ShouldReturnFalse()
    {
        // Arrange
        var model = new EditStudentFormModel
        {
            Id = 0,
            FirstName = "",
            LastName = "",
            ClassId = 0
        };
        model = null;

        // Act
        bool result = await _studentsService.UpdateStudentAsync(model);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public async Task UpdateStudentAsync_WhenClassNotExists_ShouldReturnFalse()
    {
        // Arrange
        var model = new EditStudentFormModel
        {
            Id = 2,
            FirstName = "Updated",
            LastName = "Name",
            ClassId = 158
        };

        // Act
        bool result = await _studentsService.UpdateStudentAsync(model);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public async Task UpdateStudentAsync_ShouldUpdateAllFields_WhenStudentExists()
    {
        // Arrange
        var newClass = new Class
        {
            Id = 2,
            GradeLevel = 11,
            Section = _testSection,
            SectionId = _testSection.Id
        };
        await _dbContext.Classes.AddAsync(newClass);
        await _dbContext.SaveChangesAsync();

        var model = new EditStudentFormModel
        {
            Id = _testStudent.Id,
            FirstName = "UpdatedFirst",
            LastName = "UpdatedLast",
            ClassId = newClass.Id
        };

        // Act
        bool result = await _studentsService.UpdateStudentAsync(model);

        // Assert
        Assert.That(result, Is.True);

        var updatedStudent = await _dbContext.Students
            .FirstOrDefaultAsync(s => s.Id == _testStudent.Id);

        Assert.That(updatedStudent, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(updatedStudent!.FirstName, Is.EqualTo("UpdatedFirst"));
            Assert.That(updatedStudent.LastName, Is.EqualTo("UpdatedLast"));
            Assert.That(updatedStudent.ClassId, Is.EqualTo(newClass.Id));
        });
    }

    [Test]
    public async Task UpdateStudentAsync_ShouldPreserveUnchangedFields()
    {
        // Arrange
        var model = new EditStudentFormModel
        {
            Id = _testStudent.Id,
            FirstName = "UpdatedFirst",
            LastName = _testStudent.LastName,
            ClassId = _testStudent.ClassId
        };

        // Act
        bool result = await _studentsService.UpdateStudentAsync(model);

        // Assert
        Assert.That(result, Is.True);

        var updatedStudent = await _dbContext.Students
            .FirstOrDefaultAsync(s => s.Id == _testStudent.Id);

        Assert.That(updatedStudent, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(updatedStudent!.FirstName, Is.EqualTo("UpdatedFirst"));
            Assert.That(updatedStudent.LastName, Is.EqualTo(_testStudent.LastName));
            Assert.That(updatedStudent.ClassId, Is.EqualTo(_testStudent.ClassId));
            Assert.That(updatedStudent.MiddleName, Is.EqualTo(_testStudent.MiddleName));
        });
    }

    [Test]
    public async Task UpdateStudentAsync_ShouldMaintainRelationships_AfterUpdate()
    {
        // Arrange
        var model = new EditStudentFormModel
        {
            Id = _testStudent.Id,
            FirstName = "UpdatedFirst",
            LastName = "UpdatedLast",
            ClassId = _testStudent.ClassId
        };

        // Act
        bool result = await _studentsService.UpdateStudentAsync(model);

        // Assert
        Assert.That(result, Is.True);

        // Verify that related data remains intact
        var studentWithRelations = await _dbContext.Students
            .Include(s => s.Grades)
            .Include(s => s.SubjectStudents)
            .FirstOrDefaultAsync(s => s.Id == _testStudent.Id);

        Assert.That(studentWithRelations, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(studentWithRelations!.Grades.Count, Is.EqualTo(1));
            Assert.That(studentWithRelations.SubjectStudents.Count, Is.EqualTo(1));
        });
    }
    //----------------UpdateStudentAsync---------------------
    //----------------GetStudentGradesAsync---------------------
    [Test]
    public async Task GetStudentGradesAsync_ShouldReturnNull_WhenStudentDoesNotExist()
    {
        // Arrange
        int nonExistentId = 999;

        // Act
        var result = await _studentsService.GetStudentGradesAsync(nonExistentId);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetStudentGradesAsync_ShouldReturnCorrectData_WhenStudentExists()
    {
        // Act
        var result = await _studentsService.GetStudentGradesAsync(_testStudent.Id);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result!.StudentId, Is.EqualTo(_testStudent.Id));
            Assert.That(result.FirstName, Is.EqualTo(_testStudent.FirstName));
            Assert.That(result.LastName, Is.EqualTo(_testStudent.LastName));
            Assert.That(result.SubjectGrades.Count, Is.EqualTo(1));
        });

        var subjectGrade = result!.SubjectGrades.First();
        Assert.Multiple(() =>
        {
            Assert.That(subjectGrade.SubjectId, Is.EqualTo(_testSubject.Id));
            Assert.That(subjectGrade.SubjectName, Is.EqualTo(_testSubject.Name));
            Assert.That(subjectGrade.Grades, Has.Count.EqualTo(1));
        });

        var grade = subjectGrade.Grades.First();
        Assert.Multiple(() =>
        {
            Assert.That(grade.Id, Is.EqualTo(_testGrade.Id));
            Assert.That(grade.GradeValue, Is.EqualTo(_testGrade.GradeValue));
            Assert.That(grade.GradeType, Is.EqualTo(_testGrade.GradeType));
        });
    }

    [Test]
    public async Task GetStudentGradesAsync_ShouldReturnCorrectData_ForStudentWithMultipleGrades()
    {
        // Arrange
        var secondGrade = new Grade
        {
            Id = 2,
            GradeValue = 6,
            AddedOn = DateTime.UtcNow,
            StudentId = _testStudent.Id,
            TeacherId = _testTeacher.GuidId,
            Teacher = _testTeacher,
            SubjectId = _testSubject.Id,
            Subject = _testSubject,
            GradeType = GradeType.Current
        };

        var secondSubject = new Subject
        {
            Id = 2,
            Name = "Physics"
        };

        var secondSubjectGrade = new Grade
        {
            Id = 3,
            GradeValue = 4,
            AddedOn = DateTime.UtcNow,
            StudentId = _testStudent.Id,
            TeacherId = _testTeacher.GuidId,
            Teacher = _testTeacher,
            SubjectId = secondSubject.Id,
            Subject = secondSubject,
            GradeType = GradeType.Current
        };

        var secondSubjectStudent = new SubjectStudent
        {
            StudentId = _testStudent.Id,
            Student = _testStudent,
            SubjectId = secondSubject.Id,
            Subject = secondSubject
        };

        await _dbContext.Subjects.AddAsync(secondSubject);
        await _dbContext.Grades.AddRangeAsync(secondGrade, secondSubjectGrade);
        await _dbContext.SubjectsStudents.AddAsync(secondSubjectStudent);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _studentsService.GetStudentGradesAsync(_testStudent.Id);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.SubjectGrades.Count, Is.EqualTo(2));

        var mathGrades = result.SubjectGrades
            .First(sg => sg.SubjectName == "Mathematics")
            .Grades;
        Assert.That(mathGrades, Has.Count.EqualTo(2));

        var physicsGrades = result.SubjectGrades
            .First(sg => sg.SubjectName == "Physics")
            .Grades;
        Assert.That(physicsGrades, Has.Count.EqualTo(1));
    }

    [Test]
    public async Task GetStudentGradesAsync_ShouldReturnEmptyGradesList_ForStudentWithoutGrades()
    {
        // Arrange
        var studentWithoutGrades = new Student
        {
            Id = 2,
            FirstName = "No",
            MiddleName = "NoMiddle",
            LastName = "Grades",
            ClassId = _testClass.Id,
            ApplicationUserId = Guid.NewGuid()
        };

        var subjectStudent = new SubjectStudent
        {
            StudentId = studentWithoutGrades.Id,
            Student = studentWithoutGrades,
            SubjectId = _testSubject.Id,
            Subject = _testSubject
        };

        await _dbContext.Students.AddAsync(studentWithoutGrades);
        await _dbContext.SubjectsStudents.AddAsync(subjectStudent);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _studentsService.GetStudentGradesAsync(studentWithoutGrades.Id);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result!.StudentId, Is.EqualTo(studentWithoutGrades.Id));
            Assert.That(result.SubjectGrades.Count, Is.EqualTo(1));
            Assert.That(result.SubjectGrades.First().Grades, Is.Empty);
        });
    }
    //----------------GetStudentGradesAsync---------------------
    //----------------DeleteGradeAsync---------------------
    [Test]
    public async Task DeleteGradeAsync_ShouldReturnTrue_WhenGradeExists()
    {
        // Arrange
        int gradeId = _testGrade.Id;

        // Act
        bool result = await _studentsService.DeleteGradeAsync(gradeId);

        // Assert
        Assert.That(result, Is.True);

        var deletedGrade = await _dbContext.Grades
            .FirstOrDefaultAsync(g => g.Id == gradeId);
        Assert.That(deletedGrade, Is.Null);

        var student = await _dbContext.Students
            .Include(s => s.SubjectStudents)
            .FirstOrDefaultAsync(s => s.Id == _testStudent.Id);

        Assert.That(student, Is.Not.Null);
        Assert.That(student!.SubjectStudents.Count, Is.EqualTo(1));
    }

    [Test]
    public async Task DeleteGradeAsync_ShouldReturnFalse_WhenGradeDoesNotExist()
    {
        // Arrange
        int nonExistentId = 999;

        // Act
        bool result = await _studentsService.DeleteGradeAsync(nonExistentId);

        // Assert
        Assert.That(result, Is.False);

        var gradesCount = await _dbContext.Grades.CountAsync();
        Assert.That(gradesCount, Is.EqualTo(1));
    }

    [Test]
    public async Task DeleteGradeAsync_ShouldOnlyDeleteSpecificGrade_WhenStudentHasMultipleGrades()
    {
        // Arrange
        var secondGrade = new Grade
        {
            Id = 2,
            GradeValue = 6,
            AddedOn = DateTime.UtcNow,
            StudentId = _testStudent.Id,
            TeacherId = _testTeacher.GuidId,
            Teacher = _testTeacher,
            SubjectId = _testSubject.Id,
            Subject = _testSubject,
            GradeType = GradeType.Current
        };

        await _dbContext.Grades.AddAsync(secondGrade);
        await _dbContext.SaveChangesAsync();

        // Act
        bool result = await _studentsService.DeleteGradeAsync(_testGrade.Id);

        // Assert
        Assert.That(result, Is.True);

        var remainingGrade = await _dbContext.Grades
            .FirstOrDefaultAsync(g => g.Id == secondGrade.Id);
        Assert.That(remainingGrade, Is.Not.Null);

        var deletedGrade = await _dbContext.Grades
            .FirstOrDefaultAsync(g => g.Id == _testGrade.Id);
        Assert.That(deletedGrade, Is.Null);
    }
    //----------------DeleteGradeAsync---------------------
    //----------------AddStudentAsync---------------------
    [Test]
    public async Task AddStudentAsync_ShouldReturnFalse_WhenModelIsNull()
    {
        // Act
        bool result = await _studentsService.AddStudentAsync(null, Guid.NewGuid().ToString());

        // Assert
        Assert.That(result, Is.False);

        var studentsCount = await _dbContext.Students.CountAsync();
        Assert.That(studentsCount, Is.EqualTo(1));
    }

    [Test]
    public async Task AddStudentAsync_ShouldCreateStudent_WithAllSubjects()
    {
        // Arrange
        var secondSubject = new Subject
        {
            Id = 2,
            Name = "Physics"
        };
        await _dbContext.Subjects.AddAsync(secondSubject);
        await _dbContext.SaveChangesAsync();

        var model = new AddStudentFormModel
        {
            FirstName = "New",
            MiddleName = "Test",
            LastName = "Student",
            ClassId = _testClass.Id
        };

        string userId = Guid.NewGuid().ToString();

        // Act
        bool result = await _studentsService.AddStudentAsync(model, userId);

        // Assert
        Assert.That(result, Is.True);

        var addedStudent = await _dbContext.Students
            .Include(s => s.SubjectStudents)
            .FirstOrDefaultAsync(s => s.FirstName == model.FirstName);

        Assert.That(addedStudent, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(addedStudent!.FirstName, Is.EqualTo(model.FirstName));
            Assert.That(addedStudent.MiddleName, Is.EqualTo(model.MiddleName));
            Assert.That(addedStudent.LastName, Is.EqualTo(model.LastName));
            Assert.That(addedStudent.ClassId, Is.EqualTo(model.ClassId));
            Assert.That(addedStudent.ApplicationUserId.ToString(), Is.EqualTo(userId));

            Assert.That(addedStudent.SubjectStudents, Has.Count.EqualTo(2));
        });

        var subjectIds = addedStudent!.SubjectStudents.Select(ss => ss.SubjectId).ToList();
        Assert.Multiple(() =>
        {
            Assert.That(subjectIds, Does.Contain(_testSubject.Id));
            Assert.That(subjectIds, Does.Contain(secondSubject.Id));
        });
    }
    //----------------AddStudentAsync---------------------
}