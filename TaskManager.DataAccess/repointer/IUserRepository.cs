using TaskManager.Entities;
using TaskManager.Entities.Helper;

namespace TaskManager.DataAccess.Repository;

public interface IUserRepository :  ICrudRepository<User>
{
    Task IsEmailExist(string email);
    Task<User?> GetUserById(long id);
    Task<User?> GetUserByEmail(string email);
    Task<User?> GetUserByUsername(string username);
    Task<User?> GetAssignedTasksByUserId(long userId);
    Task<bool> IsExistByUsername(string username);
    Task<bool> IsExistByEmail(string email);
    Task<List<User?>> GetUsers();
    Task<List<User?>> GetUsersByUserType(UserType userType);
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByUsernameAsync(string username);
    Task<bool> ExistsByEmailAsync(string email);
    Task<bool> ExistsByUsernameAsync(string username);
    Task<List<Teams>> GetTeamsByUserIdAsync(long userId);
    Task<User?> GetUserWithTeams(long userId);
    Task<List<User>> GetUsersByRole(UserType role);
    Task<User?> GetUserWithAssignments(long userId);
    Task<List<User>> SearchUsers(string keyword);
    Task<List<User>> GetUsersByTeamId(long teamId);
    Task<List<(long TeamId, UserType Role)>> GetUserRolesInTeams(long userId);
}