using AutoMapper;
using TaskManager.Entities;
using TaskManager.Business.DTOs;
using TaskManager.Business.DTOs.UserDTOs;

public class TeamProfiles : Profile
{
    public TeamProfiles()
    {
        CreateMap<Teams, TeamListDto>();
        CreateMap<Teams, TeamDetailDto>()
            .ForMember(dest => dest.Members, opt => opt.MapFrom(src => src.TeamAssignments.Select(ta => ta.Member)));
    }
} 