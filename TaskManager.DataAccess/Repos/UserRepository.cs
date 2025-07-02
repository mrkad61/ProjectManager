using Microsoft.EntityFrameworkCore;
using TaskManager.DataAccess.Repository;
using TaskManager.Entities.Helper;
using TaskManager.Entities;
namespace TaskManager.DataAccess.Repos;

public class UserRepository : CrudRepository<User>, IUserRepository
{
    private readonly AppDbContext _context;
    
    public UserRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
    

    public async Task IsEmailExist(string email)
    {
        if (await IsExistByEmail(email))
            throw new Exception("Bu e-posta ile kayıtlı bir kullanıcı zaten var.");
    }

    public async Task<User?> GetUserById(long id)
    {
        try
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public async Task<User?> GetUserByEmail(string email)
        => await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    
    public async Task<User?> GetUserByUsername(string username)
        => await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
    
    public async Task<User?> GetAssignedTasksByUserId(long userId)
    {
        
        return await _context.Users
            .Include(t => t.Teams)
            .ThenInclude(ta => ta.TaskItem)
            .FirstOrDefaultAsync(u => u.Id == userId);
    }
    
    public async Task<User?> GetTeamsManagedByUserId(long userId)
    {
        return await _context.Users
            .Include(t => t.Teams)
            .FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<bool> IsExistByUsername(string username)
    {
        return await _context.Users.AnyAsync(u => u.Username == username);
    }

    public async Task<bool> IsExistByEmail(string email)
    {
        return await _context.Users.AnyAsync(u => u.Email == email);
    }

    public async Task<List<User>> GetUsers()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<List<User>> GetUsersByUserType(UserType userType)
        => await _context.Users.Where(u=> u.Role == userType).ToListAsync();

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await _context.Users.AnyAsync(u => u.Email == email);
    }

    public async Task<bool> ExistsByUsernameAsync(string username)
    {
        return await _context.Users.AnyAsync(u => u.Username == username);
    }

    public async Task<List<Teams>> GetTeamsByUserIdAsync(long userId)
    {
        return await _context.TeamAssignments
            .Where(ta => ta.Member_id == userId)
            .Include(ta => ta.Team)
            .Select(ta => ta.Team)
            .ToListAsync();
    }

    public async Task<User?> GetUserWithTeams(long userId)
    {
        return await _context.Users
            .Include(u => u.Teams)
            .ThenInclude(ta => ta.Team)
            .FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<List<User>> GetUsersByRole(UserType role)
    {
        return await _context.Users
            .Where(u => u.Role == role)
            .ToListAsync();
    }

    public async Task<User?> GetUserWithAssignments(long userId)
    {
        return await _context.Users
            .Include(u => u.TaskAssignments)
            .ThenInclude(ta => ta.TaskItem)
            .FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<List<User>> SearchUsers(string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
            return await _context.Users.ToListAsync();
        return await _context.Users
            .Where(u => u.Username.ToLower().Contains(keyword.ToLower()) ||
                         u.Email.ToLower().Contains(keyword.ToLower()))
            .ToListAsync();
    }

    public async Task<List<User>> GetUsersByTeamId(long teamId)
    {
        return await _context.TeamAssignments
            .Where(ta => ta.Team_id == teamId)
            .Include(ta => ta.Member)
            .Select(ta => ta.Member)
            .ToListAsync();
    }

    public async Task<List<(long TeamId, UserType Role)>> GetUserRolesInTeams(long userId)
    {
        return await _context.TeamAssignments
            .Where(ta => ta.Member_id == userId)
            .Select(ta => new ValueTuple<long, UserType>(ta.Team_id, ta.UserType))
            .ToListAsync();
    }
}