namespace Lab2._4_FinalComponent;

public record TaskItem
{
    public int Id { get; init; }
    public string Title { get; init; }
    public string? Description { get; init; }
    public TaskPriority Priority { get; init; }
    public DateTime? DueDate { get; init; }
    public bool IsCompleted { get; private set; }
    public DateTime CreatedDate { get; private set; }
    public List<string> Tags { get; init; } = new();

    public TaskItem(int id, string title, TaskPriority priority = TaskPriority.Medium)
    {
        Id = id;
        Title = title;
        Priority = priority;
        IsCompleted = false;
        CreatedDate = DateTime.Now;
    }

    public TaskItem MarkCompleted() => this with { IsCompleted = true };

    public TaskItem UpdateDescription(string? description) => this with { Description = description };

    public TaskItem SetDueDate(DateTime? dueDate) => this with { DueDate = dueDate };

    public TaskItem AddTag(string tag)
    {
        var newTags = new List<string>(Tags) { tag };
        return this with { Tags = newTags };
    }

    public void DisplayInfo()
    {
        Console.WriteLine($"Task {Id}: {Title}");
        Console.WriteLine($"Priority: {Priority}");
        Console.WriteLine($"Description: {Description ?? "No description"}");
        Console.WriteLine($"Due Date: {DueDate?.ToString("yyyy-MM-dd") ?? "No due date"}");
        Console.WriteLine($"Completed: {IsCompleted}");
        Console.WriteLine($"Tags: {(Tags.Any() ? string.Join(", ", Tags) : "None")}");
        Console.WriteLine($"Created: {CreatedDate}");
        Console.WriteLine();
    }
}

public enum TaskPriority
{
    Low,
    Medium,
    High,
    Urgent
}

public static class TaskAnalyzer
{
    public static string AnalyzeTask(TaskItem task) => task switch
    {
        { IsCompleted: true } => "✅ Completed task",
        { Priority: TaskPriority.Urgent, DueDate: not null } when task.DueDate < DateTime.Now =>
            "🚨 Overdue urgent task!",
        { Priority: TaskPriority.Urgent } => "🔴 Urgent task",
        { Priority: TaskPriority.High, DueDate: not null } when task.DueDate < DateTime.Now.AddDays(1) =>
            "🟠 High priority task due soon",
        { Priority: TaskPriority.High } => "🟠 High priority task",
        { DueDate: not null } when task.DueDate < DateTime.Now => "⚠️ Overdue task",
        { Description: null } => "📝 Task needs description",
        { Tags.Count: > 3 } => "🏷️ Well-tagged task",
        _ => "📋 Regular task"
    };

    public static IEnumerable<TaskItem> FilterTasks(IEnumerable<TaskItem> tasks, TaskFilter filter) =>
        filter switch
        {
            { Status: TaskStatus.Completed } => tasks.Where(t => t.IsCompleted),
            { Status: TaskStatus.Pending } => tasks.Where(t => !t.IsCompleted),
            { Priority: not null } => tasks.Where(t => t.Priority == filter.Priority),
            { HasDueDate: true } => tasks.Where(t => t.DueDate.HasValue),
            { Tag: not null } => tasks.Where(t => t.Tags.Contains(filter.Tag)),
            _ => tasks
        };

    public static string GetTaskSummary(IEnumerable<TaskItem> tasks)
    {
        var total = tasks.Count();
        var completed = tasks.Count(t => t.IsCompleted);
        var overdue = tasks.Count(t => t.DueDate.HasValue && t.DueDate < DateTime.Now && !t.IsCompleted);
        var urgent = tasks.Count(t => t.Priority == TaskPriority.Urgent);

        return $"Total: {total}, Completed: {completed}, Overdue: {overdue}, Urgent: {urgent}";
    }
}

public record TaskFilter
{
    public TaskStatus? Status { get; init; }
    public TaskPriority? Priority { get; init; }
    public bool? HasDueDate { get; init; }
    public string? Tag { get; init; }
}

public enum TaskStatus
{
    Pending,
    Completed
}