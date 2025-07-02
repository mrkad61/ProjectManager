using AutoMapper;
using TaskManager.Entities.Project;
using TaskManager.Business.DTOs;

public class TaskItemProfiles : Profile
{
    public TaskItemProfiles()
    {
        CreateMap<TaskItem, TaskItemListDto>();
        CreateMap<TaskItem, TaskItemDetailDto>();
    }
} 