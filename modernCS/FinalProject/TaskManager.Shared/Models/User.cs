namespace TaskManager.Shared.Models;

public record User
{
    public int Id { get; init; }
    public required string Username { get; init; }
    public required string Email { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public ICollection<Task> AssignedTasks { get; init; } = new List<Task>();
}