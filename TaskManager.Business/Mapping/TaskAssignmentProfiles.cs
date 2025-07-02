using AutoMapper;
using TaskManager.Entities.Project;
using TaskManager.Business.DTOs;

public class TaskAssignmentProfiles : Profile
{
    public TaskAssignmentProfiles()
    {
        CreateMap<TaskAssignment, TaskAssignmentListDto>();
        CreateMap<TaskAssignment, TaskAssignmentDetailDto>();
    }
} 