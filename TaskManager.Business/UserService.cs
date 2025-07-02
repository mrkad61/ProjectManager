using TaskManager.DataAccess.Repository;
using TaskManager.Entities;
using TaskManager.Entities.Helper;
using AutoMapper;
using TaskManager.Business.DTOs.UserDTOs;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }
    
    public async Task<User?> AuthenticateAsync(string email, string password)
    {
        var user = await _userRepository.GetUserByEmail(email);
        if (user == null) return null;

        bool isValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
        return isValid ? user : null;
    }

    public async Task<User?> GetUserByIdAsync(long id)
        => await _userRepository.GetUserById(id);

    public async Task<List<UserListDto>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetUsers();
        return _mapper.Map<List<UserListDto>>(users);
    }

    public async Task<List<UserListDto>> GetUsersByRoleAsync(UserType role)
    {
        var users = await _userRepository.GetUsersByRole(role);
        return _mapper.Map<List<UserListDto>>(users);
    }

    public async Task<List<UserListDto>> SearchUsersAsync(string keyword)
    {
        var users = await _userRepository.SearchUsers(keyword);
        return _mapper.Map<List<UserListDto>>(users);
    }

    public async Task<List<UserListDto>> GetUsersByTeamIdAsync(long teamId)
    {
        var users = await _userRepository.GetUsersByTeamId(teamId);
        return _mapper.Map<List<UserListDto>>(users);
    }

    public async Task<bool> IsUsernameExistsAsync(string username)
        => await _userRepository.IsExistByUsername(username);
    
    public async Task<User?> GetUserByUsernameAsync(string username)
        => await _userRepository.GetUserByUsername(username);
    
    public async Task<User?> GetUserByEmailAsync(string email)
        => await _userRepository.GetUserByEmail(email);

    public async Task<User?> GetUserWithTeamsAsync(long userId)
        => await _userRepository.GetUserWithTeams(userId);

    public async Task<User?> GetUserWithAssignmentsAsync(long userId)
        => await _userRepository.GetUserWithAssignments(userId);

    public async Task<List<Teams>> GetTeamsByUserIdAsync(long userId)
        => await _userRepository.GetTeamsByUserIdAsync(userId);
}