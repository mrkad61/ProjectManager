using TaskManager.Entities.Project;

namespace TaskManager.DataAccess.Repository;

public interface IProjectRepository : ICrudRepository<Project>
{
    Task<Project?> GetProjectByIdAsync(long projectId);
    Task<List<Project>> GetAllProjectsAsync();
    Task<bool> IsProjectExistByNameAsync(string projectName);
    Task<List<Project>> GetProjectWithTasks(long projectId);
    Task<List<Project>> GetProjectWithTasksByName(string projectName);
    Task<List<Project>> GetProjectsByUserId(long userId);
    Task<List<Project>> SearchProjects(string keyword);
}
