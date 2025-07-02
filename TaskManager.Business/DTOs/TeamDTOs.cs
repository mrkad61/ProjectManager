namespace TaskManager.Business.DTOs;

public class TeamListDto
{
    public long Id { get; set; }
    public string Name { get; set; }
}

public class TeamDetailDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public long? ManagerId { get; set; }
    public List<UserListDto> Members { get; set; } = new();
} 