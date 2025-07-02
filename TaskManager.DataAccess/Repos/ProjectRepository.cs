using Microsoft.EntityFrameworkCore;
using TaskManager.DataAccess.Repository;
using TaskManager.Entities.Project;

namespace TaskManager.DataAccess.Repos;

public class ProjectRepository : CrudRepository<Project>, IProjectRepository
{
    private readonly AppDbContext _context;

    public ProjectRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<Project>> GetProjectWithTasks(long projectId)
    {
        return await _context.Projects
            .Include(p => p.Tasks)
            .Where(p=>p.Id == projectId)
            .ToListAsync();
    }

    public async Task<List<Project>> GetProjectWithTasksByName(string projectName)
    {
        return await _context.Projects
            .Include(p => p.Tasks)
            .Where(p=>p.Title == projectName)
            .ToListAsync();
    }

    public async Task<Project?> GetProjectByIdAsync(long projectId)
    {
        return await _context.Projects.FirstOrDefaultAsync(p => p.Id == projectId);
    }


    public async Task<List<Project>> GetAllProjectsAsync()
    {
        return await _context.Projects.ToListAsync();
    }


    public async Task<bool> IsProjectExistByNameAsync(string projectName)
    {
        return await _context.Projects.AnyAsync(p => p.Title == projectName);
    }

    public async Task<List<Project>> GetProjectsByUserId(long userId)
    {
        return await _context.Projects
            .Where(p => p.Team.TeamAssignments.Any(ta => ta.Member_id == userId))
            .ToListAsync();
    }

    public async Task<List<Project>> SearchProjects(string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
            return await _context.Projects.ToListAsync();
        return await _context.Projects
            .Where(p => p.Title.ToLower().Contains(keyword.ToLower()) ||
                         (p.Description != null && p.Description.ToLower().Contains(keyword.ToLower())))
            .ToListAsync();
    }
}