// TaskManager.Business/Interfaces/IUserService.cs
using TaskManager.Entities;
using TaskManager.Entities.Helper;

public interface IUserService
{
    Task<User?> AuthenticateAsync(string email, string password);
    Task<User?> GetUserByIdAsync(long id);
    Task<List<User?>> GetAllUsersAsync();
    Task<List<User>> GetUsersByRoleAsync(UserType role);
    Task<bool> IsUsernameExistsAsync(string username);
    Task<User?> GetUserByUsernameAsync(string username);
    Task<User?> GetUserByEmailAsync(string email);
    Task<User?> GetUserWithTeamsAsync(long userId);
    Task<User?> GetUserWithAssignmentsAsync(long userId);
    Task<List<Teams>> GetTeamsByUserIdAsync(long userId);
    Task<List<User>> SearchUsersAsync(string keyword);
    Task<List<User>> GetUsersByTeamIdAsync(long teamId);
    Task<List<(long TeamId, UserType Role)>> GetUserRolesInTeamsAsync(long userId);
}
