using TaskManager.Entities.DTOs;

namespace TaskManager.Business;

public interface ITeamService
{
    Task CreateTeamAsync(CreateTeamDto dto);
    Task InviteMemberAsync(InviteMemberDto dto);
    Task RemoveMemberAsync(RemoveMemberDto dto);
    Task<Teams?> GetTeamWithMembersAsync(long teamId);
    Task<List<Teams>> GetTeamsManagedByUserAsync(long userId);
    Task<List<Teams>> SearchTeamsByNameOrDescriptionAsync(string keyword);
    Task<List<User>> GetUsersByTeamIdAsync(long teamId);
}
