using AutoMapper;
using TaskManager.Business.DTOs;

namespace TaskManager.Business
{
    public class InvitationService
    {
        private readonly IInvitationRepository _invitationRepository;
        private readonly IMapper _mapper;

        public InvitationService(IInvitationRepository invitationRepository, IMapper mapper)
        {
            _invitationRepository = invitationRepository;
            _mapper = mapper;
        }

        public async Task<List<InvitationListDto>> GetInvitationsByUserIdAsync(long userId)
        {
            var invitations = await _invitationRepository.GetInvitationsByUserId(userId);
            return _mapper.Map<List<InvitationListDto>>(invitations);
        }

        public async Task<List<InvitationListDto>> GetPendingInvitationsByTeamIdAsync(long teamId)
        {
            var invitations = await _invitationRepository.GetPendingInvitationsByTeamId(teamId);
            return _mapper.Map<List<InvitationListDto>>(invitations);
        }
    }
} 