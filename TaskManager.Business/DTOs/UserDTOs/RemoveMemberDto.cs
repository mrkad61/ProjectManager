namespace TaskManager.Entities.DTOs;
public class RemoveMemberDto
{
    public required string MemberName { get; set; }
    public long TeamId { get; set; }
}