using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.DataAccess.Repository;
using TaskManager.Entities.Project;
using AutoMapper;
using TaskManager.Business.DTOs;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;
    private readonly IMapper _mapper;
    public ProjectService(IProjectRepository projectRepository, IMapper mapper)
    {
        _projectRepository = projectRepository;
        _mapper = mapper;
    }
    public async Task<List<ProjectListDto>> GetProjectsByUserIdAsync(long userId)
    {
        var projects = await _projectRepository.GetProjectsByUserId(userId);
        return _mapper.Map<List<ProjectListDto>>(projects);
    }
    public async Task<List<ProjectListDto>> SearchProjectsAsync(string keyword)
    {
        var projects = await _projectRepository.SearchProjects(keyword);
        return _mapper.Map<List<ProjectListDto>>(projects);
    }
} 