namespace TaskManager.Shared.Models;

public record Task
{
    public Guid Id { get; init; }
    public required string Title { get; init; }
    public string? Description { get; init; }
    public TaskStatus Status { get; init; } = TaskStatus.Created;
    public TaskPriority Priority { get; init; } = TaskPriority.Medium;
    public DateTime? DueDate { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; init; }
    public string? AssignedToId { get; init; }
    public string? AssignedToName { get; init; }
    public Guid? ProjectId { get; init; }
    public string? ProjectName { get; init; }
    public bool IsOverdue { get; init; }
    public int CommentCount { get; init; }
    public int AttachmentCount { get; init; }
}

public enum TaskStatus
{
    Created = 1,
    InProgress = 2,
    OnHold = 3,
    Completed = 4,
    Cancelled = 5
}

public enum TaskPriority
{
    Low = 1,
    Medium = 2,
    High = 3,
    Critical = 4
}