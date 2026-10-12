# Lab 3.2: Task-Based Programming

## Goal
Learn Task-based Asynchronous Programming (TAP) using the Task Parallel Library (TPL) in C#.

## Key Concepts
- **Task Creation**: Using `Task.Run()` and `Task.Factory.StartNew()`
- **Task Continuation**: Chaining tasks with `ContinueWith()`
- **Task Results**: Getting results from asynchronous operations
- **Exception Handling**: Handling exceptions in tasks
- **Cancellation**: Cancelling long-running operations
- **Task Coordination**: Using `Task.WhenAll()` and `Task.WhenAny()`

## Implementation
This lab demonstrates essential Task-based programming concepts:

1. **Basic Task Creation**: Creating and running tasks with `Task.Run()`
2. **Task Continuation**: Building task chains with continuation
3. **Tasks with Results**: Returning values from asynchronous operations
4. **Exception Handling**: Proper exception handling in concurrent code
5. **Task Cancellation**: Using `CancellationToken` for cooperative cancellation
6. **Parallel Processing**: Running multiple tasks concurrently

## Key Features Demonstrated
- `Task.Run()` for fire-and-forget operations
- `Task.ContinueWith()` for task chaining
- `Task.WhenAll()` and `Task.WhenAny()` for coordination
- `CancellationTokenSource` and `CancellationToken` for cancellation
- Exception propagation and handling in tasks
- Task-based parallel processing patterns

## Expected Output
The program will show:
- Tasks running on different threads
- Continuation execution after primary task completion
- Results from computational tasks
- Exception handling and propagation
- Cancellation of long-running operations
- Parallel execution of multiple tasks

## Important Notes
- Tasks are the preferred way for concurrency in modern C#
- Use `async/await` for simpler asynchronous code (covered in Chapter 4)
- Always handle exceptions in tasks to prevent unobserved exceptions
- Cancellation is cooperative - tasks must check cancellation tokens
- Task scheduling is managed by the thread pool

## References
- [Task-based Asynchronous Pattern (TAP)](https://docs.microsoft.com/en-us/dotnet/standard/asynchronous-programming-patterns/task-based-asynchronous-pattern-tap)
- [Task Parallel Library (TPL)](https://docs.microsoft.com/en-us/dotnet/standard/parallel-programming/task-parallel-library-tpl)