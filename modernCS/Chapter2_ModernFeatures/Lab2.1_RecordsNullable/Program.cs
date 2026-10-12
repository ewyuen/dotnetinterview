// Lab 2.1: Records and Nullable Types
Console.WriteLine("=== Lab 2.1: Records and Nullable Types ===");

// Create tasks with nullable descriptions
Task task1 = new Task(1, "Learn C# Records", "Study record types in C#");
Task task2 = new Task(2, "Practice Nullable Types"); // No description (null)

Console.WriteLine("Original tasks:");
task1.DisplayInfo();
task2.DisplayInfo();

// Demonstrate record immutability and 'with' expression
Task task1Completed = task1.MarkCompleted();
Task task2Updated = task2.UpdateDescription("Master nullable reference types");

Console.WriteLine("\nAfter modifications (new instances created):");
task1Completed.DisplayInfo();
task2Updated.DisplayInfo();

// Show original tasks are unchanged
Console.WriteLine("\nOriginal tasks unchanged:");
task1.DisplayInfo();
task2.DisplayInfo();

// Demonstrate record equality
Task task3 = new Task(1, "Learn C# Records", "Study record types in C#") { CreatedDate = task1.CreatedDate };
Console.WriteLine($"\nTask1 equals Task3: {task1 == task3}"); // Should be True for records with same data

// Nullable reference types in action
string? nullableString = null;
Console.WriteLine($"\nNullable string: {nullableString ?? "Default value"}");

// Null-conditional operator
Task? nullableTask = null;
Console.WriteLine($"Nullable task title: {nullableTask?.Title ?? "No task"}");
