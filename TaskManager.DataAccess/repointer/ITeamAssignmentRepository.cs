using TaskManager.Entities;

namespace TaskManager.DataAccess.Repository;

public interface ITeamAssignmentRepository : ICrudRepository<TeamAssigment>
{
    Task<List<TeamAssigment>> GetAssignmentsByUserId(long userId);
    Task<List<TeamAssigment>> GetAssignmentsByTeamId(long teamId);
    Task<TeamAssigment?> GetAssignment(long userId, long teamId);
    Task RemoveUserFromAllTeams(long userId);
}
