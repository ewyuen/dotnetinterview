public record Task
{
    public int Id { get; init; }
    public string Title { get; init; }
    public string? Description { get; init; } // Nullable
    public bool IsCompleted { get; init; }
    public DateTime CreatedDate { get; set; } // Not init, so not part of equality

    public Task(int id, string title, string? description = null)
    {
        Id = id;
        Title = title;
        Description = description;
        IsCompleted = false;
        CreatedDate = DateTime.Now;
    }

    public Task MarkCompleted()
    {
        return this with { IsCompleted = true };
    }

    public Task UpdateDescription(string? desc)
    {
        return this with { Description = desc };
    }

    public void DisplayInfo()
    {
        Console.WriteLine($"Id: {Id}, Title: {Title}, Description: {Description ?? "No description"}, Completed: {IsCompleted}, Created: {CreatedDate}");
    }
}