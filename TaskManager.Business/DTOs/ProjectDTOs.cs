namespace TaskManager.Business.DTOs;

public class ProjectListDto
{
    public long Id { get; set; }
    public string Title { get; set; }
}

public class ProjectDetailDto
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public long TeamId { get; set; }
} 