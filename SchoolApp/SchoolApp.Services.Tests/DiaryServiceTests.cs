using Microsoft.EntityFrameworkCore;

using SchoolApp.Data;
using SchoolApp.Data.Models;
using SchoolApp.Data.Repository;
using SchoolApp.Data.Repository.Contracts;
using SchoolApp.Services.Data;
using SchoolApp.Web.ViewModels.Diary.AddForms;
using SchoolApp.Web.ViewModels.Diary.Remarks;

using static SchoolApp.Common.ApplicationConstants;

namespace SchoolApp.Services.Tests;

[TestFixture]
public class DiaryServiceTests
{
    private ApplicationDbContext _dbContext;
    private IRepository _repository;
    private DiaryService _diaryService;

    private Class _testClass1;
    private Class _testClass2;
    private Class _testClass3;
    private Section _testSection1;
    private Section _testSection2;
    private Student _testStudent1;
    private Student _testStudent2;
    private Subject _testSubject;
    private Subject _testSubject2;
    private Teacher _testTeacher;
    private Grade _testGrade1;
    private Grade _testGrade2;
    private Absence _testAbsence1;
    private Absence _testAbsence2;
    private Absence _testAbsence3;
    private Remark _testRemark1;
    private Remark _testRemark2;
    private Remark _testRemark3;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDiaryDb")
            .Options;

        _dbContext = new ApplicationDbContext(options);
        _repository = new BaseRepository(_dbContext);
        _diaryService = new DiaryService(_repository);

        _testSection1 = new Section
        {
            Id = 1,
            Name = "A"
        };

        _testSection2 = new Section
        {
            Id = 2,
            Name = "B"
        };

        _testClass1 = new Class
        {
            Id = 1,
            GradeLevel = 10,
            Section = _testSection1,
            SectionId = _testSection1.Id
        };

        _testClass2 = new Class
        {
            Id = 2,
            GradeLevel = 10,
            Section = _testSection2,
            SectionId = _testSection2.Id
        };

        _testClass3 = new Class
        {
            Id = 3,
            GradeLevel = 11,
            Section = _testSection1,
            SectionId = _testSection1.Id
        };

        _testTeacher = new Teacher
        {
            GuidId = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe",
            ImageUrl = "Test.url",
            JobTitle = "TestTitle",
            ApplicationUserId = Guid.NewGuid()
        };

        _testSubject = new Subject
        {
            Id = 1,
            Name = "Mathematics"
        };

        _testSubject2 = new Subject
        {
            Id = 2,
            Name = "Physics"
        };

        _testStudent1 = new Student
        {
            Id = 1,
            FirstName = "Alice",
            MiddleName = "TestMiddle",
            LastName = "Smith",
            ClassId = _testClass1.Id,
            Class = _testClass1,
            ApplicationUserId = Guid.NewGuid()
        };

        _testStudent2 = new Student
        {
            Id = 2,
            FirstName = "Bob",
            MiddleName = "MiddleName",
            LastName = "Johnson",
            ClassId = _testClass1.Id,
            Class = _testClass1,
            ApplicationUserId = Guid.NewGuid()
        };

        _testGrade1 = new Grade
        {
            Id = 1,
            GradeValue = 5,
            AddedOn = DateTime.Now.AddDays(-5),
            StudentId = _testStudent1.Id,
            Student = _testStudent1,
            TeacherId = _testTeacher.GuidId,
            Teacher = _testTeacher,
            SubjectId = _testSubject.Id,
            Subject = _testSubject,
            GradeType = GradeType.Current
        };

        _testGrade2 = new Grade
        {
            Id = 2,
            GradeValue = 6,
            AddedOn = DateTime.Now,
            StudentId = _testStudent1.Id,
            Student = _testStudent1,
            TeacherId = _testTeacher.GuidId,
            Teacher = _testTeacher,
            SubjectId = _testSubject.Id,
            Subject = _testSubject,
            GradeType = GradeType.FirstTerm
        };

        _testAbsence1 = new Absence
        {
            Id = 1,
            AddedOn = DateTime.Now.AddDays(-2),
            IsExcused = false,
            StudentId = _testStudent1.Id,
            Student = _testStudent1,
            SubjectId = _testSubject.Id,
            Subject = _testSubject
        };

        _testAbsence2 = new Absence
        {
            Id = 2,
            AddedOn = DateTime.Now.AddDays(-1),
            IsExcused = true,
            StudentId = _testStudent1.Id,
            Student = _testStudent1,
            SubjectId = _testSubject2.Id,
            Subject = _testSubject2
        };

        _testAbsence3 = new Absence
        {
            Id = 3,
            AddedOn = DateTime.Now,
            IsExcused = false,
            StudentId = _testStudent1.Id,
            Student = _testStudent1,
            SubjectId = _testSubject.Id,
            Subject = _testSubject
        };

        _testRemark1 = new Remark
        {
            Id = 1,
            RemarkText = "Test remark 1",
            AddedOn = DateTime.Now.AddDays(-2),
            StudentId = _testStudent1.Id,
            Student = _testStudent1,
            TeacherId = _testTeacher.GuidId,
            Teacher = _testTeacher,
            SubjectId = _testSubject.Id,
            Subject = _testSubject
        };

        _testRemark2 = new Remark
        {
            Id = 2,
            RemarkText = "Test remark 2",
            AddedOn = DateTime.Now.AddDays(-1),
            StudentId = _testStudent1.Id,
            Student = _testStudent1,
            TeacherId = _testTeacher.GuidId,
            Teacher = _testTeacher,
            SubjectId = _testSubject2.Id,
            Subject = _testSubject2
        };

        _testRemark3 = new Remark
        {
            Id = 3,
            RemarkText = "Test remark 3",
            AddedOn = DateTime.Now,
            StudentId = _testStudent1.Id,
            Student = _testStudent1,
            TeacherId = _testTeacher.GuidId,
            Teacher = _testTeacher,
            SubjectId = _testSubject.Id,
            Subject = _testSubject
        };

        _dbContext.Remarks.AddRange(_testRemark1, _testRemark2, _testRemark3);

