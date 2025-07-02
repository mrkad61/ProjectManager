using AutoMapper;
using TaskManager.Entities.Project;
using TaskManager.Business.DTOs;

public class ProjectProfiles : Profile
{
    public ProjectProfiles()
    {
        CreateMap<Project, ProjectListDto>();
        CreateMap<Project, ProjectDetailDto>();
    }
} 