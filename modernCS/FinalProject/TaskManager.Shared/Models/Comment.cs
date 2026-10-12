namespace TaskManager.Shared.Models;

public record Comment
{
    public int Id { get; init; }
    public required string Content { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public int TaskId { get; init; }
    public Task? Task { get; init; }
    public int UserId { get; init; }
    public User? User { get; init; }
}