using TaskManager.Entities.Project;

namespace TaskManager.DataAccess.Repository;

public interface ITaskAssignmentRepository : ICrudRepository<TaskAssignment>
{
    Task<List<TaskAssignment>> GetAssignmentsByUserId(long userId);
    Task<List<TaskAssignment>> GetAssignmentsByTaskId(long taskId);
    Task<TaskAssignment?> GetAssignment(long userId, long taskId);
    Task<bool> IsUserAssignedToTask(long userId, long taskId);
    Task<List<TaskAssignment>> GetCompletedTasksByUserId(long userId);
    Task<List<TaskAssignment>> GetPendingTasksByUserId(long userId);
}
