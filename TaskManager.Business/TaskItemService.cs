using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.DataAccess.Repository;
using TaskManager.Entities.Project;
using TaskManager.Business.Mappers;

public class TaskItemService : ITaskItemService
{
    private readonly ITaskItemRepository _taskItemRepository;
    private readonly ITaskItemMapper _mapper;

    public TaskItemService(ITaskItemRepository taskItemRepository, ITaskItemMapper mapper)
    {
        _taskItemRepository = taskItemRepository;
        _mapper = mapper;
    }

    public async Task<List<TaskItem>> GetTasksByUserIdAsync(long userId)
        => await _taskItemRepository.GetTasksByUserId(userId);

    public async Task<List<TaskItem>> GetTasksByProjectIdAsync(long projectId)
        => await _taskItemRepository.GetTasksByProjectId(projectId);

    public async Task<List<TaskItem>> SearchTasksAsync(string keyword)
        => await _taskItemRepository.SearchTasks(keyword);
} 