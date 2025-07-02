using TaskManager.Entities.Helper;

namespace TaskManager.Entities.DTOs;

public class UserUpdateDto
{
    public required long Id { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public UserType? Role { get; set; }
}
