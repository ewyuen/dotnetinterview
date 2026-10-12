# Lab 1.1: Book Class

## Goal
This lab introduces the fundamentals of object-oriented programming in C# by creating a simple `Book` class. The objective is to understand:
- Defining classes and properties
- Implementing constructors
- Creating instance methods
- Working with basic data types and object instantiation

## Illustration
The `Book` class represents a library book with the following characteristics:
- **Properties**: Title, Author, ISBN, and availability status
- **Methods**: Check out a book, return a book, and display book information

### Key Concepts
- **Class Definition**: A blueprint for creating objects
- **Properties**: Encapsulated data fields with getters/setters
- **Methods**: Behaviors that objects can perform
- **Object Instantiation**: Creating instances of classes

## Implementation
The implementation includes:

### Book.cs
```csharp
public class Book
{
    // Properties
    public string Title { get; set; }
    public string Author { get; set; }
    public string ISBN { get; set; }
    public bool IsAvailable { get; set; }

    // Constructor
    public Book(string title, string author, string isbn)
    {
        Title = title;
        Author = author;
        ISBN = isbn;
        IsAvailable = true; // Books start as available
    }

    // Methods
    public void CheckOut()
    {
        if (IsAvailable)
        {
            IsAvailable = false;
            Console.WriteLine($"{Title} has been checked out.");
        }
        else
        {
            Console.WriteLine($"{Title} is not available.");
        }
    }

    public void Return()
    {
        IsAvailable = true;
        Console.WriteLine($"{Title} has been returned.");
    }

    public void DisplayInfo()
    {
        Console.WriteLine($"Title: {Title}, Author: {Author}, ISBN: {ISBN}, Available: {IsAvailable}");
    }
}
```

### Program.cs
```csharp
// Create a book instance
Book book1 = new Book("1984", "George Orwell", "123456");

// Demonstrate methods
book1.DisplayInfo();  // Show initial state
book1.CheckOut();     // Check out the book
book1.DisplayInfo();  // Show updated state
book1.Return();       // Return the book
book1.DisplayInfo();  // Show final state
```

## Running the Lab
1. Navigate to this directory
2. Run `dotnet run`
3. Observe the console output showing book operations

## Expected Output
```
Title: 1984, Author: George Orwell, ISBN: 123456, Available: True
1984 has been checked out.
Title: 1984, Author: George Orwell, ISBN: 123456, Available: False
1984 has been returned.
Title: 1984, Author: George Orwell, ISBN: 123456, Available: True
```

## Learning Outcomes
After completing this lab, you should understand:
- How to define and use classes in C#
- The difference between properties and methods
- Basic object lifecycle (creation, method calls, state changes)
- Console output for debugging and demonstration