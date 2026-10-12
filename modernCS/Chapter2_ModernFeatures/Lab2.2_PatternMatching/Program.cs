// Lab 2.2: Pattern Matching
Console.WriteLine("=== Lab 2.2: Pattern Matching ===");

// Create some tasks
Task task1 = new Task(1, "Learn Records", "Study record types");
Task task2 = new Task(2, "Practice Patterns"); // No description
Task task3 = new Task(3, "Complete Tutorial", "Finish all chapters") { CreatedDate = task1.CreatedDate };
task3 = task3.MarkCompleted();

List<Task> tasks = new() { task1, task2, task3 };

// Pattern matching with switch expression
Console.WriteLine("Task Status Analysis:");
foreach (var task in tasks)
{
    string status = task switch
    {
        { IsCompleted: true } => "Completed",
        { Description: null } => "Needs Description",
        { Title.Length: > 15 } => "Long Title",
        _ => "In Progress"
    };
    Console.WriteLine($"{task.Title}: {status}");
}

Console.WriteLine();

// Property patterns
Console.WriteLine("Detailed Analysis:");
foreach (var task in tasks)
{
    string analysis = task switch
    {
        { IsCompleted: true, Title: var title } when title.Contains("Tutorial") => $"Tutorial '{title}' is done!",
        { IsCompleted: false, Description: null } => "Incomplete task without description",
        { Id: < 3, IsCompleted: false } => "Early task still in progress",
        _ => "Other task"
    };
    Console.WriteLine($"{task.Id}: {analysis}");
}

Console.WriteLine();

// Type patterns with when clauses
Console.WriteLine("Type and Value Patterns:");
object[] objects = { task1, "Hello World", 42, null, task2 };

foreach (var obj in objects)
{
    string description = obj switch
    {
        Task { IsCompleted: true } => "Completed Task",
        Task t when t.Description == null => "Task without description",
        string s when s.Length > 10 => $"Long string: {s}",
        int i when i > 40 => $"Large number: {i}",
        null => "Null value",
        _ => "Other type"
    };
    Console.WriteLine($"{obj?.GetType().Name ?? "null"}: {description}");
}

Console.WriteLine();

// Positional patterns (if we had tuples)
var tuple = (task1, "Active");
string tupleDesc = tuple switch
{
    ( { IsCompleted: true }, "Active" ) => "Completed active task",
    ( { IsCompleted: false }, var status ) => $"Incomplete task with status: {status}",
    _ => "Other combination"
};
Console.WriteLine($"Tuple pattern result: {tupleDesc}");
