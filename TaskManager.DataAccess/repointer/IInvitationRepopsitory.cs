using TaskManager.Entities;

namespace TaskManager.DataAccess.Repository;

public interface IInvitationRepository : ICrudRepository<Invitation>
{
    Task<List<Invitation>> GetInvitationsByUserId(long userId);
    Task<Invitation?> GetInvitationById(long invitationId);
    Task<List<Invitation>> GetPendingInvitations();
    Task<List<Invitation>> GetPendingInvitationsByTeamId(long teamId);
    Task AcceptInvitation(long invitationId);
    Task RejectInvitation(long invitationId);
}