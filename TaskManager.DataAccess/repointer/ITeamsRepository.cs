using TaskManager.Entities;

namespace TaskManager.DataAccess.Repository;

public interface ITeamsRepository : ICrudRepository<Teams>
{
    Task<Teams?> GetTeamByName(string teamName);
    Task<List<Teams>> SearchTeams(string keyword);
    Task<List<Teams>> GetTeamsPaged(int page, int pageSize);
    Task<List<User>> GetUsersByTeamId(long teamId);
    Task<Teams?> GetTeamWithMembers(long teamId);
    Task<List<Teams>> GetTeamsManagedByUser(long userId);
    Task<List<Teams>> SearchTeamsByNameOrDescription(string keyword);
    Task<List<User>> GetTeamAdmins(long teamId);
    Task<List<User>> GetTeamMembers(long teamId, UserType? role = null);
}