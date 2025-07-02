using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using TaskManager.Business;
using TaskManager.DataAccess.Repository;
using TaskManager.Entities;
using TaskManager.Entities.Helper;
using Xunit;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _userService = new UserService(_userRepositoryMock.Object);
    }

    [Fact]
    public async Task GetUsersByRoleAsync_ReturnsUsersWithGivenRole()
    {
        // Arrange
        var role = UserType.Admin;
        var users = new List<User> { new User { Id = 1, Role = role }, new User { Id = 2, Role = role } };
        _userRepositoryMock.Setup(r => r.GetUsersByRole(role)).ReturnsAsync(users);

        // Act
        var result = await _userService.GetUsersByRoleAsync(role);

        // Assert
        Assert.NotNull(result);
        Assert.All(result, u => Assert.Equal(role, u.Role));
    }

    [Fact]
    public async Task GetUserWithTeamsAsync_ReturnsUserWithTeams()
    {
        // Arrange
        var userId = 1L;
        var user = new User { Id = userId, Teams = new List<TeamAssigment> { new TeamAssigment() } };
        _userRepositoryMock.Setup(r => r.GetUserWithTeams(userId)).ReturnsAsync(user);

        // Act
        var result = await _userService.GetUserWithTeamsAsync(userId);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Teams);
        Assert.NotEmpty(result.Teams);
    }

    [Fact]
    public async Task GetUserWithAssignmentsAsync_ReturnsUserWithAssignments()
    {
        // Arrange
        var userId = 1L;
        var user = new User { Id = userId, TaskAssignments = new List<TaskAssigment> { new TaskAssigment() } };
        _userRepositoryMock.Setup(r => r.GetUserWithAssignments(userId)).ReturnsAsync(user);

        // Act
        var result = await _userService.GetUserWithAssignmentsAsync(userId);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.TaskAssignments);
        Assert.NotEmpty(result.TaskAssignments);
    }
} 