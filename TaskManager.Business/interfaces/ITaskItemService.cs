using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Entities.Project;

public interface ITaskItemService
{
    Task<List<TaskItem>> GetTasksByUserIdAsync(long userId);
    Task<List<TaskItem>> GetTasksByProjectIdAsync(long projectId);
    Task<List<TaskItem>> SearchTasksAsync(string keyword);
} 