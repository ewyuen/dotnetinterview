# Lab 1.3: Generics and Interfaces

## Goal
This lab introduces generics and interfaces, advanced C# features for creating reusable and type-safe code. You'll learn:
- Creating generic classes and methods
- Defining and implementing interfaces
- Using type constraints
- Implementing collection interfaces

## Illustration
We'll create a generic `Library<T>` class that implements both `ICollection<T>` and a custom `ISearchable<T>` interface. This demonstrates how generics allow type-safe operations on collections of any type.

### Key Concepts
- **Generics**: Type parameters that allow classes/methods to work with any data type
- **Interfaces**: Contracts that define method signatures without implementation
- **Type Safety**: Compile-time checking to prevent type-related errors
- **Collection Interfaces**: Standard interfaces like `ICollection<T>` for consistent APIs

## Implementation
This lab builds on the previous labs by incorporating all existing classes and adding generic functionality.

### ISearchable.cs (Interface)
```csharp
public interface ISearchable<T>
{
    IEnumerable<T> Search(string query);
}
```

### Library.cs (Generic Class)
```csharp
public class Library<T> : ICollection<T>, ISearchable<T>
{
    private List<T> _items = new List<T>();

    // ICollection<T> implementation
    public int Count => _items.Count;
    public bool IsReadOnly => false;

    public void Add(T item) => _items.Add(item);
    public void Clear() => _items.Clear();
    public bool Contains(T item) => _items.Contains(item);
    public void CopyTo(T[] array, int arrayIndex) => _items.CopyTo(array, arrayIndex);
    public bool Remove(T item) => _items.Remove(item);

    // IEnumerable<T> implementation
    public IEnumerator<T> GetEnumerator() => _items.GetEnumerator();
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => _items.GetEnumerator();

    // ISearchable<T> implementation
    public IEnumerable<T> Search(string query)
    {
        // For this example, we implement search for Book objects
        if (typeof(T) == typeof(Book))
        {
            return _items.Where(item =>
            {
                var book = item as Book;
                return book != null &&
                       (book.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                        book.Author.Contains(query, StringComparison.OrdinalIgnoreCase));
            });
        }
        return Enumerable.Empty<T>();
    }

    // Helper method to display all items
    public void GetAll()
    {
        foreach (var item in _items)
        {
            if (item is Book book)
                book.DisplayInfo();
            else if (item is Vehicle vehicle)
                vehicle.DisplayInfo();
        }
    }
}
```

### Program.cs
```csharp
// Create a generic library for books
Library<Book> bookLibrary = new Library<Book>();

// Add books to the library
bookLibrary.Add(new Book("1984", "George Orwell", "123456"));
bookLibrary.Add(new Book("To Kill a Mockingbird", "Harper Lee", "789012"));
bookLibrary.Add(new Book("Pride and Prejudice", "Jane Austen", "345678"));

// Display all books
Console.WriteLine("All books:");
bookLibrary.GetAll();

// Search for books
Console.WriteLine("\nSearch for 'George':");
var searchResults = bookLibrary.Search("George");
foreach (var book in searchResults)
{
    book.DisplayInfo();
}
```

## Running the Lab
1. Navigate to this directory
2. Run `dotnet run`
3. Observe the generic collection operations and search functionality

## Expected Output
```
All books:
Title: 1984, Author: George Orwell, ISBN: 123456, Available: True
Title: To Kill a Mockingbird, Author: Harper Lee, ISBN: 789012, Available: True
Title: Pride and Prejudice, Author: Jane Austen, ISBN: 345678, Available: True

Search for 'George':
Title: 1984, Author: George Orwell, ISBN: 123456, Available: True
```

## Learning Outcomes
After completing this lab, you should understand:
- How generics provide type safety and code reuse
- The difference between interfaces and abstract classes
- Implementing standard .NET interfaces like `ICollection<T>`
- Using LINQ methods like `Where()` for querying collections
- How to create flexible, reusable components with generics