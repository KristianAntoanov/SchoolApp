using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using SchoolApp.Data;
using SchoolApp.Data.Models;
using SchoolApp.Data.Repository;
using SchoolApp.Data.Repository.Contracts;
using SchoolApp.Services.Data;

using static SchoolApp.Common.ApplicationConstants;
using static SchoolApp.Common.TempDataMessages.UserRoles;

namespace SchoolApp.Services.Tests;


[TestFixture]
public class AdminUserRolesServiceTests
{
    private ApplicationDbContext _dbContext;
    private IRepository _repository;
    private Mock<UserManager<ApplicationUser>> _userManagerMock;
    private Mock<RoleManager<ApplicationRole>> _roleManagerMock;
    private AdminUserRolesService _userRolesService;

    private ApplicationUser _testUser;
    private ApplicationRole _testRole;
    private Teacher _testTeacher;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestUserRolesDb")
            .Options;

        _dbContext = new ApplicationDbContext(options);
        _repository = new BaseRepository(_dbContext);

        // Setup UserManager mock
        var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
        _userManagerMock = new Mock<UserManager<ApplicationUser>>(
            userStoreMock.Object, null, null, null, null, null, null, null, null);

        // Setup RoleManager mock
        var roleStoreMock = new Mock<IRoleStore<ApplicationRole>>();
        _roleManagerMock = new Mock<RoleManager<ApplicationRole>>(
            roleStoreMock.Object, null, null, null, null);

        _userRolesService = new AdminUserRolesService(
            _repository,
            _userManagerMock.Object,
            _roleManagerMock.Object);

        // Setup test data
        _testRole = new ApplicationRole
        {
            Id = Guid.NewGuid(),
            Name = "TestRole",
            NormalizedName = "TESTROLE"
        };

