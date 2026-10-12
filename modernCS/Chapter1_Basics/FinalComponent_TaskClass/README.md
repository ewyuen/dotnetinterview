# Final Component: Task Class

## Goal
This final component brings together all the concepts learned in Chapter 1 to create a `Task` class that will serve as the foundation for the task management application. The objective is to:
- Apply all OOP principles learned (encapsulation, inheritance, polymorphism, abstraction)
- Create a practical class that demonstrates real-world usage
- Prepare for the final project by establishing a core data model

## Illustration
The `Task` class represents a task item in a task management system. It incorporates properties for task identification, content, status, and metadata, along with methods to manipulate task state.

### Key Concepts Applied
- **Encapsulation**: Private fields with public properties
- **Data Validation**: Basic validation in methods
- **Object Lifecycle**: Creation, modification, and state changes
- **Real-world Modeling**: Practical application of learned concepts

## Implementation
This component includes all classes from previous labs plus the new `Task` class.

### Task.cs
```csharp
public class Task
{
    // Properties
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime CreatedDate { get; set; }

    // Constructor
    public Task(int id, string title, string description)
    {
        Id = id;
        Title = title;
        Description = description;
        IsCompleted = false; // Tasks start as incomplete
        CreatedDate = DateTime.Now; // Set creation timestamp
    }

    // Methods
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
```

### Program.cs
```csharp
// Demonstrate the Task class
Task task1 = new Task(1, "Complete C# Basics", "Finish all labs in Chapter 1");

// Display initial state
task1.DisplayInfo();

// Update the task
task1.UpdateDescription("Finish all labs in Chapter 1 and test thoroughly");

// Mark as completed
task1.MarkCompleted();

// Display final state
task1.DisplayInfo();
```

## Running the Component
1. Navigate to this directory
2. Run `dotnet run`
3. Observe the task lifecycle demonstration

## Expected Output
```
Id: 1, Title: Complete C# Basics, Description: Finish all labs in Chapter 1, Completed: False, Created: 9/25/2025 8:28:17 AM
Id: 1, Title: Complete C# Basics, Description: Finish all labs in Chapter 1 and test thoroughly, Completed: True, Created: 9/25/2025 8:28:17 AM
```

## Integration with Previous Labs
This component demonstrates how all Chapter 1 concepts work together:
- **Classes and Objects** (Lab 1.1): Basic class structure
- **Inheritance** (Lab 1.2): Could be extended with derived task types
- **Generics and Interfaces** (Lab 1.3): Tasks could be stored in generic collections

## Learning Outcomes
After completing this component, you should:
- Understand how to combine multiple OOP concepts in a single application
- Be able to model real-world entities as classes
- Know how to create classes that will integrate with larger systems
- Feel prepared to move on to more advanced C# features in Chapter 2

## Next Steps
This `Task` class will serve as the foundation for:
- Chapter 2: Modern C# features (records, pattern matching, etc.)
- Chapter 3: Multithreading and concurrency
- Chapter 4: Async programming
- Chapter 5: ASP.NET Core web development
- Final Project: Complete task management web application