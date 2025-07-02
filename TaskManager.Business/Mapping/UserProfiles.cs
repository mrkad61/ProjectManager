using AutoMapper;
using TaskManager.Entities;
using TaskManager.Entities.DTOs;

namespace TaskManager.Business.Mapping;

public class UserProfiles : Profile
{
    public UserProfiles()
    {
        CreateMap<UserUpdateDto, User>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        CreateMap<UserRegisterDto, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
            .ForMember(dest => dest.Username, opt => opt.Ignore())
            .ForMember(dest => dest.Email, opt => opt.Ignore());
    }
}