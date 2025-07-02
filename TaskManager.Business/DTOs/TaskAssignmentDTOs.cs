namespace TaskManager.Business.DTOs;

public class TaskAssignmentListDto
{
    public long Id { get; set; }
    public long TaskItemId { get; set; }
    public long AssignedUserId { get; set; }
    public bool IsCompleted { get; set; }
}

public class TaskAssignmentDetailDto
{
    public long Id { get; set; }
    public long TaskItemId { get; set; }
    public long AssignedUserId { get; set; }
    public bool IsCompleted { get; set; }
    public bool IsApprovedByController { get; set; }
} 