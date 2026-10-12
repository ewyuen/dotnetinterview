namespace Lab2._3_InitOnlySetters;

public record TodoTask
{
    public int Id { get; init; }
    public string Title { get; init; }
    public string? Description { get; init; }
    public bool IsCompleted { get; private set; }
    public DateTime CreatedDate { get; private set; }

    public TodoTask(int id, string title, string? description = null)
    {
        Id = id;
        Title = title;
        Description = description;
        IsCompleted = false;
        CreatedDate = DateTime.Now;
    }

    public TodoTask MarkCompleted()
    {
        return this with { IsCompleted = true };
    }

    public TodoTask UpdateDescription(string newDescription)
    {
        return this with { Description = newDescription };
    }

    public void DisplayInfo()
    {
        Console.WriteLine($"Task {Id}: {Title}");
        Console.WriteLine($"Description: {Description ?? "No description"}");
        Console.WriteLine($"Completed: {IsCompleted}");
        Console.WriteLine($"Created: {CreatedDate}");
        Console.WriteLine();
    }
}