        _testUser = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            UserName = "testuser@test.com",
            NormalizedUserName = "TESTUSER@TEST.COM",
            Email = "testuser@test.com",
            NormalizedEmail = "TESTUSER@TEST.COM"
        };

        _testTeacher = new Teacher
        {
            GuidId = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe",
            ImageUrl = "Test.url",
            JobTitle = "Math Teacher",
            ApplicationUserId = _testUser.Id
        };

        _dbContext.Users.Add(_testUser);
        _dbContext.Teachers.Add(_testTeacher);
        _dbContext.SaveChanges();

        _userManagerMock.Setup(x => x.GetRolesAsync(It.IsAny<ApplicationUser>()))
             .ReturnsAsync(new List<string> { _testRole.Name! });

        _userManagerMock.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(_testUser);

        _userManagerMock.Setup(x => x.IsInRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            .ReturnsAsync(false);

        _userManagerMock.Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);

        _userManagerMock.Setup(x => x.RemoveFromRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);

        _userManagerMock.Setup(x => x.AddToRolesAsync(It.IsAny<ApplicationUser>(), It.IsAny<IEnumerable<string>>()))
            .ReturnsAsync(IdentityResult.Success);

        _userManagerMock.Setup(x => x.RemoveFromRolesAsync(It.IsAny<ApplicationUser>(), It.IsAny<IEnumerable<string>>()))
            .ReturnsAsync(IdentityResult.Success);

        // Setup RoleManager mock responses
        _roleManagerMock.Setup(x => x.Roles)
            .Returns(new[] { _testRole }.AsQueryable());
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    //----------------GetPagedUsersWithRolesAsync---------------------
    [Test]
    public async Task GetPagedUsersWithRolesAsync_ShouldReturnCorrectPaginatedData()
    {
        // Act
        var result = await _userRolesService.GetPagedUsersWithRolesAsync(1, 10);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Items.Count, Is.EqualTo(1));
            Assert.That(result.PageNumber, Is.EqualTo(1));
            Assert.That(result.TotalPages, Is.EqualTo(1));
            Assert.That(result.TotalItems, Is.EqualTo(1));
        });

        var user = result.Items.First();
        Assert.Multiple(() =>
        {
            Assert.That(user.Id, Is.EqualTo(_testUser.Id));
            Assert.That(user.Username, Is.EqualTo(_testUser.UserName));
            Assert.That(user.Email, Is.EqualTo(_testUser.Email));
            Assert.That(user.TeacherId, Is.EqualTo(_testTeacher.GuidId));
            Assert.That(user.UserRoles, Is.Not.Empty);
            Assert.That(user.UserRoles.First(), Is.EqualTo(_testRole.Name));
        });
    }

    [Test]
    public async Task GetPagedUsersWithRolesAsync_ShouldHandlePaginationCorrectly()
    {
        // Arrange
        for (int i = 0; i < 15; i++)
        {
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                UserName = $"testuser{i}@test.com",
                NormalizedUserName = $"TESTUSER{i}@TEST.COM",
                Email = $"testuser{i}@test.com",
                NormalizedEmail = $"TESTUSER{i}@TEST.COM"
            };
            await _dbContext.Users.AddAsync(user);
        }
        await _dbContext.SaveChangesAsync();

        // Act
        var firstPage = await _userRolesService.GetPagedUsersWithRolesAsync(1, 10);
        var secondPage = await _userRolesService.GetPagedUsersWithRolesAsync(2, 10);

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
    public async Task GetPagedUsersWithRolesAsync_ShouldHandleUserWithoutTeacher()
    {
        // Arrange
        var userWithoutTeacher = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            UserName = "noteacher@test.com",
            NormalizedUserName = "NOTEACHER@TEST.COM",
            Email = "noteacher@test.com",
            NormalizedEmail = "NOTEACHER@TEST.COM"
        };
        await _dbContext.Users.AddAsync(userWithoutTeacher);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _userRolesService.GetPagedUsersWithRolesAsync(1, 10);

        // Assert
        Assert.That(result.Items.Count, Is.EqualTo(2));

        var userWithoutTeacherResult = result.Items.First(u => u.Id == userWithoutTeacher.Id);
        Assert.That(userWithoutTeacherResult.TeacherId, Is.EqualTo(Guid.Empty));
    }

    [Test]
    public async Task GetPagedUsersWithRolesAsync_ShouldHandleUserWithoutRoles()
    {
        // Arrange
        _userManagerMock.Setup(x => x.GetRolesAsync(It.Is<ApplicationUser>(u => u.Id == _testUser.Id)))
            .ReturnsAsync(new List<string>());

        // Act
        var result = await _userRolesService.GetPagedUsersWithRolesAsync(1, 10);

        // Assert
        Assert.That(result.Items.First().UserRoles, Is.Empty);
    }
    //----------------GetPagedUsersWithRolesAsync---------------------
    //----------------UpdateUserTeacherAsync---------------------
    [Test]
    public async Task UpdateUserTeacherAsync_WhenUserNotFound_ShouldReturnFalse()
    {
        // Arrange
        _userManagerMock.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((ApplicationUser)null!);

        // Act
        var (success, message) = await _userRolesService.UpdateUserTeacherAsync(Guid.NewGuid(), Guid.NewGuid());

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(success, Is.False);
            Assert.That(message, Is.EqualTo(UserNotFoundMessage));
        });
    }

    [Test]
    public async Task UpdateUserTeacherAsync_WhenAssigningTeacher_ShouldAddTeacherRole()
    {
        // Arrange
        var newTeacher = new Teacher
        {
            GuidId = Guid.NewGuid(),
            FirstName = "Jane",
            LastName = "Smith",
            JobTitle = "Physics Teacher",
            ImageUrl = "Test2.url"
        };
        await _dbContext.Teachers.AddAsync(newTeacher);
        await _dbContext.SaveChangesAsync();

        // Act
        var (success, message) = await _userRolesService.UpdateUserTeacherAsync(_testUser.Id, newTeacher.GuidId);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(success, Is.True);
            Assert.That(message, Is.EqualTo(TeacherLinkUpdateSuccessMessage));
        });

        _userManagerMock.Verify(x => x.AddToRoleAsync(
            It.IsAny<ApplicationUser>(),
            It.Is<string>(role => role == TeacherRole)),
            Times.Once);
    }

    [Test]
    public async Task UpdateUserTeacherAsync_WhenRemovingTeacher_ShouldRemoveTeacherRole()
    {
        // Act
        var (success, message) = await _userRolesService.UpdateUserTeacherAsync(_testUser.Id, null);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(success, Is.True);
            Assert.That(message, Is.EqualTo(TeacherLinkUpdateSuccessMessage));
        });

        _userManagerMock.Verify(x => x.RemoveFromRoleAsync(
            It.IsAny<ApplicationUser>(),
            It.Is<string>(role => role == TeacherRole)),
            Times.Once);
    }

    [Test]
    public async Task UpdateUserTeacherAsync_WhenUpdatingExistingTeacher_ShouldUpdateCorrectly()
    {
        // Arrange
        var newTeacher = new Teacher
        {
            GuidId = Guid.NewGuid(),
            FirstName = "Jane",
            LastName = "Smith",
            JobTitle = "Physics Teacher",
            ImageUrl = "Test2.url"
        };
        await _dbContext.Teachers.AddAsync(newTeacher);
        await _dbContext.SaveChangesAsync();

        // Act
        var (success, message) = await _userRolesService.UpdateUserTeacherAsync(_testUser.Id, newTeacher.GuidId);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(success, Is.True);
            Assert.That(message, Is.EqualTo(TeacherLinkUpdateSuccessMessage));
        });

        var oldTeacher = await _dbContext.Teachers.FindAsync(_testTeacher.GuidId);
        var updatedTeacher = await _dbContext.Teachers.FindAsync(newTeacher.GuidId);

        Assert.Multiple(() =>
        {
            Assert.That(oldTeacher!.ApplicationUserId, Is.Null);
            Assert.That(updatedTeacher!.ApplicationUserId, Is.EqualTo(_testUser.Id));
        });
    }

    [Test]
    public async Task UpdateUserTeacherAsync_WhenTeacherNotFound_ShouldStillRemoveOldTeacherLink()
    {
        // Arrange
        var nonExistentTeacherId = Guid.NewGuid();

        // Act
        var (success, message) = await _userRolesService.UpdateUserTeacherAsync(_testUser.Id, nonExistentTeacherId);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(success, Is.True);
            Assert.That(message, Is.EqualTo(TeacherLinkUpdateSuccessMessage));
        });

        var oldTeacher = await _dbContext.Teachers.FindAsync(_testTeacher.GuidId);
        Assert.That(oldTeacher!.ApplicationUserId, Is.Null);
    }

    [Test]
    public async Task UpdateUserTeacherAsync_WhenUserAlreadyHasTeacherRole_ShouldNotAddRoleAgain()
    {
        // Arrange
        _userManagerMock.Setup(x => x.IsInRoleAsync(It.IsAny<ApplicationUser>(), TeacherRole))
            .ReturnsAsync(true);

        var newTeacher = new Teacher
        {
            GuidId = Guid.NewGuid(),
            FirstName = "Jane",
            LastName = "Smith",
            JobTitle = "Physics Teacher",
            ImageUrl = "Test2.url"
        };
        await _dbContext.Teachers.AddAsync(newTeacher);
        await _dbContext.SaveChangesAsync();

        // Act
        var (success, message) = await _userRolesService.UpdateUserTeacherAsync(_testUser.Id, newTeacher.GuidId);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(success, Is.True);
            Assert.That(message, Is.EqualTo(TeacherLinkUpdateSuccessMessage));
        });

        _userManagerMock.Verify(x => x.AddToRoleAsync(
            It.IsAny<ApplicationUser>(),
            It.IsAny<string>()),
            Times.Never);
    }
    //----------------UpdateUserTeacherAsync---------------------
    //----------------UpdateUserRolesAsync---------------------
    [Test]
    public async Task UpdateUserRolesAsync_WhenUserNotFound_ShouldReturnFalse()
    {
        // Arrange
        _userManagerMock.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((ApplicationUser)null!);

        // Act
        var (success, message) = await _userRolesService.UpdateUserRolesAsync(
            Guid.NewGuid(),
            new List<string> { "NewRole" });

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(success, Is.False);
            Assert.That(message, Is.EqualTo(UserNotFoundMessage));
        });
    }

    [Test]
    public async Task UpdateUserRolesAsync_WhenRemovingAllRoles_ShouldSucceed()
    {
        // Arrange
        _userManagerMock.Setup(x => x.GetRolesAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(new List<string> { "Role1", "Role2" });

        _userManagerMock.Setup(x => x.RemoveFromRolesAsync(
            It.IsAny<ApplicationUser>(),
            It.IsAny<IEnumerable<string>>()))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var (success, message) = await _userRolesService.UpdateUserRolesAsync(
            _testUser.Id,
            new List<string>());

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(success, Is.True);
            Assert.That(message, Is.EqualTo(RolesUpdateSuccessMessage));
        });

        _userManagerMock.Verify(x => x.RemoveFromRolesAsync(
            It.IsAny<ApplicationUser>(),
            It.Is<IEnumerable<string>>(roles => roles.Contains("Role1") && roles.Contains("Role2"))),
            Times.Once);

        _userManagerMock.Verify(x => x.AddToRolesAsync(
            It.IsAny<ApplicationUser>(),
            It.IsAny<IEnumerable<string>>()),
            Times.Never);
    }

    [Test]
    public async Task UpdateUserRolesAsync_WhenAddingNewRoles_ShouldSucceed()
    {
        // Arrange
        var currentRoles = new List<string> { "OldRole" };
        var newRoles = new List<string> { "NewRole1", "NewRole2" };

        _userManagerMock.Setup(x => x.GetRolesAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(currentRoles);

        _userManagerMock.Setup(x => x.RemoveFromRolesAsync(
            It.IsAny<ApplicationUser>(),
            It.IsAny<IEnumerable<string>>()))
            .ReturnsAsync(IdentityResult.Success);

        _userManagerMock.Setup(x => x.AddToRolesAsync(
            It.IsAny<ApplicationUser>(),
            It.IsAny<IEnumerable<string>>()))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var (success, message) = await _userRolesService.UpdateUserRolesAsync(_testUser.Id, newRoles);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(success, Is.True);
            Assert.That(message, Is.EqualTo(RolesUpdateSuccessMessage));
        });

        _userManagerMock.Verify(x => x.RemoveFromRolesAsync(
            It.IsAny<ApplicationUser>(),
            It.Is<IEnumerable<string>>(roles => roles.Single() == "OldRole")),
            Times.Once);

        _userManagerMock.Verify(x => x.AddToRolesAsync(
            It.IsAny<ApplicationUser>(),
            It.Is<IEnumerable<string>>(roles =>
                roles.Contains("NewRole1") &&
                roles.Contains("NewRole2"))),
            Times.Once);
    }

    [Test]
    public async Task UpdateUserRolesAsync_WhenAddToRolesFails_ShouldAttemptToRestoreOldRoles()
    {
        // Arrange
        var currentRoles = new List<string> { "OldRole1", "OldRole2" };
        _userManagerMock.Setup(x => x.GetRolesAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(currentRoles);

        _userManagerMock.Setup(x => x.RemoveFromRolesAsync(
            It.IsAny<ApplicationUser>(),
            It.IsAny<IEnumerable<string>>()))
            .ReturnsAsync(IdentityResult.Success);

        _userManagerMock.Setup(x => x.AddToRolesAsync(
            It.IsAny<ApplicationUser>(),
            It.Is<IEnumerable<string>>(roles => roles.Contains("NewRole"))))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError
            {
                Description = "Failed to add roles"
            }));

        _userManagerMock.Setup(x => x.AddToRolesAsync(
            It.IsAny<ApplicationUser>(),
            It.Is<IEnumerable<string>>(roles =>
                roles.Contains("OldRole1") &&
                roles.Contains("OldRole2"))))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var (success, message) = await _userRolesService.UpdateUserRolesAsync(_testUser.Id,
            new List<string> { "NewRole" });

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(success, Is.False);
            Assert.That(message, Is.EqualTo(RolesUpdateErrorMessage));
        });

        // Verify that we tried to restore the old roles
        _userManagerMock.Verify(x => x.AddToRolesAsync(
            It.IsAny<ApplicationUser>(),
            It.Is<IEnumerable<string>>(roles =>
                roles.Contains("OldRole1") &&
                roles.Contains("OldRole2"))),
            Times.Once);
    }

    [Test]
    public async Task UpdateUserRolesAsync_WhenAddToRolesFails_ShouldReturnFalse()
    {
        // Arrange
        _userManagerMock.Setup(x => x.GetRolesAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(new List<string> { "OldRole" });

        _userManagerMock.Setup(x => x.RemoveFromRolesAsync(
            It.IsAny<ApplicationUser>(),
            It.IsAny<IEnumerable<string>>()))
            .ReturnsAsync(IdentityResult.Success);

        _userManagerMock.Setup(x => x.AddToRolesAsync(
            It.IsAny<ApplicationUser>(),
            It.IsAny<IEnumerable<string>>()))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError
            {
                Description = "Failed to add roles"
            }));


        // Act
        var (success, message) = await _userRolesService.UpdateUserRolesAsync(_testUser.Id,
            new List<string> { "NewRole" });

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(success, Is.False);
            Assert.That(message, Is.EqualTo(RolesUpdateErrorMessage));
        });
    }

    [Test]
    public async Task UpdateUserRolesAsync_WhenRolesListIsNull_ShouldHandleGracefully()
    {
        // Arrange
        _userManagerMock.Setup(x => x.GetRolesAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(new List<string> { "CurrentRole" });

        _userManagerMock.Setup(x => x.RemoveFromRolesAsync(
            It.IsAny<ApplicationUser>(),
            It.IsAny<IEnumerable<string>>()))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _userRolesService.UpdateUserRolesAsync(_testUser.Id, new List<string>()); // Променяме тук

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.success, Is.True);
            Assert.That(result.message, Is.EqualTo(RolesUpdateSuccessMessage));
        });

        _userManagerMock.Verify(x => x.RemoveFromRolesAsync(
            It.IsAny<ApplicationUser>(),
            It.IsAny<IEnumerable<string>>()),
            Times.Once);

        _userManagerMock.Verify(x => x.AddToRolesAsync(
            It.IsAny<ApplicationUser>(),
            It.IsAny<IEnumerable<string>>()),
            Times.Never);
    }
    //----------------UpdateUserRolesAsync---------------------

    //----------------UpdateTeacherUserAsync---------------------
    [Test]
    public async Task UpdateTeacherUserAsync_WhenOnlyRemovingLink_ShouldSucceed()
    {
        // Arrange
        Guid userId = _testUser.Id;
        Guid? teacherId = null;

        // Act
        bool result = await _userRolesService.UpdateTeacherUserAsync(userId, teacherId);

        // Assert
        Assert.That(result, Is.True);

        var oldTeacher = await _dbContext.Teachers.FindAsync(_testTeacher.GuidId);
        Assert.That(oldTeacher!.ApplicationUserId, Is.Null);
    }

    [Test]
    public async Task UpdateTeacherUserAsync_WhenChangingTeacher_ShouldUpdateCorrectly()
    {
        // Arrange
        var newTeacher = new Teacher
        {
            GuidId = Guid.NewGuid(),
            FirstName = "Jane",
            LastName = "Smith",
            JobTitle = "Physics Teacher",
            ImageUrl = "Test2.url"
        };
        await _dbContext.Teachers.AddAsync(newTeacher);
        await _dbContext.SaveChangesAsync();

        // Act
        bool result = await _userRolesService.UpdateTeacherUserAsync(_testUser.Id, newTeacher.GuidId);

        // Assert
        Assert.That(result, Is.True);

        var oldTeacher = await _dbContext.Teachers.FindAsync(_testTeacher.GuidId);
        var updatedTeacher = await _dbContext.Teachers.FindAsync(newTeacher.GuidId);

        Assert.Multiple(() =>
        {
            Assert.That(oldTeacher!.ApplicationUserId, Is.Null);
            Assert.That(updatedTeacher!.ApplicationUserId, Is.EqualTo(_testUser.Id));
        });
    }

    [Test]
    public async Task UpdateTeacherUserAsync_WhenTeacherDoesNotExist_ShouldRemoveOldLinkAndReturnTrue()
    {
        // Arrange
        var nonExistentTeacherId = Guid.NewGuid();

        // Act
        bool result = await _userRolesService.UpdateTeacherUserAsync(_testUser.Id, nonExistentTeacherId);

        // Assert
        Assert.That(result, Is.True);

        var oldTeacher = await _dbContext.Teachers.FindAsync(_testTeacher.GuidId);
        Assert.That(oldTeacher!.ApplicationUserId, Is.Null);
    }

    [Test]
    public async Task UpdateTeacherUserAsync_WhenNoCurrentTeacherExists_ShouldAddNewLink()
    {
        // Arrange
        var userWithoutTeacher = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            UserName = "noteacher@test.com",
            Email = "noteacher@test.com"
        };
        await _dbContext.Users.AddAsync(userWithoutTeacher);

        var newTeacher = new Teacher
        {
            GuidId = Guid.NewGuid(),
            FirstName = "Jane",
            LastName = "Smith",
            JobTitle = "Physics Teacher",
            ImageUrl = "Test2.url"
        };
        await _dbContext.Teachers.AddAsync(newTeacher);
        await _dbContext.SaveChangesAsync();

        // Act
        bool result = await _userRolesService.UpdateTeacherUserAsync(userWithoutTeacher.Id, newTeacher.GuidId);

        // Assert
        Assert.That(result, Is.True);

        var updatedTeacher = await _dbContext.Teachers.FindAsync(newTeacher.GuidId);
        Assert.That(updatedTeacher!.ApplicationUserId, Is.EqualTo(userWithoutTeacher.Id));
    }
    //----------------UpdateTeacherUserAsync---------------------
    //----------------GetTeacherIdByUserIdAsync---------------------
    [Test]
    public async Task GetTeacherIdByUserIdAsync_WhenTeacherExists_ShouldReturnTeacherId()
    {
        // Act
        var result = await _userRolesService.GetTeacherIdByUserIdAsync(_testUser.Id);

        // Assert
        Assert.That(result, Is.EqualTo(_testTeacher.GuidId));
    }

    [Test]
    public async Task GetTeacherIdByUserIdAsync_WhenNoTeacherExists_ShouldReturnEmptyGuid()
    {
        // Arrange
        var userWithoutTeacher = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            UserName = "noteacher@test.com",
            Email = "noteacher@test.com"
        };
        await _dbContext.Users.AddAsync(userWithoutTeacher);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _userRolesService.GetTeacherIdByUserIdAsync(userWithoutTeacher.Id);

        // Assert
        Assert.That(result, Is.EqualTo(Guid.Empty));
    }
    //----------------GetTeacherIdByUserIdAsync---------------------

    //----------------GetAvailableTeachersForAssignmentAsync---------------------
    [Test]
    public async Task GetAvailableTeachersForAssignmentAsync_ShouldReturnUnassignedTeachers()
    {
        // Arrange
        var unassignedTeacher = new Teacher
        {
            GuidId = Guid.NewGuid(),
            FirstName = "Jane",
            LastName = "Smith",
            JobTitle = "Physics Teacher",
            ImageUrl = "Test2.url"
        };
        await _dbContext.Teachers.AddAsync(unassignedTeacher);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _userRolesService.GetAvailableTeachersForAssignmentAsync();

        // Assert
        Assert.That(result.Count(), Is.EqualTo(1));
        var teacher = result.First();
        Assert.Multiple(() =>
        {
            Assert.That(teacher.Id, Is.EqualTo(unassignedTeacher.GuidId));
            Assert.That(teacher.DisplayName, Is.EqualTo($"{unassignedTeacher.FirstName} {unassignedTeacher.LastName} - {unassignedTeacher.JobTitle}"));
        });
    }

    [Test]
    public async Task GetAvailableTeachersForAssignmentAsync_WhenCurrentTeacherProvided_ShouldIncludeCurrentTeacher()
    {
        // Act
        var result = await _userRolesService.GetAvailableTeachersForAssignmentAsync(_testTeacher.GuidId);

        // Assert
        Assert.That(result.Count(), Is.EqualTo(1));
        var teacher = result.First();
        Assert.Multiple(() =>
        {
            Assert.That(teacher.Id, Is.EqualTo(_testTeacher.GuidId));
            Assert.That(teacher.DisplayName, Is.EqualTo($"{_testTeacher.FirstName} {_testTeacher.LastName} - {_testTeacher.JobTitle}"));
        });
    }
    //----------------GetAvailableTeachersForAssignmentAsync---------------------

    //----------------GetTeacherBasicInfoAsync---------------------
    [Test]
    public async Task GetTeacherBasicInfoAsync_WhenTeacherExists_ShouldReturnBasicInfo()
    {
        // Act
        var result = await _userRolesService.GetTeacherBasicInfoAsync(_testTeacher.GuidId);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result!.FirstName, Is.EqualTo(_testTeacher.FirstName));
            Assert.That(result.LastName, Is.EqualTo(_testTeacher.LastName));
            Assert.That(result.JobTitle, Is.EqualTo(_testTeacher.JobTitle));
        });
    }

    [Test]
    public async Task GetTeacherBasicInfoAsync_WhenTeacherDoesNotExist_ShouldReturnNull()
    {
        // Act
        var result = await _userRolesService.GetTeacherBasicInfoAsync(Guid.NewGuid());

        // Assert
        Assert.That(result, Is.Null);
    }
    //----------------GetTeacherBasicInfoAsync---------------------
}