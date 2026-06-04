using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.DTOs.Tasks;

public class TaskItemDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Domain.Enums.TaskStatus Status { get; set; }
    public TaskPriority Priority { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid UserId { get; set; }
}

