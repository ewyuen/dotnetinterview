# Lab 4.2: Async Patterns and Best Practices

## Goal
Learn advanced async programming patterns, error handling, cancellation, and best practices for robust asynchronous code.

## Key Concepts
- **Exception Handling**: Proper exception handling in async methods
- **Cancellation**: Cooperative cancellation with CancellationToken
- **Timeout Patterns**: Implementing timeouts for async operations
- **Retry Logic**: Implementing retry patterns with exponential backoff
- **Progress Reporting**: IProgress&lt;T&gt; for async operation progress
- **Async Lazy**: Lazy initialization of async resources
- **Common Pitfalls**: Avoiding deadlocks and other async anti-patterns

## Implementation
This lab demonstrates essential async patterns and best practices:

1. **Exception Handling**: Catching exceptions from async operations
2. **Cancellation**: Using CancellationToken for cooperative cancellation
3. **Timeout Patterns**: Implementing timeouts with CancellationTokenSource and Task.WhenAny
4. **Retry Patterns**: Exponential backoff retry logic
5. **Progress Reporting**: Using IProgress&lt;T&gt; for operation progress
6. **Async Lazy**: Thread-safe lazy initialization of async resources
7. **Common Pitfalls**: Demonstrating and avoiding async anti-patterns

## Key Features Demonstrated
- `AggregateException` handling from `Task.WhenAll()`
- `CancellationToken.ThrowIfCancellationRequested()`
- Linked cancellation tokens with `CreateLinkedTokenSource()`
- Timeout implementation using `Task.WhenAny()`
- Exponential backoff retry patterns
- `IProgress<T>` for progress reporting
- Custom `AsyncLazy<T>` implementation
- Avoiding `async void` in non-event handlers
- Proper async/await usage without blocking

## Expected Output
The program will show:
- Exception handling from async operations
- Cancellation of long-running operations
- Timeout behavior with different patterns
- Retry logic with exponential backoff
- Progress updates during operations
- Lazy initialization behavior
- Demonstration of common pitfalls (including unhandled exceptions)

## Important Notes
- Always handle exceptions in async methods
- Use CancellationToken for cooperative cancellation
- Prefer Task.WhenAny for timeouts over blocking waits
- Implement retry logic with exponential backoff
- Use IProgress&lt;T&gt; for progress reporting in async operations
- Avoid async void except for event handlers
- Never use .Result or .Wait() on async operations (can cause deadlocks)
- Multiple awaits on the same Task are safe and efficient

## References
- [Cancellation in Managed Threads](https://docs.microsoft.com/en-us/dotnet/standard/threading/cancellation-in-managed-threads)
- [Async/Await Best Practices](https://docs.microsoft.com/en-us/archive/msdn-magazine/2013/march/async-await-best-practices)
- [Implementing the Task-based Asynchronous Pattern](https://docs.microsoft.com/en-us/dotnet/standard/asynchronous-programming-patterns/implementing-the-task-based-asynchronous-pattern)