namespace TaskManager.Business.DTOs;

public class InvitationListDto
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public long TeamId { get; set; }
    public string Status { get; set; }
}

public class InvitationDetailDto
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public long TeamId { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }
} 