namespace TaskManager.Shared.Models;

public class CreateTaskRequest
{
    public string Title { get; set; } = "";
    public string? Description { get; set; }
    public TaskPriority Priority { get; set; } = TaskPriority.Medium;
    public DateTime? DueDate { get; set; }
    public string? AssignedToId { get; set; }
    public Guid? ProjectId { get; set; }
}

public record UpdateTaskRequest(
    string? Title,
    string? Description,
    TaskPriority? Priority,
    TaskStatus? Status,
    DateTime? DueDate,
    string? AssignedToId
);