using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.DataAccess.Repository;
using TaskManager.Entities.Project;
using AutoMapper;
using TaskManager.Business.DTOs;

public class TaskAssignmentService : ITaskAssignmentService
{
    private readonly ITaskAssignmentRepository _taskAssignmentRepository;
    private readonly IMapper _mapper;
    public TaskAssignmentService(ITaskAssignmentRepository taskAssignmentRepository, IMapper mapper)
    {
        _taskAssignmentRepository = taskAssignmentRepository;
        _mapper = mapper;
    }
    public async Task<List<TaskAssignmentListDto>> GetAssignmentsByTaskIdAsync(long taskId)
    {
        var assignments = await _taskAssignmentRepository.GetAssignmentsByTaskId(taskId);
        return _mapper.Map<List<TaskAssignmentListDto>>(assignments);
    }
    public async Task<List<TaskAssignmentListDto>> GetCompletedTasksByUserIdAsync(long userId)
    {
        var assignments = await _taskAssignmentRepository.GetCompletedTasksByUserId(userId);
        return _mapper.Map<List<TaskAssignmentListDto>>(assignments);
    }
    public async Task<List<TaskAssignmentListDto>> GetPendingTasksByUserIdAsync(long userId)
    {
        var assignments = await _taskAssignmentRepository.GetPendingTasksByUserId(userId);
        return _mapper.Map<List<TaskAssignmentListDto>>(assignments);
    }
} 