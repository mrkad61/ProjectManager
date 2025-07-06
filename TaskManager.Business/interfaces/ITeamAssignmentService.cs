using System.Threading.Tasks;
using TaskManager.Business.DTOs.UserDTOs;

namespace TaskManager.Business
{
    public interface ITeamAssignmentService
    {
        Task RemoveUserFromAllTeamsAsync(long userId);
        Task<List<UserListDto>> GetTeamMembersAsync(long teamId);
        Task<bool> IsUserInTeamAsync(long userId, long teamId);
    }
} 