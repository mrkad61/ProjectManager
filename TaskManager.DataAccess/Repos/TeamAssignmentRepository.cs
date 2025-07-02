using Microsoft.EntityFrameworkCore;
using TaskManager.DataAccess.Repository;
using TaskManager.Entities;

namespace TaskManager.DataAccess.Repos;

public class TeamAssignmentRepository : CrudRepository<TeamAssigment>, ITeamAssignmentRepository
{
    private readonly AppDbContext _context;
    public TeamAssignmentRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<TeamAssigment>> GetAssignmentsByUserId(long userId)
    {
        return await _context.TeamAssignments
            .Where(u => u.Member_id == userId)
            .ToListAsync();
    }

    public async Task<List<TeamAssigment>> GetAssignmentsByTeamId(long teamId)
    {
        return await _context.TeamAssignments
            .Where(t=>t.Team_id == teamId)
            .ToListAsync();
    }

    public async Task<TeamAssigment?> GetAssignment(long userId, long teamId)
    {
        return await _context.TeamAssignments
            .FirstOrDefaultAsync(ta => ta.Member_id == userId && 
                                       ta.Team_id == teamId);
    }

    public async Task RemoveUserFromAllTeams(long userId)
    {
        var assignments = await _context.TeamAssignments.Where(ta => ta.Member_id == userId).ToListAsync();
        _context.TeamAssignments.RemoveRange(assignments);
        await _context.SaveChangesAsync();
    }
}