        _dbContext.Sections.AddRange(_testSection1, _testSection2);
        _dbContext.Classes.AddRange(_testClass1, _testClass2, _testClass3);
        _dbContext.Teachers.Add(_testTeacher);
        _dbContext.Subjects.AddRange(_testSubject, _testSubject2);
        _dbContext.Students.AddRange(_testStudent1, _testStudent2);
        _dbContext.Grades.AddRange(_testGrade1, _testGrade2);
        _dbContext.Absences.AddRange(_testAbsence1, _testAbsence2, _testAbsence3);
        _dbContext.SaveChanges();
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    //----------------IndexGetAllClassesAsync---------------------
    [Test]
    public async Task IndexGetAllClassesAsync_ShouldReturnAllClasses()
    {
        // Act
        var result = await _diaryService.IndexGetAllClassesAsync();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(3));
    }

    [Test]
    public async Task IndexGetAllClassesAsync_ShouldReturnClassesOrderedByGradeLevelAndSection()
    {
        // Act
        var result = (await _diaryService.IndexGetAllClassesAsync()).ToList();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result[0].ClassName, Is.EqualTo("10 A"));
            Assert.That(result[1].ClassName, Is.EqualTo("10 B"));
            Assert.That(result[2].ClassName, Is.EqualTo("11 A"));
        });
    }

    [Test]
    public async Task IndexGetAllClassesAsync_ShouldReturnCorrectClassInformation()
    {
        // Act
        var result = await _diaryService.IndexGetAllClassesAsync();
        var firstClass = result.First();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(firstClass.ClassId, Is.EqualTo(_testClass1.Id));
            Assert.That(firstClass.ClassName, Is.EqualTo($"{_testClass1.GradeLevel} {_testClass1.Section.Name}"));
        });
    }

    [Test]
    public async Task IndexGetAllClassesAsync_WhenNoClassesExist_ShouldReturnEmptyCollection()
    {
        // Arrange
        _dbContext.Grades.RemoveRange(_dbContext.Grades);
        _dbContext.Absences.RemoveRange(_dbContext.Absences);
        _dbContext.Students.RemoveRange(_dbContext.Students);
        _dbContext.Classes.RemoveRange(_dbContext.Classes);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _diaryService.IndexGetAllClassesAsync();

        // Assert
        Assert.That(result, Is.Empty);
    }

    //----------------GetGradeContentAsync---------------------
    [Test]
    public async Task GetGradeContentAsync_ShouldReturnAllStudentsInClass()
    {
        // Act
        var result = await _diaryService.GetGradeContentAsync(_testClass1.Id, _testSubject.Id);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(2));
    }

    [Test]
    public async Task GetGradeContentAsync_ShouldReturnStudentWithCorrectGrades()
    {
        // Act
        var result = await _diaryService.GetGradeContentAsync(_testClass1.Id, _testSubject.Id);
        var student = result.First(s => s.FirstName == _testStudent1.FirstName);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(student.FirstName, Is.EqualTo(_testStudent1.FirstName));
            Assert.That(student.LastName, Is.EqualTo(_testStudent1.LastName));
            Assert.That(student.Grades.Count(), Is.EqualTo(2));
        });

        var grades = student.Grades.OrderBy(g => g.GradeDate).ToList();
        Assert.Multiple(() =>
        {
            Assert.That(grades[0].GradeValue, Is.EqualTo(_testGrade1.GradeValue));
            Assert.That(grades[0].GradeDate, Is.EqualTo(_testGrade1.AddedOn));
            Assert.That(grades[0].TeacherName, Is.EqualTo($"{_testTeacher.FirstName} {_testTeacher.LastName}"));
            Assert.That(grades[0].GradeType, Is.EqualTo(_testGrade1.GradeType));

            Assert.That(grades[1].GradeValue, Is.EqualTo(_testGrade2.GradeValue));
            Assert.That(grades[1].GradeDate, Is.EqualTo(_testGrade2.AddedOn));
            Assert.That(grades[1].TeacherName, Is.EqualTo($"{_testTeacher.FirstName} {_testTeacher.LastName}"));
            Assert.That(grades[1].GradeType, Is.EqualTo(_testGrade2.GradeType));
        });
    }

    [Test]
    public async Task GetGradeContentAsync_StudentWithNoGrades_ShouldReturnEmptyGradesList()
    {
        // Act
        var result = await _diaryService.GetGradeContentAsync(_testClass1.Id, _testSubject.Id);
        var student = result.First(s => s.FirstName == _testStudent2.FirstName);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(student.FirstName, Is.EqualTo(_testStudent2.FirstName));
            Assert.That(student.LastName, Is.EqualTo(_testStudent2.LastName));
            Assert.That(student.Grades, Is.Empty);
        });
    }

    [Test]
    public async Task GetGradeContentAsync_WithNonExistentClass_ShouldReturnEmptyCollection()
    {
        // Act
        var result = await _diaryService.GetGradeContentAsync(999, _testSubject.Id);

        // Assert
        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task GetGradeContentAsync_ShouldReturnOnlyGradesForSpecificSubject()
    {
        // Arrange
        var otherSubject = new Subject
        {
            Id = 3,
            Name = "Biology"
        };

        var otherGrade = new Grade
        {
            Id = 3,
            GradeValue = 4,
            AddedOn = DateTime.Now,
            StudentId = _testStudent1.Id,
            Student = _testStudent1,
            TeacherId = _testTeacher.GuidId,
            Teacher = _testTeacher,
            SubjectId = otherSubject.Id,
            Subject = otherSubject,
            GradeType = GradeType.Current
        };

        await _dbContext.Subjects.AddAsync(otherSubject);
        await _dbContext.Grades.AddAsync(otherGrade);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _diaryService.GetGradeContentAsync(_testClass1.Id, _testSubject.Id);
        var student = result.First(s => s.FirstName == _testStudent1.FirstName);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(student.Grades.Count(), Is.EqualTo(2));
            Assert.That(student.Grades.All(g => g.GradeValue != otherGrade.GradeValue), Is.True);
        });
    }

    //----------------GetAbsencesContentAsync---------------------
    [Test]
    public async Task GetAbsencesContentAsync_ShouldReturnAllStudentsInClass()
    {
        // Act
        var result = await _diaryService.GetAbsencesContentAsync(_testClass1.Id);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(2));
    }

    [Test]
    public async Task GetAbsencesContentAsync_ShouldReturnStudentWithCorrectAbsences()
    {
        // Act
        var result = await _diaryService.GetAbsencesContentAsync(_testClass1.Id);
        var student = result.First(s => s.FirstName == _testStudent1.FirstName);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(student.FirstName, Is.EqualTo(_testStudent1.FirstName));
            Assert.That(student.LastName, Is.EqualTo(_testStudent1.LastName));
            Assert.That(student.Absences.Count(), Is.EqualTo(3));
        });

        var absences = student.Absences.OrderBy(a => a.AddedOn).ToList();
        Assert.Multiple(() =>
        {
            Assert.That(absences[0].Id, Is.EqualTo(_testAbsence1.Id));
            Assert.That(absences[0].SubjectName, Is.EqualTo(_testSubject.Name));
            Assert.That(absences[0].AddedOn, Is.EqualTo(_testAbsence1.AddedOn.ToString(DateFormat)));
            Assert.That(absences[0].IsExcused, Is.EqualTo(_testAbsence1.IsExcused));

            Assert.That(absences[1].Id, Is.EqualTo(_testAbsence2.Id));
            Assert.That(absences[1].SubjectName, Is.EqualTo(_testSubject2.Name));
            Assert.That(absences[1].AddedOn, Is.EqualTo(_testAbsence2.AddedOn.ToString(DateFormat)));
            Assert.That(absences[1].IsExcused, Is.EqualTo(_testAbsence2.IsExcused));

            Assert.That(absences[2].Id, Is.EqualTo(_testAbsence3.Id));
            Assert.That(absences[2].SubjectName, Is.EqualTo(_testSubject.Name));
            Assert.That(absences[2].AddedOn, Is.EqualTo(_testAbsence3.AddedOn.ToString(DateFormat)));
            Assert.That(absences[2].IsExcused, Is.EqualTo(_testAbsence3.IsExcused));
        });
    }

    [Test]
    public async Task GetAbsencesContentAsync_StudentWithNoAbsences_ShouldReturnEmptyAbsencesList()
    {
        // Act
        var result = await _diaryService.GetAbsencesContentAsync(_testClass1.Id);
        var student = result.First(s => s.FirstName == _testStudent2.FirstName);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(student.FirstName, Is.EqualTo(_testStudent2.FirstName));
            Assert.That(student.LastName, Is.EqualTo(_testStudent2.LastName));
            Assert.That(student.Absences, Is.Empty);
        });
    }

    [Test]
    public async Task GetAbsencesContentAsync_WithNonExistentClass_ShouldReturnEmptyCollection()
    {
        // Act
        var result = await _diaryService.GetAbsencesContentAsync(999);

        // Assert
        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task GetAbsencesContentAsync_ShouldIncludeAbsencesFromAllSubjects()
    {
        // Act
        var result = await _diaryService.GetAbsencesContentAsync(_testClass1.Id);
        var student = result.First(s => s.FirstName == _testStudent1.FirstName);
        var subjectNames = student.Absences.Select(a => a.SubjectName).Distinct().ToList();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(subjectNames, Has.Count.EqualTo(2));
            Assert.That(subjectNames, Does.Contain(_testSubject.Name));
            Assert.That(subjectNames, Does.Contain(_testSubject2.Name));
        });
    }

    [Test]
    public async Task GetAbsencesContentAsync_ShouldHandleBothExcusedAndUnexcusedAbsences()
    {
        // Act
        var result = await _diaryService.GetAbsencesContentAsync(_testClass1.Id);
        var student = result.First(s => s.FirstName == _testStudent1.FirstName);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(student.Absences.Count(a => a.IsExcused), Is.EqualTo(1));
            Assert.That(student.Absences.Count(a => !a.IsExcused), Is.EqualTo(2));
        });
    }

    //----------------GetClassContentAsync---------------------
    [Test]
    public async Task GetClassContentAsync_ShouldReturnAllSubjects()
    {
        // Act
        var result = await _diaryService.GetClassContentAsync(_testClass1.Id);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(2));
    }

    [Test]
    public async Task GetClassContentAsync_ShouldReturnCorrectSubjectInformation()
    {
        // Act
        var result = await _diaryService.GetClassContentAsync(_testClass1.Id);
        var subjects = result.ToList();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(subjects[0].Id, Is.EqualTo(_testSubject.Id));
            Assert.That(subjects[0].Name, Is.EqualTo(_testSubject.Name));

            Assert.That(subjects[1].Id, Is.EqualTo(_testSubject2.Id));
            Assert.That(subjects[1].Name, Is.EqualTo(_testSubject2.Name));
        });
    }

    [Test]
    public async Task GetClassContentAsync_WhenNoSubjectsExist_ShouldReturnEmptyCollection()
    {
        // Arrange
        _dbContext.Grades.RemoveRange(_dbContext.Grades);
        _dbContext.Absences.RemoveRange(_dbContext.Absences);
        _dbContext.Subjects.RemoveRange(_dbContext.Subjects);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _diaryService.GetClassContentAsync(_testClass1.Id);

        // Assert
        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task GetClassContentAsync_WithNonExistentClass_ShouldStillReturnAllSubjects()
    {
        // Act
        var result = await _diaryService.GetClassContentAsync(999);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(2));
    }

    //----------------GetRemarksContentAsync---------------------
    [Test]
    public async Task GetRemarksContentAsync_ShouldReturnAllStudentsInClass()
    {
        // Act
        var result = await _diaryService.GetRemarksContentAsync(_testClass1.Id);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(2));
    }

    [Test]
    public async Task GetRemarksContentAsync_ShouldReturnStudentWithCorrectRemarks()
    {
        // Act
        var result = await _diaryService.GetRemarksContentAsync(_testClass1.Id);
        var student = result.First(s => s.FirstName == _testStudent1.FirstName);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(student.FirstName, Is.EqualTo(_testStudent1.FirstName));
            Assert.That(student.LastName, Is.EqualTo(_testStudent1.LastName));
            Assert.That(student.Remarks.Count(), Is.EqualTo(3));
        });

        var remarks = student.Remarks.OrderBy(r => r.AddedOn).ToList();
        Assert.Multiple(() =>
        {
            Assert.That(remarks[0].Id, Is.EqualTo(_testRemark1.Id));
            Assert.That(remarks[0].SubjectName, Is.EqualTo(_testSubject.Name));
            Assert.That(remarks[0].TeacherName, Is.EqualTo($"{_testTeacher.FirstName} {_testTeacher.LastName}"));
            Assert.That(remarks[0].RemarkText, Is.EqualTo(_testRemark1.RemarkText));
            Assert.That(remarks[0].AddedOn, Is.EqualTo(_testRemark1.AddedOn.ToString(DateFormat)));

            Assert.That(remarks[1].Id, Is.EqualTo(_testRemark2.Id));
            Assert.That(remarks[1].SubjectName, Is.EqualTo(_testSubject2.Name));
            Assert.That(remarks[1].TeacherName, Is.EqualTo($"{_testTeacher.FirstName} {_testTeacher.LastName}"));
            Assert.That(remarks[1].RemarkText, Is.EqualTo(_testRemark2.RemarkText));
            Assert.That(remarks[1].AddedOn, Is.EqualTo(_testRemark2.AddedOn.ToString(DateFormat)));

            Assert.That(remarks[2].Id, Is.EqualTo(_testRemark3.Id));
            Assert.That(remarks[2].SubjectName, Is.EqualTo(_testSubject.Name));
            Assert.That(remarks[2].TeacherName, Is.EqualTo($"{_testTeacher.FirstName} {_testTeacher.LastName}"));
            Assert.That(remarks[2].RemarkText, Is.EqualTo(_testRemark3.RemarkText));
            Assert.That(remarks[2].AddedOn, Is.EqualTo(_testRemark3.AddedOn.ToString(DateFormat)));
        });
    }

    [Test]
    public async Task GetRemarksContentAsync_StudentWithNoRemarks_ShouldReturnEmptyRemarksList()
    {
        // Act
        var result = await _diaryService.GetRemarksContentAsync(_testClass1.Id);
        var student = result.First(s => s.FirstName == _testStudent2.FirstName);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(student.FirstName, Is.EqualTo(_testStudent2.FirstName));
            Assert.That(student.LastName, Is.EqualTo(_testStudent2.LastName));
            Assert.That(student.Remarks, Is.Empty);
        });
    }

    [Test]
    public async Task GetRemarksContentAsync_WithNonExistentClass_ShouldReturnEmptyCollection()
    {
        // Act
        var result = await _diaryService.GetRemarksContentAsync(999);

        // Assert
        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task GetRemarksContentAsync_ShouldIncludeRemarksFromAllSubjects()
    {
        // Act
        var result = await _diaryService.GetRemarksContentAsync(_testClass1.Id);
        var student = result.First(s => s.FirstName == _testStudent1.FirstName);
        var subjectNames = student.Remarks.Select(r => r.SubjectName).Distinct().ToList();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(subjectNames, Has.Count.EqualTo(2));
            Assert.That(subjectNames, Does.Contain(_testSubject.Name));
            Assert.That(subjectNames, Does.Contain(_testSubject2.Name));
        });
    }

    //----------------GetClassStudentForAddAsync---------------------
    [Test]
    public async Task GetClassStudentForAddAsync_WithGradeFormModel_ShouldReturnCorrectData()
    {
        // Act
        var result = await _diaryService.GetClassStudentForAddAsync<GradeFormModel>(_testClass1.Id, _testSubject.Id);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.AddedOn.Date, Is.EqualTo(DateTime.Now.Date));
            Assert.That(result.SubjectId, Is.EqualTo(_testSubject.Id));
            Assert.That(result.Subjects.Count(), Is.EqualTo(2));
            Assert.That(result.Students, Is.Not.Null);
            Assert.That(result.Students, Has.Count.EqualTo(2));
        });

        var subjects = result.Subjects.ToList();
        Assert.Multiple(() =>
        {
            Assert.That(subjects[0].Id, Is.EqualTo(_testSubject.Id));
            Assert.That(subjects[0].Name, Is.EqualTo(_testSubject.Name));
            Assert.That(subjects[1].Id, Is.EqualTo(_testSubject2.Id));
            Assert.That(subjects[1].Name, Is.EqualTo(_testSubject2.Name));
        });

        var students = result.Students.ToList();
        Assert.Multiple(() =>
        {
            Assert.That(students[0].Id, Is.EqualTo(_testStudent1.Id));
            Assert.That(students[0].FirstName, Is.EqualTo(_testStudent1.FirstName));
            Assert.That(students[0].LastName, Is.EqualTo(_testStudent1.LastName));
            Assert.That(students[0].Grade, Is.EqualTo(0));

            Assert.That(students[1].Id, Is.EqualTo(_testStudent2.Id));
            Assert.That(students[1].FirstName, Is.EqualTo(_testStudent2.FirstName));
            Assert.That(students[1].LastName, Is.EqualTo(_testStudent2.LastName));
            Assert.That(students[1].Grade, Is.EqualTo(0));
        });
    }

    [Test]
    public async Task GetClassStudentForAddAsync_WithAbsenceFormModel_ShouldReturnCorrectData()
    {
        // Act
        var result = await _diaryService.GetClassStudentForAddAsync<AbsenceFormModel>(_testClass1.Id, _testSubject.Id);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.AddedOn.Date, Is.EqualTo(DateTime.Now.Date));
            Assert.That(result.SubjectId, Is.EqualTo(_testSubject.Id));
            Assert.That(result.Subjects.Count(), Is.EqualTo(2));
            Assert.That(result.Students, Is.Not.Null);
            Assert.That(result.Students, Has.Count.EqualTo(2));
        });

        var students = result.Students.ToList();
        Assert.Multiple(() =>
        {
            Assert.That(students[0].Id, Is.EqualTo(_testStudent1.Id));
            Assert.That(students[0].FirstName, Is.EqualTo(_testStudent1.FirstName));
            Assert.That(students[0].LastName, Is.EqualTo(_testStudent1.LastName));
            Assert.That(students[0].IsChecked, Is.False);

            Assert.That(students[1].Id, Is.EqualTo(_testStudent2.Id));
            Assert.That(students[1].FirstName, Is.EqualTo(_testStudent2.FirstName));
            Assert.That(students[1].LastName, Is.EqualTo(_testStudent2.LastName));
            Assert.That(students[1].IsChecked, Is.False);
        });
    }

    [Test]
    public async Task GetClassStudentForAddAsync_WithRemarkFormModel_ShouldReturnCorrectData()
    {
        // Act
        var result = await _diaryService.GetClassStudentForAddAsync<RemarkFormModel>(_testClass1.Id, _testSubject.Id);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.AddedOn.Date, Is.EqualTo(DateTime.Now.Date));
            Assert.That(result.SubjectId, Is.EqualTo(_testSubject.Id));
            Assert.That(result.Subjects.Count(), Is.EqualTo(2));
            Assert.That(result.Students, Is.Not.Null);
            Assert.That(result.Students, Has.Count.EqualTo(2));
        });

        var students = result.Students.ToList();
        Assert.Multiple(() =>
        {
            Assert.That(students[0].Id, Is.EqualTo(_testStudent1.Id));
            Assert.That(students[0].FirstName, Is.EqualTo(_testStudent1.FirstName));
            Assert.That(students[0].LastName, Is.EqualTo(_testStudent1.LastName));

            Assert.That(students[1].Id, Is.EqualTo(_testStudent2.Id));
            Assert.That(students[1].FirstName, Is.EqualTo(_testStudent2.FirstName));
            Assert.That(students[1].LastName, Is.EqualTo(_testStudent2.LastName));
        });
    }

    [Test]
    public async Task GetClassStudentForAddAsync_WithNonExistentClass_ShouldReturnModelWithEmptyStudentsList()
    {
        // Act
        var resultGrade = await _diaryService.GetClassStudentForAddAsync<GradeFormModel>(999, _testSubject.Id);
        var resultAbsence = await _diaryService.GetClassStudentForAddAsync<AbsenceFormModel>(999, _testSubject.Id);
        var resultRemark = await _diaryService.GetClassStudentForAddAsync<RemarkFormModel>(999, _testSubject.Id);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(resultGrade.Students, Is.Empty);
            Assert.That(resultAbsence.Students, Is.Empty);
            Assert.That(resultRemark.Students, Is.Empty);
        });
    }

    //----------------AddGradesAsync---------------------
    [Test]
    public async Task AddGradesAsync_WithValidData_ShouldAddGradesSuccessfully()
    {
        _dbContext.Grades.RemoveRange(_dbContext.Grades);
        await _dbContext.SaveChangesAsync();

        // Arrange
        var model = new GradeFormModel
        {
            AddedOn = DateTime.Now,
            SubjectId = _testSubject.Id,
            GradeType = GradeType.Current,
            Students = new List<StudentGradeFormModel>
            {
                new StudentGradeFormModel
                {
                    Id = _testStudent1.Id,
                    FirstName = _testStudent1.FirstName,
                    LastName = _testStudent1.LastName,
                    Grade = 6
                },
                new StudentGradeFormModel
                {
                    Id = _testStudent2.Id,
                    FirstName = _testStudent2.FirstName,
                    LastName = _testStudent2.LastName,
                    Grade = 5
                }
            }
        };

        // Act
        bool result = await _diaryService.AddGradesAsync(_testTeacher.ApplicationUserId.ToString(), model);

        // Assert
        Assert.That(result, Is.True);

        var addedGrades = await _dbContext.Grades
            .Where(g => g.AddedOn.Date == model.AddedOn.Date &&
                       g.SubjectId == model.SubjectId)
            .ToListAsync();

        Assert.Multiple(() =>
        {
            Assert.That(addedGrades, Has.Count.EqualTo(2));

            var grade1 = addedGrades.First(g => g.StudentId == _testStudent1.Id);
            Assert.That(grade1.GradeValue, Is.EqualTo(6));
            Assert.That(grade1.TeacherId, Is.EqualTo(_testTeacher.GuidId));
            Assert.That(grade1.GradeType, Is.EqualTo(GradeType.Current));

            var grade2 = addedGrades.First(g => g.StudentId == _testStudent2.Id);
            Assert.That(grade2.GradeValue, Is.EqualTo(5));
            Assert.That(grade2.TeacherId, Is.EqualTo(_testTeacher.GuidId));
            Assert.That(grade2.GradeType, Is.EqualTo(GradeType.Current));
        });
    }

    [Test]
    public async Task AddGradesAsync_WithInvalidTeacherId_ShouldReturnFalse()
    {
        _dbContext.Grades.RemoveRange(_dbContext.Grades);
        await _dbContext.SaveChangesAsync();

        // Arrange
        var model = new GradeFormModel
        {
            AddedOn = DateTime.Now,
            SubjectId = _testSubject.Id,
            GradeType = GradeType.Current,
            Students = new List<StudentGradeFormModel>
            {
                new StudentGradeFormModel
                {
                    Id = _testStudent1.Id,
                    FirstName = _testStudent1.FirstName,
                    LastName = _testStudent1.LastName,
                    Grade = 6
                }
            }
        };

        // Act
        bool result = await _diaryService.AddGradesAsync(Guid.NewGuid().ToString(), model);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.False);
            Assert.That(_dbContext.Grades
                .Where(g => g.AddedOn.Date == model.AddedOn.Date)
                .Count(), Is.EqualTo(0));
        });
    }

    [Test]
    public async Task AddGradesAsync_ShouldOnlyAddGradesForNonZeroValues()
    {
        _dbContext.Grades.RemoveRange(_dbContext.Grades);
        await _dbContext.SaveChangesAsync();

        // Arrange
        var model = new GradeFormModel
        {
            AddedOn = DateTime.Now,
            SubjectId = _testSubject.Id,
            GradeType = GradeType.Current,
            Students = new List<StudentGradeFormModel>
            {
                new StudentGradeFormModel
                {
                    Id = _testStudent1.Id,
                    FirstName = _testStudent1.FirstName,
                    LastName = _testStudent1.LastName,
                    Grade = 6
                },
                new StudentGradeFormModel
                {
                    Id = _testStudent2.Id,
                    FirstName = _testStudent2.FirstName,
                    LastName = _testStudent2.LastName,
                    Grade = 0
                }
            }
        };

        // Act
        bool result = await _diaryService.AddGradesAsync(_testTeacher.ApplicationUserId.ToString(), model);

        // Assert
        Assert.That(result, Is.True);

        var addedGrades = await _dbContext.Grades
            .Where(g => g.AddedOn.Date == model.AddedOn.Date)
            .ToListAsync();

        Assert.Multiple(() =>
        {
            Assert.That(addedGrades, Has.Count.EqualTo(1));
            Assert.That(addedGrades[0].StudentId, Is.EqualTo(_testStudent1.Id));
            Assert.That(addedGrades[0].GradeValue, Is.EqualTo(6));
        });
    }

    //----------------AddAbsenceAsync---------------------
    [Test]
    public async Task AddAbsenceAsync_WithValidData_ShouldAddAbsencesSuccessfully()
    {
        _dbContext.Absences.RemoveRange(_dbContext.Absences);
        await _dbContext.SaveChangesAsync();

        // Arrange
        var model = new AbsenceFormModel
        {
            AddedOn = DateTime.Now,
            SubjectId = _testSubject.Id,
            Students = new List<StudentAbcenseFormModel>
            {
                new StudentAbcenseFormModel
                {
                    Id = _testStudent1.Id,
                    FirstName = _testStudent1.FirstName,
                    LastName = _testStudent1.LastName,
                    IsChecked = true
                },
                new StudentAbcenseFormModel
                {
                    Id = _testStudent2.Id,
                    FirstName = _testStudent2.FirstName,
                    LastName = _testStudent2.LastName,
                    IsChecked = true
                }
            }
        };

        // Act
        bool result = await _diaryService.AddAbsenceAsync(model);

        // Assert
        Assert.That(result, Is.True);

        var addedAbsences = await _dbContext.Absences
            .Where(a => a.AddedOn.Date == model.AddedOn.Date &&
                       a.SubjectId == model.SubjectId)
            .ToListAsync();

        Assert.Multiple(() =>
        {
            Assert.That(addedAbsences, Has.Count.EqualTo(2));

            var absence1 = addedAbsences.First(a => a.StudentId == _testStudent1.Id);
            Assert.That(absence1.IsExcused, Is.False);
            Assert.That(absence1.SubjectId, Is.EqualTo(_testSubject.Id));

            var absence2 = addedAbsences.First(a => a.StudentId == _testStudent2.Id);
            Assert.That(absence2.IsExcused, Is.False);
            Assert.That(absence2.SubjectId, Is.EqualTo(_testSubject.Id));
        });
    }

    [Test]
    public async Task AddAbsenceAsync_ShouldOnlyAddAbsencesForCheckedStudents()
    {

        _dbContext.Absences.RemoveRange(_dbContext.Absences);
        await _dbContext.SaveChangesAsync();

        // Arrange
        var model = new AbsenceFormModel
        {
            AddedOn = DateTime.Now,
            SubjectId = _testSubject.Id,
            Students = new List<StudentAbcenseFormModel>
            {
                new StudentAbcenseFormModel
                {
                    Id = _testStudent1.Id,
                    FirstName = _testStudent1.FirstName,
                    LastName = _testStudent1.LastName,
                    IsChecked = true
                },
                new StudentAbcenseFormModel
                {
                    Id = _testStudent2.Id,
                    FirstName = _testStudent2.FirstName,
                    LastName = _testStudent2.LastName,
                    IsChecked = false
                }
            }
        };

        // Act
        bool result = await _diaryService.AddAbsenceAsync(model);

        // Assert
        Assert.That(result, Is.True);

        var addedAbsences = await _dbContext.Absences
            .Where(a => a.AddedOn.Date == model.AddedOn.Date)
            .ToListAsync();

        Assert.Multiple(() =>
        {
            Assert.That(addedAbsences, Has.Count.EqualTo(1));
            Assert.That(addedAbsences[0].StudentId, Is.EqualTo(_testStudent1.Id));
            Assert.That(addedAbsences[0].SubjectId, Is.EqualTo(_testSubject.Id));
            Assert.That(addedAbsences[0].IsExcused, Is.False);
        });
    }

    [Test]
    public async Task AddAbsenceAsync_WithNullModel_ShouldReturnFalse()
    {
        // Act
        bool result = await _diaryService.AddAbsenceAsync(null);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.False);
            Assert.That(_dbContext.Absences.Count(), Is.EqualTo(3));
        });
    }

    //----------------AddRemarkAsync---------------------
    [Test]
    public async Task AddRemarkAsync_WithValidData_ShouldAddRemarkSuccessfully()
    {
        // Arrange
        _dbContext.Remarks.RemoveRange(_dbContext.Remarks);
        await _dbContext.SaveChangesAsync();

        var model = new RemarkFormModel
        {
            AddedOn = DateTime.Now,
            SubjectId = _testSubject.Id,
            StudentId = _testStudent1.Id,
            RemarkText = "New test remark"
        };

        // Act
        bool result = await _diaryService.AddRemarkAsync(_testTeacher.ApplicationUserId.ToString(), model);

        // Assert
        Assert.That(result, Is.True);

        var addedRemark = await _dbContext.Remarks
            .FirstOrDefaultAsync(r => r.AddedOn.Date == model.AddedOn.Date &&
                                     r.SubjectId == model.SubjectId &&
                                     r.StudentId == model.StudentId);

        Assert.That(addedRemark, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(addedRemark!.RemarkText, Is.EqualTo(model.RemarkText));
            Assert.That(addedRemark.TeacherId, Is.EqualTo(_testTeacher.GuidId));
        });
    }

    [Test]
    public async Task AddRemarkAsync_WithNullModel_ShouldReturnFalse()
    {
        // Act
        bool result = await _diaryService.AddRemarkAsync(_testTeacher.ApplicationUserId.ToString(), null);

        // Assert
        Assert.That(result, Is.False);

        var remarksCount = await _dbContext.Remarks.CountAsync();
        Assert.That(remarksCount, Is.EqualTo(3));
    }

    [Test]
    public async Task AddRemarkAsync_WithInvalidTeacherId_ShouldReturnFalse()
    {
        // Arrange
        var model = new RemarkFormModel
        {
            AddedOn = DateTime.Now,
            SubjectId = _testSubject.Id,
            StudentId = _testStudent1.Id,
            RemarkText = "Test remark"
        };

        // Act
        bool result = await _diaryService.AddRemarkAsync(Guid.NewGuid().ToString(), model);

        // Assert
        Assert.That(result, Is.False);

        var remarksCount = await _dbContext.Remarks.CountAsync();
        Assert.That(remarksCount, Is.EqualTo(3));
    }

    //----------------ExcuseAbsenceAsync---------------------
    [Test]
    public async Task ExcuseAbsenceAsync_WithValidId_ShouldExcuseAbsence()
    {
        // Arrange
        var unexcusedAbsence = _testAbsence1;
        Assert.That(unexcusedAbsence.IsExcused, Is.False);

        // Act
        bool result = await _diaryService.ExcuseAbsenceAsync(unexcusedAbsence.Id);

        // Assert
        Assert.That(result, Is.True);

        var excusedAbsence = await _dbContext.Absences
            .FirstOrDefaultAsync(a => a.Id == unexcusedAbsence.Id);

        Assert.That(excusedAbsence, Is.Not.Null);
        Assert.That(excusedAbsence!.IsExcused, Is.True);
    }

    [Test]
    public async Task ExcuseAbsenceAsync_WithNonExistentId_ShouldReturnFalse()
    {
        // Arrange
        int nonExistentId = 999;

        // Act
        bool result = await _diaryService.ExcuseAbsenceAsync(nonExistentId);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public async Task ExcuseAbsenceAsync_ShouldOnlyExcuseSpecificAbsence()
    {
        // Arrange
        var absenceToExcuse = _testAbsence1;
        var otherAbsence = _testAbsence3;

        // Act
        bool result = await _diaryService.ExcuseAbsenceAsync(absenceToExcuse.Id);

        // Assert
        Assert.That(result, Is.True);

        var excusedAbsence = await _dbContext.Absences
            .FirstOrDefaultAsync(a => a.Id == absenceToExcuse.Id);
        var unaffectedAbsence = await _dbContext.Absences
            .FirstOrDefaultAsync(a => a.Id == otherAbsence.Id);

        Assert.Multiple(() =>
        {
            Assert.That(excusedAbsence!.IsExcused, Is.True);
            Assert.That(unaffectedAbsence!.IsExcused, Is.False);
        });
    }

    [Test]
    public async Task ExcuseAbsenceAsync_WithAlreadyExcusedAbsence_ShouldStillReturnTrue()
    {
        // Arrange
        var excusedAbsence = _testAbsence2;
        Assert.That(excusedAbsence.IsExcused, Is.True);

        // Act
        bool result = await _diaryService.ExcuseAbsenceAsync(excusedAbsence.Id);

        // Assert
        Assert.That(result, Is.True);

        var absence = await _dbContext.Absences
            .FirstOrDefaultAsync(a => a.Id == excusedAbsence.Id);
        Assert.That(absence!.IsExcused, Is.True);
    }

    //----------------DeleteAbsenceAsync---------------------
    [Test]
    public async Task DeleteAbsenceAsync_WithValidId_ShouldDeleteAbsence()
    {
        // Arrange
        int absenceId = _testAbsence1.Id;

        // Act
        bool result = await _diaryService.DeleteAbsenceAsync(absenceId);

        // Assert
        Assert.That(result, Is.True);

        var deletedAbsence = await _dbContext.Absences
            .FirstOrDefaultAsync(a => a.Id == absenceId);
        Assert.That(deletedAbsence, Is.Null);
    }

    [Test]
    public async Task DeleteAbsenceAsync_WithNonExistentId_ShouldReturnFalse()
    {
        // Arrange
        int nonExistentId = 999;

        // Act
        bool result = await _diaryService.DeleteAbsenceAsync(nonExistentId);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.False);
            Assert.That(_dbContext.Absences.Count(), Is.EqualTo(3));
        });
    }

    [Test]
    public async Task DeleteAbsenceAsync_ShouldOnlyDeleteSpecificAbsence()
    {
        // Arrange
        int absenceToDeleteId = _testAbsence1.Id;
        int initialAbsencesCount = await _dbContext.Absences.CountAsync();

        // Act
        bool result = await _diaryService.DeleteAbsenceAsync(absenceToDeleteId);

        // Assert
        Assert.That(result, Is.True);

        var remainingAbsencesCount = await _dbContext.Absences.CountAsync();
        Assert.Multiple(() =>
        {
            Assert.That(remainingAbsencesCount, Is.EqualTo(initialAbsencesCount - 1));
            Assert.That(_dbContext.Absences.Any(a => a.Id == _testAbsence2.Id), Is.True);
            Assert.That(_dbContext.Absences.Any(a => a.Id == _testAbsence3.Id), Is.True);
        });
    }

    //----------------DeleteRemarkAsync---------------------
    [Test]
    public async Task DeleteRemarkAsync_WithValidId_ShouldDeleteRemark()
    {
        // Arrange
        int remarkId = _testRemark1.Id;

        // Act
        bool result = await _diaryService.DeleteRemarkAsync(remarkId);

        // Assert
        Assert.That(result, Is.True);

        var deletedRemark = await _dbContext.Remarks
            .FirstOrDefaultAsync(r => r.Id == remarkId);
        Assert.That(deletedRemark, Is.Null);
    }

    [Test]
    public async Task DeleteRemarkAsync_WithNonExistentId_ShouldReturnFalse()
    {
        // Arrange
        int nonExistentId = 999;

        // Act
        bool result = await _diaryService.DeleteRemarkAsync(nonExistentId);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.False);
            Assert.That(_dbContext.Remarks.Count(), Is.EqualTo(3));
        });
    }

    [Test]
    public async Task DeleteRemarkAsync_ShouldOnlyDeleteSpecificRemark()
    {
        // Arrange
        int remarkToDeleteId = _testRemark1.Id;
        int initialRemarksCount = await _dbContext.Remarks.CountAsync();

        // Act
        bool result = await _diaryService.DeleteRemarkAsync(remarkToDeleteId);

        // Assert
        Assert.That(result, Is.True);

        var remainingRemarksCount = await _dbContext.Remarks.CountAsync();
        Assert.Multiple(() =>
        {
            Assert.That(remainingRemarksCount, Is.EqualTo(initialRemarksCount - 1));
            Assert.That(_dbContext.Remarks.Any(r => r.Id == _testRemark2.Id), Is.True);
            Assert.That(_dbContext.Remarks.Any(r => r.Id == _testRemark3.Id), Is.True);
        });
    }

    //----------------GetRemarkByIdAsync---------------------
    [Test]
    public async Task GetRemarkByIdAsync_WithValidId_ShouldReturnCorrectRemark()
    {
        // Act
        var result = await _diaryService.GetRemarkByIdAsync(_testRemark1.Id);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result!.Id, Is.EqualTo(_testRemark1.Id));
            Assert.That(result.AddedOn, Is.EqualTo(_testRemark1.AddedOn));
            Assert.That(result.SubjectId, Is.EqualTo(_testRemark1.SubjectId));
            Assert.That(result.StudentId, Is.EqualTo(_testRemark1.StudentId));
            Assert.That(result.RemarkText, Is.EqualTo(_testRemark1.RemarkText));
        });
    }

    [Test]
    public async Task GetRemarkByIdAsync_WithNonExistentId_ShouldReturnNull()
    {
        // Arrange
        int nonExistentId = 999;

        // Act
        var result = await _diaryService.GetRemarkByIdAsync(nonExistentId);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetRemarkByIdAsync_ShouldReturnCorrectModel_WithAllProperties()
    {
        var newRemark = new Remark
        {
            Id = 4,
            RemarkText = "Special test remark",
            AddedOn = DateTime.Now.AddDays(-1),
            StudentId = _testStudent2.Id,
            Student = _testStudent2,
            TeacherId = _testTeacher.GuidId,
            Teacher = _testTeacher,
            SubjectId = _testSubject2.Id,
            Subject = _testSubject2
        };

        await _dbContext.Remarks.AddAsync(newRemark);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _diaryService.GetRemarkByIdAsync(newRemark.Id);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result!.Id, Is.EqualTo(newRemark.Id));
            Assert.That(result.AddedOn, Is.EqualTo(newRemark.AddedOn));
            Assert.That(result.SubjectId, Is.EqualTo(newRemark.SubjectId));
            Assert.That(result.StudentId, Is.EqualTo(newRemark.StudentId));
            Assert.That(result.RemarkText, Is.EqualTo(newRemark.RemarkText));
        });
    }

    [Test]
    public async Task GetRemarkByIdAsync_WithZeroId_ShouldReturnNull()
    {
        // Act
        var result = await _diaryService.GetRemarkByIdAsync(0);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetRemarkByIdAsync_WithNegativeId_ShouldReturnNull()
    {
        // Act
        var result = await _diaryService.GetRemarkByIdAsync(-1);

        // Assert
        Assert.That(result, Is.Null);
    }

    //----------------EditRemarkAsync---------------------
    [Test]
    public async Task EditRemarkAsync_WithValidData_ShouldUpdateRemark()
    {
        // Arrange
        var model = new EditRemarkViewModel
        {
            Id = _testRemark1.Id,
            AddedOn = DateTime.Now,
            RemarkText = "Updated remark text",
            SubjectId = _testRemark1.SubjectId,
            StudentId = _testRemark1.StudentId
        };

        // Act
        bool result = await _diaryService.EditRemarkAsync(model);

        // Assert
        Assert.That(result, Is.True);

        var updatedRemark = await _dbContext.Remarks
            .FirstOrDefaultAsync(r => r.Id == model.Id);

        Assert.That(updatedRemark, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(updatedRemark!.RemarkText, Is.EqualTo(model.RemarkText));
            Assert.That(updatedRemark.AddedOn, Is.EqualTo(model.AddedOn));

            Assert.That(updatedRemark.SubjectId, Is.EqualTo(_testRemark1.SubjectId));
            Assert.That(updatedRemark.StudentId, Is.EqualTo(_testRemark1.StudentId));
            Assert.That(updatedRemark.TeacherId, Is.EqualTo(_testRemark1.TeacherId));
        });
    }

    [Test]
    public async Task EditRemarkAsync_WithNonExistentId_ShouldReturnFalse()
    {
        // Arrange
        var model = new EditRemarkViewModel
        {
            Id = 999,
            AddedOn = DateTime.Now,
            RemarkText = "This should not be updated",
            SubjectId = _testRemark1.SubjectId,
            StudentId = _testRemark1.StudentId
        };

        // Act
        bool result = await _diaryService.EditRemarkAsync(model);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public async Task EditRemarkAsync_ShouldOnlyUpdateSpecificRemark()
    {
        // Arrange
        var originalText2 = _testRemark2.RemarkText;
        var originalText3 = _testRemark3.RemarkText;

        var model = new EditRemarkViewModel
        {
            Id = _testRemark1.Id,
            AddedOn = DateTime.Now,
            RemarkText = "Updated text for remark 1",
            SubjectId = _testRemark1.SubjectId,
            StudentId = _testRemark1.StudentId
        };

        // Act
        bool result = await _diaryService.EditRemarkAsync(model);

        // Assert
        Assert.That(result, Is.True);

        var remarks = await _dbContext.Remarks.ToListAsync();
        Assert.Multiple(() =>
        {
            Assert.That(remarks.First(r => r.Id == _testRemark1.Id).RemarkText, Is.EqualTo(model.RemarkText));
            Assert.That(remarks.First(r => r.Id == _testRemark2.Id).RemarkText, Is.EqualTo(originalText2));
            Assert.That(remarks.First(r => r.Id == _testRemark3.Id).RemarkText, Is.EqualTo(originalText3));
        });
    }

    [Test]
    public async Task EditRemarkAsync_WithNullModel_ShouldReturnFalse()
    {
        // Act
        bool result = await _diaryService.EditRemarkAsync(null);

        // Assert
        Assert.That(result, Is.False);

        var remarks = await _dbContext.Remarks.ToListAsync();
        Assert.That(remarks, Has.Count.EqualTo(3));
    }

    [Test]
    public async Task EditRemarkAsync_ShouldPreserveOtherProperties()
    {
        // Arrange
        var originalRemark = _testRemark1;
        var model = new EditRemarkViewModel
        {
            Id = originalRemark.Id,
            AddedOn = DateTime.Now,
            RemarkText = "Updated text",
            SubjectId = originalRemark.SubjectId,
            StudentId = originalRemark.StudentId
        };

        // Act
        bool result = await _diaryService.EditRemarkAsync(model);

        // Assert
        Assert.That(result, Is.True);

        var updatedRemark = await _dbContext.Remarks
            .Include(r => r.Teacher)
            .Include(r => r.Subject)
            .Include(r => r.Student)
            .FirstOrDefaultAsync(r => r.Id == model.Id);

        Assert.That(updatedRemark, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(updatedRemark!.TeacherId, Is.EqualTo(originalRemark.TeacherId));
            Assert.That(updatedRemark.SubjectId, Is.EqualTo(originalRemark.SubjectId));
            Assert.That(updatedRemark.StudentId, Is.EqualTo(originalRemark.StudentId));
            Assert.That(updatedRemark.Subject, Is.Not.Null);
            Assert.That(updatedRemark.Teacher, Is.Not.Null);
            Assert.That(updatedRemark.Student, Is.Not.Null);
        });
    }

    //----------------GetSubjectsAsync---------------------
    [Test]
    public async Task GetSubjectsAsync_ShouldReturnAllSubjects()
    {
        // Act
        var result = await _diaryService.GetSubjectsAsync();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(2));

        var subjects = result.ToList();
        Assert.Multiple(() =>
        {
            Assert.That(subjects[0].Id, Is.EqualTo(_testSubject.Id));
            Assert.That(subjects[0].Name, Is.EqualTo(_testSubject.Name));
            Assert.That(subjects[1].Id, Is.EqualTo(_testSubject2.Id));
            Assert.That(subjects[1].Name, Is.EqualTo(_testSubject2.Name));
        });
    }

    [Test]
    public async Task GetSubjectsAsync_WhenNoSubjectsExist_ShouldReturnEmptyCollection()
    {
        // Arrange
        _dbContext.Grades.RemoveRange(_dbContext.Grades);
        _dbContext.Absences.RemoveRange(_dbContext.Absences);
        _dbContext.Remarks.RemoveRange(_dbContext.Remarks);
        _dbContext.Subjects.RemoveRange(_dbContext.Subjects);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _diaryService.GetSubjectsAsync();

        // Assert
        Assert.That(result, Is.Empty);
    }

    //----------------GetStudentsAsync---------------------
    [Test]
    public async Task GetStudentsAsync_ShouldReturnAllStudentsInClass()
    {
        // Arrange
        var model = new RemarkFormModel
        {
            StudentId = _testStudent1.Id
        };

        // Act
        var result = await _diaryService.GetStudentsAsync(model);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(2));

        var students = result.ToList();
        Assert.Multiple(() =>
        {
            Assert.That(students[0].Id, Is.EqualTo(_testStudent1.Id));
            Assert.That(students[0].FirstName, Is.EqualTo(_testStudent1.FirstName));
            Assert.That(students[0].LastName, Is.EqualTo(_testStudent1.LastName));

            Assert.That(students[1].Id, Is.EqualTo(_testStudent2.Id));
            Assert.That(students[1].FirstName, Is.EqualTo(_testStudent2.FirstName));
            Assert.That(students[1].LastName, Is.EqualTo(_testStudent2.LastName));
        });
    }

    [Test]
    public async Task GetStudentsAsync_WithNonExistentStudentId_ShouldReturnEmptyList()
    {
        // Arrange
        var model = new RemarkFormModel
        {
            StudentId = 999
        };

        // Act
        var result = await _diaryService.GetStudentsAsync(model);

        // Assert
        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task GetStudentsAsync_ShouldReturnOnlyStudentsFromSameClass()
    {
        // Arrange
        var studentInDifferentClass = new Student
        {
            Id = 3,
            FirstName = "Different",
            MiddleName = "MiddleDifferent",
            LastName = "Class",
            ClassId = _testClass2.Id,
            ApplicationUserId = Guid.NewGuid()
        };
        await _dbContext.Students.AddAsync(studentInDifferentClass);
        await _dbContext.SaveChangesAsync();

        var model = new RemarkFormModel
        {
            StudentId = _testStudent1.Id
        };

        // Act
        var result = await _diaryService.GetStudentsAsync(model);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(2));
        Assert.That(result.Any(s => s.Id == studentInDifferentClass.Id), Is.False);
    }

    [Test]
    public async Task GetStudentsAsync_WithNullModel_ShouldReturnEmptyList()
    {
        // Act
        var result = await _diaryService.GetStudentsAsync(null);

        // Assert
        Assert.That(result, Is.Empty);
    }

    //----------------GetGradeTypes---------------------
    [Test]
    public void GetGradeTypes_ShouldReturnAllGradeTypes()
    {
        // Act
        var result = _diaryService.GetGradeTypes();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(4));
    }

    [Test]
    public void GetGradeTypes_ShouldReturnCorrectValuesAndText()
    {
        // Act
        var gradeTypes = _diaryService.GetGradeTypes().ToList();

        // Assert
        Assert.Multiple(() =>
        {
            var current = gradeTypes.FirstOrDefault(gt => gt.Value == GradeType.Current.ToString());
            Assert.That(current, Is.Not.Null);
            Assert.That(current!.Text, Is.EqualTo("Текуща оценка"));

            var firstTerm = gradeTypes.FirstOrDefault(gt => gt.Value == GradeType.FirstTerm.ToString());
            Assert.That(firstTerm, Is.Not.Null);
            Assert.That(firstTerm!.Text, Is.EqualTo("Първи срок"));

            var secondTerm = gradeTypes.FirstOrDefault(gt => gt.Value == GradeType.SecondTerm.ToString());
            Assert.That(secondTerm, Is.Not.Null);
            Assert.That(secondTerm!.Text, Is.EqualTo("Втори срок"));

            var yearly = gradeTypes.FirstOrDefault(gt => gt.Value == GradeType.Yearly.ToString());
            Assert.That(yearly, Is.Not.Null);
            Assert.That(yearly!.Text, Is.EqualTo("Годишна оценка"));
        });
    }

    [Test]
    public void GetGradeTypes_ShouldReturnSelectListItems()
    {
        // Act
        var result = _diaryService.GetGradeTypes();

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.All(item => item is not null), Is.True);
            Assert.That(result.All(item => !string.IsNullOrEmpty(item.Value)), Is.True);
            Assert.That(result.All(item => !string.IsNullOrEmpty(item.Text)), Is.True);
        });
    }

    [Test]
    public void GetGradeTypes_OrderShouldMatchEnumOrder()
    {
        // Act
        var gradeTypes = _diaryService.GetGradeTypes().ToList();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(gradeTypes[0].Value, Is.EqualTo(GradeType.Current.ToString()));
            Assert.That(gradeTypes[1].Value, Is.EqualTo(GradeType.FirstTerm.ToString()));
            Assert.That(gradeTypes[2].Value, Is.EqualTo(GradeType.SecondTerm.ToString()));
            Assert.That(gradeTypes[3].Value, Is.EqualTo(GradeType.Yearly.ToString()));
        });
    }
}
