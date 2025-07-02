using TaskManager.Entities.Project;

namespace TaskManager.DataAccess.Repository;

public interface ITaskItemRepository : ICrudRepository<TaskItem>
{
    Task<List<TaskItem>> GetTasksByUserId(long userId);
    Task<List<TaskItem>> GetTasksByTeamId(long teamId);
    Task<List<TaskItem>> GetTasksByProjectId(long projectId);
    Task<List<TaskItem>> SearchTasks(string keyword);
}
