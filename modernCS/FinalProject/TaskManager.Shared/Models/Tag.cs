namespace TaskManager.Shared.Models;

public record Tag
{
    public int Id { get; init; }
    public required string Name { get; init; }
    public string? Color { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public ICollection<Task> Tasks { get; init; } = new List<Task>();
}