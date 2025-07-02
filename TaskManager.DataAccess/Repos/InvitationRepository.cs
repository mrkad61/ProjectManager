using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskManager.DataAccess.Entities;
using TaskManager.DataAccess.Interfaces;

namespace TaskManager.DataAccess.Repos
{
    public class InvitationRepository : IInvitationRepository
    {
        private readonly TaskManagerContext _context;

        public InvitationRepository(TaskManagerContext context)
        {
            _context = context;
        }

        public async Task<List<Invitation>> GetInvitationsByUserId(long userId)
        {
            return await _context.Invitations
                .Where(i => i.User.Id == userId)
                .ToListAsync();
        }

        public async Task<List<Invitation>> GetPendingInvitationsByTeamId(long teamId)
        {
            return await _context.Invitations
                .Where(i => i.Teams.id == teamId && i.Status == InvitationStatus.Pending)
                .ToListAsync();
        }

        public async Task AcceptInvitation(long invitationId)
        {
            var invitation = await _context.Invitations.FindAsync(invitationId);
            if (invitation != null)
            {
                invitation.Status = InvitationStatus.Accepted;
                await _context.SaveChangesAsync();
            }
        }

        public async Task RejectInvitation(long invitationId)
        {
            var invitation = await _context.Invitations.FindAsync(invitationId);
            if (invitation != null)
            {
                invitation.Status = InvitationStatus.Rejected;
                await _context.SaveChangesAsync();
            }
        }
    }
} 