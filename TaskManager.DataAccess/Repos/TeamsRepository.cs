using Microsoft.EntityFrameworkCore;
using TaskManager.DataAccess.Repository;
using TaskManager.Entities;

namespace TaskManager.DataAccess.Repos;

public class TeamsRepository : CrudRepository<Teams> , ITeamsRepository
{
    private readonly AppDbContext _context;
    public TeamsRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Teams?> GetTeamByName(string teamName)
    {
        return await _context.Teams.FirstOrDefaultAsync(t => t.Name == teamName);
    }

    public async Task<List<Teams>> SearchTeams(string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
            return await _context.Teams.ToListAsync();

        return await _context.Teams
            .Where(t => t.Name.ToLower().Contains(keyword.ToLower()))
            .ToListAsync();
    }

    public async Task<List<Teams>> GetTeamsPaged(int page, int pageSize)
    {
        return await _context.Teams
            .OrderBy(t => t.id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<List<User>> GetUsersByTeamId(long teamId)
    {
        return await _context.TeamAssignments
            .Where(ta => ta.Team_id == teamId)
            .Include(ta => ta.Member)
            .Select(ta => ta.Member)
            .ToListAsync();
    }

    public async Task<Teams?> GetTeamWithMembers(long teamId)
    {
        return await _context.Teams
            .Include(t => t.TeamAssignments)
            .ThenInclude(ta => ta.Member)
            .FirstOrDefaultAsync(t => t.id == teamId);
    }

    public async Task<List<Teams>> GetTeamsManagedByUser(long userId)
    {
        return await _context.Teams
            .Where(t => t.ManagerId == userId)
            .ToListAsync();
    }

    public async Task<List<Teams>> SearchTeamsByNameOrDescription(string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
            return await _context.Teams.ToListAsync();
        return await _context.Teams
            .Where(t => t.Name.ToLower().Contains(keyword.ToLower()) ||
                        t.Description.ToLower().Contains(keyword.ToLower()))
            .ToListAsync();
    }

    public async Task<List<User>> GetTeamAdmins(long teamId)
    {
        return await _context.TeamAssignments
            .Where(ta => ta.Team_id == teamId && ta.UserType == UserType.Admin)
            .Include(ta => ta.Member)
            .Select(ta => ta.Member)
            .ToListAsync();
    }

    public async Task<List<User>> GetTeamMembers(long teamId, UserType? role = null)
    {
        var query = _context.TeamAssignments
            .Where(ta => ta.Team_id == teamId)
            .Include(ta => ta.Member)
            .AsQueryable();
        if (role.HasValue)
            query = query.Where(ta => ta.UserType == role.Value);
        return await query.Select(ta => ta.Member).ToListAsync();
    }
}