using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Entities.Project;

public interface ITaskAssignmentService
{
    Task<List<TaskAssignment>> GetAssignmentsByTaskIdAsync(long taskId);
    Task<List<TaskAssignment>> GetCompletedTasksByUserIdAsync(long userId);
    Task<List<TaskAssignment>> GetPendingTasksByUserIdAsync(long userId);
} 