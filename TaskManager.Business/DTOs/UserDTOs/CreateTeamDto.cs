using TaskManager.Entities.Helper;

namespace TaskManager.Entities.DTOs;

public class CreateTeamDto
{
    public string Name { get; set; }
    public long UserId { get; set; }
    public UserType  UserType { get; set; }
}