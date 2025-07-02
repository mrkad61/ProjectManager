namespace TaskManager.Business.DTOs.UserDTOs;

public class UserListDto
{
    public long Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public UserType Role { get; set; }
} 