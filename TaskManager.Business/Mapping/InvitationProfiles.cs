using AutoMapper;
using TaskManager.Entities.repo.entity;
using TaskManager.Business.DTOs;

public class InvitationProfiles : Profile
{
    public InvitationProfiles()
    {
        CreateMap<Invitation, InvitationListDto>();
        CreateMap<Invitation, InvitationDetailDto>();
    }
} 