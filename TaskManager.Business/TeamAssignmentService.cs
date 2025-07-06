using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Business;
using TaskManager.DataAccess.Repository;
using TaskManager.Business.DTOs.UserDTOs;
using AutoMapper;

namespace TaskManager.Business
{
    public class TeamAssignmentService : ITeamAssignmentService
    {
        private readonly ITeamAssignmentRepository _teamAssignmentRepository;
        private readonly IMapper _mapper;

        public TeamAssignmentService(ITeamAssignmentRepository teamAssignmentRepository, IMapper mapper)
        {
            _teamAssignmentRepository = teamAssignmentRepository;
            _mapper = mapper;
        }

        public async Task RemoveUserFromAllTeamsAsync(long userId)
        {
            await _teamAssignmentRepository.RemoveUserFromAllTeams(userId);
        }

        public async Task<List<UserListDto>> GetTeamMembersAsync(long teamId)
        {
            var assignments = await _teamAssignmentRepository.GetAssignmentsByTeamId(teamId);
            var users = new List<User>();
            
            foreach (var assignment in assignments)
            {
                if (assignment.Member != null)
                    users.Add(assignment.Member);
            }
            
            return _mapper.Map<List<UserListDto>>(users);
        }

        public async Task<bool> IsUserInTeamAsync(long userId, long teamId)
        {
            var assignment = await _teamAssignmentRepository.GetAssignment(userId, teamId);
            return assignment != null;
        }
    }
} 