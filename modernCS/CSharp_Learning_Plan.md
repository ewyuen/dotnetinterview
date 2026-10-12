# C# Learning Plan: Labs and Final Project

This plan outlines the step-by-step instructions to complete all labs from the C# review guide, building progressively toward the final project: a web-based task management application.

## Prerequisites
- Install .NET SDK (latest version, e.g., .NET 8 or 9)
- Install Visual Studio Code or Visual Studio
- Create a new workspace folder for the project

## Chapter 1: C# Basics Review

### Lab 1.1: Create a Book Class
1. Create a new console application: `dotnet new console -n BasicsLab1`
2. Define a `Book` class with properties: `Title`, `Author`, `ISBN`, `IsAvailable`
3. Add methods: `CheckOut()`, `Return()`, `DisplayInfo()`
4. In `Program.cs`, create instances of `Book` and demonstrate method calls
5. Run and test: `dotnet run`

### Lab 1.2: Inheritance with Vehicles
1. In the same project or new one, create `Vehicle` base class with `Make`, `Model`, `Year`
2. Create derived classes `Car` (add `NumberOfDoors`) and `Bike` (add `HasSidecar`)
3. Override a `DisplayInfo()` method in each derived class
4. Create instances and call methods to show polymorphism
5. Run and verify inheritance behavior

### Lab 1.3: Generics and Interfaces
1. Create a generic `Library<T>` class that implements `ICollection<T>`
2. Add methods: `Add(T item)`, `Remove(T item)`, `GetAll()`
3. Create an interface `ISearchable<T>` with `Search(string query)` method
4. Implement search in `Library<T>` for books by title/author
5. Test with a collection of `Book` objects

### Final Component: Task Class
1. Create a `Task` class with properties: `Id`, `Title`, `Description`, `IsCompleted`, `CreatedDate`
2. Add methods: `MarkCompleted()`, `UpdateDescription(string desc)`
3. Ensure it follows basic OOP principles

## Chapter 2: Latest C# Features

### Lab 2.1: Records and Nullable Types
1. Create a new project or update existing
2. Convert the `Book` class to a `record` with nullable `Author?`
3. Use null-conditional operators (`?.`) and null-coalescing (`??`)
4. Demonstrate record equality and `with` expressions

### Lab 2.2: Pattern Matching
1. Create a method that takes `object` parameter
2. Use switch expression with property patterns for `Book`, `Person`, etc.
3. Handle different types and properties (e.g., book availability)
4. Test with various object types

### Lab 2.3: Init-Only Setters
1. Update classes to use `init` setters and `required` properties
2. Create constructors or object initializers with `init` properties
3. Ensure immutability where appropriate

### Final Component: Update Task Class
1. Convert `Task` to a `record` with init-only setters
2. Use nullable reference types appropriately
3. Apply pattern matching for task status checks

## Chapter 3: Multithread Programming

### Lab 3.1: Basic Threading
1. Create a console app for threading demos
2. Use `Thread` class to run multiple threads processing a shared list
3. Demonstrate thread creation, starting, and joining
4. Handle basic synchronization with `lock`

### Lab 3.2: Task-Based Operations
1. Replace `Thread` usage with `Task.Run()`
2. Create async methods for background work
3. Use `Task.WaitAll()` and `Task.WhenAll()` for coordination
4. Handle exceptions in tasks

### Lab 3.3: Producer-Consumer Pattern
1. Implement producer-consumer with `ConcurrentQueue`
2. Create producer threads adding items, consumer threads processing
3. Use `BlockingCollection` for thread-safe operations
4. Demonstrate concurrent data structures

### Final Component: Concurrent Task Processing
1. Add background threads/tasks to check task completion status
2. Use concurrent collections for shared task data
3. Implement thread-safe updates to task list

## Chapter 4: Async Pattern

### Lab 4.1: Async File I/O
1. Create methods to read/write files asynchronously
2. Use `File.ReadAllTextAsync()`, `File.WriteAllTextAsync()`
3. Compare performance with synchronous versions
4. Handle file exceptions in async context

### Lab 4.2: Async Web Requests
1. Use `HttpClient` for async HTTP calls
2. Create methods to fetch data from APIs
3. Implement retry logic with `Task.Delay()`
4. Parse JSON responses asynchronously

### Lab 4.3: Async Pipeline
1. Create a series of async operations (e.g., validate -> process -> save)
2. Use `await` chaining and exception handling
3. Implement progress reporting with `IProgress<T>`
4. Test error scenarios and recovery

### Final Component: Async Task Operations
1. Make all task CRUD operations async
2. Use async file I/O for task persistence
3. Implement async validation and processing

## Chapter 5: ASP.NET Core Basics

### Lab 5.1: Basic API Controller
1. Create new ASP.NET Core Web API: `dotnet new webapi -n TaskManagerApi`
2. Add a `TasksController` with basic CRUD endpoints
3. Use `[HttpGet]`, `[HttpPost]`, etc. attributes
4. Return `IActionResult` with appropriate status codes

### Lab 5.2: Dependency Injection
1. Create `ITaskService` interface and `TaskService` implementation
2. Register services in `Program.cs` (or `Startup.cs`)
3. Inject service into controller constructor
4. Implement in-memory data storage

### Lab 5.3: Entity Framework Core
1. Add EF Core packages: `Microsoft.EntityFrameworkCore.Sqlite`
2. Create `TaskDbContext` with `DbSet<Task>`
3. Configure SQLite database in `Program.cs`
4. Implement repository pattern with async operations

### Final Component: Complete Web API
1. Add all CRUD endpoints for tasks
2. Implement proper error handling and validation
3. Add Swagger/OpenAPI documentation
4. Test endpoints with tools like Postman

## Final Project: Task Management Web App

### Integration Steps
1. Combine all components into a single ASP.NET Core project
2. Use modern C# features throughout (records, pattern matching, etc.)
3. Implement concurrent background processing for task monitoring
4. Ensure all operations are async
5. Add basic frontend (optional: Razor pages or simple HTML/JS)
6. Test the complete application:
   - Create tasks via API
   - Update task status
   - View task list
   - Background processing updates
7. Deploy locally and verify functionality

### Testing Checklist
- [ ] All CRUD operations work
- [ ] Async operations complete without blocking
- [ ] Concurrent operations are thread-safe
- [ ] Error handling is robust
- [ ] Modern C# features are used appropriately
- [ ] Code follows best practices

## Additional Resources
- Official C# documentation: https://learn.microsoft.com/en-us/dotnet/csharp/
- ASP.NET Core docs: https://learn.microsoft.com/en-us/aspnet/core/
- .NET samples: https://github.com/dotnet/samples

## Next Steps
After completing this plan, consider:
- Adding authentication/authorization
- Implementing real-time updates with SignalR
- Containerizing with Docker
- Deploying to Azure App Service