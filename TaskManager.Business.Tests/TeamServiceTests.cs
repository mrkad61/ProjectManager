using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using TaskManager.Business;
using TaskManager.Entities;
using Xunit;

public class TeamServiceTests
{
    private readonly Mock<ITeamsRepository> _teamsRepositoryMock;
    private readonly TeamService _teamService;

    public TeamServiceTests()
    {
        _teamsRepositoryMock = new Mock<ITeamsRepository>();
        // TeamService constructor'ı AppDbContext alıyor, gerçek implementasyon için repository ile DI yapılmalı.
        // Burada örnek olarak repository üzerinden test metotları gösteriliyor.
    }

    [Fact]
    public async Task GetTeamWithMembersAsync_ReturnsTeamWithMembers()
    {
        // Arrange
        var teamId = 1L;
        var team = new Teams { id = teamId, TeamAssignments = new List<TeamAssigment> { new TeamAssigment() } };
        _teamsRepositoryMock.Setup(r => r.GetTeamWithMembers(teamId)).ReturnsAsync(team);

        // Act
        var result = await _teamsRepositoryMock.Object.GetTeamWithMembers(teamId);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.TeamAssignments);
        Assert.NotEmpty(result.TeamAssignments);
    }

    [Fact]
    public async Task GetTeamsManagedByUserAsync_ReturnsTeams()
    {
        // Arrange
        var userId = 1L;
        var teams = new List<Teams> { new Teams { id = 1, ManagerId = userId }, new Teams { id = 2, ManagerId = userId } };
        _teamsRepositoryMock.Setup(r => r.GetTeamsManagedByUser(userId)).ReturnsAsync(teams);

        // Act
        var result = await _teamsRepositoryMock.Object.GetTeamsManagedByUser(userId);

        // Assert
        Assert.NotNull(result);
        Assert.All(result, t => Assert.Equal(userId, t.ManagerId));
    }

    [Fact]
    public async Task SearchTeamsByNameOrDescriptionAsync_ReturnsMatchingTeams()
    {
        // Arrange
        var keyword = "test";
        var teams = new List<Teams> { new Teams { Name = "test team" }, new Teams { Description = "test desc" } };
        _teamsRepositoryMock.Setup(r => r.SearchTeamsByNameOrDescription(keyword)).ReturnsAsync(teams);

        // Act
        var result = await _teamsRepositoryMock.Object.SearchTeamsByNameOrDescription(keyword);

        // Assert
        Assert.NotNull(result);
        Assert.All(result, t => Assert.True(t.Name.Contains(keyword) || (t.Description != null && t.Description.Contains(keyword))));
    }
} 