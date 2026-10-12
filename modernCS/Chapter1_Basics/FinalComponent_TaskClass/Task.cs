public class Task
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime CreatedDate { get; set; }

    public Task(int id, string title, string description)
    {
        Id = id;
        Title = title;
        Description = description;
        IsCompleted = false;
        CreatedDate = DateTime.Now;
    }

    public void MarkCompleted()
    {
        IsCompleted = true;
    }

    public void UpdateDescription(string desc)
    {
        Description = desc;
    }

    public void DisplayInfo()
    {
        Console.WriteLine($"Id: {Id}, Title: {Title}, Description: {Description}, Completed: {IsCompleted}, Created: {CreatedDate}");
    }
}