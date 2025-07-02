namespace TaskManager.Business.DTOs;

public class TaskItemListDto
{
    public long Id { get; set; }
    public string Title { get; set; }
    public bool IsCompleted { get; set; }
}

public class TaskItemDetailDto
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public bool IsCompleted { get; set; }
    public long ProjectId { get; set; }
} 