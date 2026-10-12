# Lab 4.3: Async Pipeline

## Goal
Learn to build asynchronous data processing pipelines with proper error handling, progress reporting, and resource management.

## Key Concepts
- **Pipeline Pattern**: Chaining asynchronous operations in sequence
- **Progress Reporting**: Using `IProgress<T>` for operation progress updates
- **Error Propagation**: Handling and recovering from errors in pipeline stages
- **Cancellation**: Supporting cancellation throughout the pipeline
- **Backpressure**: Managing producer-consumer imbalances
- **Resource Management**: Proper cleanup of resources in async pipelines

## Implementation
This lab demonstrates essential async pipeline patterns:

1. **Basic Pipeline**: Simple chained async operations
2. **Progress Reporting**: Pipeline with progress updates
3. **Error Handling**: Pipeline with error recovery and compensation
4. **Cancellation Support**: Pipeline that responds to cancellation
5. **Backpressure Handling**: Managing different processing speeds
6. **Resource Cleanup**: Proper disposal of pipeline resources

## Key Features Demonstrated
- Method chaining with `await` for sequential operations
- `IProgress<T>` for progress reporting through pipeline stages
- Exception handling and compensation logic in pipelines
- `CancellationToken` propagation through pipeline stages
- Producer-consumer patterns with `Channel<T>` or `BlockingCollection<T>`
- `IAsyncDisposable` for proper resource cleanup
- Pipeline composition and reusability

## Expected Output
The program will show:
- Sequential pipeline execution with progress updates
- Error handling and recovery in pipeline operations
- Cancellation of long-running pipeline operations
- Backpressure handling in producer-consumer scenarios
- Proper resource cleanup and disposal

## Important Notes
- Design pipelines to be composable and reusable
- Always support cancellation in long-running operations
- Use appropriate error handling strategies for each pipeline stage
- Consider backpressure when producer and consumer speeds differ
- Implement proper resource disposal with IAsyncDisposable
- Use progress reporting for long-running operations
- Test pipeline error scenarios thoroughly

## References
- [Task-based Asynchronous Pattern (TAP)](https://docs.microsoft.com/en-us/dotnet/standard/asynchronous-programming-patterns/task-based-asynchronous-pattern-tap)
- [Channels - producer/consumer queues](https://docs.microsoft.com/en-us/dotnet/core/extensions/channels)
- [IProgress<T> Interface](https://docs.microsoft.com/en-us/dotnet/api/system.iprogress-1)