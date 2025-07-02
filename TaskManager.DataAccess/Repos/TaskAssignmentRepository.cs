using Microsoft.EntityFrameworkCore;
using TaskManager.DataAccess.Repository;
using TaskManager.Entities.Project;

namespace TaskManager.DataAccess.Repos;

public class TaskAssignmentRepository : CrudRepository<TaskAssignment>, ITaskAssignmentRepository
{
    private readonly AppDbContext _context;

    public TaskAssignmentRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<TaskAssignment>> GetAssignmentsByUserId(long userId)
    {
        return await _context.TaskAssignments
            .Where(ta => ta.AssignedUserId == userId)
            .Include(ta => ta.TaskItem)
            .ToListAsync();
    }

    public async Task<List<TaskAssignment>> GetAssignmentsByTaskId(long taskId)
    {
        return await _context.TaskAssignments
            .Where(ta => ta.TaskItemId == taskId)
            .Include(ta => ta.AssignedUser)
            .ToListAsync();
    }

    public async Task<TaskAssignment?> GetAssignment(long userId, long taskId)
    {
        return await _context.TaskAssignments.FirstOrDefaultAsync(ta => ta.AssignedUserId == userId && ta.TaskItemId == taskId) ;
    }

    public async Task<bool> IsUserAssignedToTask(long userId, long taskId)
    {
        return await _context.TaskAssignments.AnyAsync(ta => ta.AssignedUserId == userId && ta.TaskItemId == taskId);
    }

    public async Task<List<TaskAssignment>> GetCompletedTasksByUserId(long userId)
    {
        return await _context.TaskAssignments
            .Where(ta => ta.AssignedUserId == userId && ta.IsCompleted)
            .ToListAsync();
    }

    public async Task<List<TaskAssignment>> GetPendingTasksByUserId(long userId)
    {
        return await _context.TaskAssignments
            .Where(ta => ta.AssignedUserId == userId && !ta.IsCompleted)
            .ToListAsync();
    }
}