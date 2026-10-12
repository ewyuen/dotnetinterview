# Lab 4.4: Advanced Async Scenarios

## Goal
Learn advanced asynchronous programming concepts including ValueTask, custom awaitables, and low-level async infrastructure.

## Key Concepts
- **ValueTask&lt;T&gt;**: High-performance alternative to Task&lt;T&gt; for synchronous completions
- **Custom Awaitables**: Implementing the awaitable pattern with GetAwaiter()
- **Async Method Builders**: Custom infrastructure for async methods
- **TaskCompletionSource**: Manual task creation and completion
- **AsyncLocal&lt;T&gt;**: Ambient data that flows with async operations
- **Dynamic Parallelism**: Runtime task creation and management
- **SynchronizationContext**: Custom threading models for async operations

## Implementation
This lab demonstrates advanced async programming techniques:

1. **ValueTask Performance**: Comparing Task vs ValueTask performance
2. **Custom Awaitables**: Building awaitable types from scratch
3. **Async Method Builders**: Custom async infrastructure
4. **TaskCompletionSource**: Manual task lifecycle management
5. **AsyncLocal Storage**: Context flowing through async operations
6. **Dynamic Parallelism**: Producer-consumer patterns with tasks
7. **Custom Sync Context**: Implementing custom threading models

## Key Features Demonstrated
- `ValueTask<T>` for avoiding allocations in fast paths
- `INotifyCompletion` and `GetAwaiter()` pattern
- `[AsyncMethodBuilder]` attribute for custom async methods
- `TaskCompletionSource<T>` for advanced task control
- `AsyncLocal<T>` for ambient context
- Dynamic task creation and semaphore-based coordination
- Custom `SynchronizationContext` implementation

## Expected Output
The program will show:
- ValueTask performance benefits over Task
- Custom awaitable types with different behaviors
- Custom async method execution
- Manual task completion scenarios
- AsyncLocal context inheritance
- Dynamic parallel processing results
- Custom synchronization context operation counts

## Important Notes
- Use ValueTask only when results are often synchronous
- Custom awaitables must implement the awaiter pattern correctly
- TaskCompletionSource is powerful but requires careful lifecycle management
- AsyncLocal enables context flowing through async call chains
- SynchronizationContext controls where async continuations execute
- Custom async infrastructure is rarely needed in application code

## References
- [ValueTask&lt;T&gt; Struct](https://docs.microsoft.com/en-us/dotnet/api/system.threading.tasks.valuetask-1)
- [TaskCompletionSource&lt;T&gt; Class](https://docs.microsoft.com/en-us/dotnet/api/system.threading.tasks.taskcompletionsource-1)
- [AsyncLocal&lt;T&gt; Class](https://docs.microsoft.com/en-us/dotnet/api/system.threading.asynclocal-1)
- [Awaitable Pattern](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/expressions#awaitable-expressions)