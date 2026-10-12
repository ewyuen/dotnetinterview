# Lab 4.1: Basic Async/Await

## Goal
Learn the fundamentals of asynchronous programming in C# using async/await patterns.

## Key Concepts
- **async/await**: Language keywords for asynchronous programming
- **Task**: Represents an asynchronous operation
- **Task&lt;T&gt;**: Represents an asynchronous operation that returns a value
- **Task.WhenAll/Task.WhenAny**: Coordinating multiple async operations
- **async void**: Fire-and-forget operations (use with caution)
- **Async I/O**: Non-blocking file and network operations

## Implementation
This lab demonstrates essential async/await patterns:

1. **Basic Async Methods**: Using `async` and `await` keywords
2. **Async Return Values**: Methods returning `Task<T>`
3. **Concurrent Operations**: Running multiple async operations simultaneously
4. **Fire-and-Forget**: `async void` methods for background operations
5. **Async File I/O**: Non-blocking file operations
6. **Async HTTP**: Asynchronous network requests

## Key Features Demonstrated
- `async` modifier and `await` operator
- `Task.Delay()` for async delays
- `Task.WhenAll()` for concurrent execution
- `Task.WhenAny()` for first-completed operation
- `async void` for event handlers and fire-and-forget operations
- `File.WriteAllTextAsync()` and `File.ReadAllTextAsync()`
- `HttpClient.GetAsync()` for async HTTP requests

## Expected Output
The program will show:
- Sequential and concurrent async operation execution
- Results from async methods with return values
- Fire-and-forget operations running in background
- Successful async file read/write operations
- HTTP request results (may fail if service unavailable)

## Important Notes
- `async` methods should return `Task` or `Task<T>`, not `void`
- Use `async void` only for event handlers (not recommended for regular methods)
- `await` captures the current context by default
- Async operations don't block threads - they free them for other work
- Always prefer async I/O methods over synchronous ones
- Exceptions in async void methods cannot be caught by callers

## References
- [async and await (C#)](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/async)
- [Task-based Asynchronous Pattern (TAP)](https://docs.microsoft.com/en-us/dotnet/standard/asynchronous-programming-patterns/task-based-asynchronous-pattern-tap)
- [Async/Await Best Practices](https://docs.microsoft.com/en-us/archive/msdn-magazine/2013/march/async-await-best-practices)