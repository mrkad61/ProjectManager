// Business/TeamService.cs

using Microsoft.EntityFrameworkCore;
using TaskManager.Business;
using TaskManager.DataAccess;
using TaskManager.Entities;
using TaskManager.Entities.DTOs;
using TaskManager.Entities.Helper;
using AutoMapper;
using TaskManager.Business.DTOs;
using TaskManager.Business.DTOs.UserDTOs;
using TaskManager.DataAccess.Repository;

public class TeamService : ITeamService
{
    private readonly ITeamsRepository _teamsRepository;
    private readonly ITeamAssignmentRepository _teamAssignmentRepository;
    private readonly IMapper _mapper;

    public TeamService(ITeamsRepository teamsRepository, ITeamAssignmentRepository teamAssignmentRepository, IMapper mapper)
    {
        _teamsRepository = teamsRepository;
        _teamAssignmentRepository = teamAssignmentRepository;
        _mapper = mapper;
    }

    public async Task CreateTeamAsync(CreateTeamDto dto)
    {
        var team = new Teams
        {
            Name = dto.Name,
            User_id = dto.UserId,
            user_number = 1,
            UserType = UserType.Admin
        };

        _teamsRepository.Add(team);
        await _teamsRepository.SaveChangesAsync();

        _teamAssignmentRepository.Add(new TeamAssigment
        {
            Team_id = team.id,
            Member_id = dto.UserId,
            UserType = UserType.Admin
        });
        await _teamsRepository.SaveChangesAsync();
    }

    public async Task InviteMemberAsync(InviteMemberDto dto)
    {
        var user = await _teamsRepository.GetUserByEmail(dto.MemberEmail);
        if (user == null) throw new Exception("Kullanıcı bulunamadı.");

        var existingInvitation = await _teamsRepository.AnyInvitation(user.Id, dto.TeamId, InvitationStatus.Pending);
        if (existingInvitation) throw new Exception("Zaten bekleyen bir davet var.");

        _teamsRepository.Add(new Invitation
        {
            User = user,
            Teams = await _teamsRepository.FindAsync(dto.TeamId),
            Status = InvitationStatus.Pending
        });

        await _teamsRepository.SaveChangesAsync();
    }

    public async Task RemoveMemberAsync(RemoveMemberDto dto)
    {
        var user = await _teamsRepository.GetUserByUsername(dto.MemberName);
        if (user == null)
            throw new Exception("Kullanıcı bulunamadı.");

        var assignment = await _teamAssignmentRepository.FirstOrDefaultAsync(x =>
            x.Team_id == dto.TeamId && x.Member_id == user.Id);

        if (assignment == null)
            throw new Exception("Kullanıcı bu takıma atanmamış.");

        _teamAssignmentRepository.Remove(assignment);
        await _teamsRepository.SaveChangesAsync();
    }

    public async Task<List<UserListDto>> GetTeamAdminsAsync(long teamId)
    {
        var users = await _teamsRepository.GetTeamAdmins(teamId);
        return _mapper.Map<List<UserListDto>>(users);
    }

    public async Task<List<UserListDto>> GetTeamMembersAsync(long teamId, UserType? role = null)
    {
        var users = await _teamsRepository.GetTeamMembers(teamId, role);
        return _mapper.Map<List<UserListDto>>(users);
    }
}