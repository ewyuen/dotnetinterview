# Lab 4.3: Async Streams

## Goal
Learn asynchronous streams using IAsyncEnumerable and async iterators for processing sequences of data asynchronously.

## Key Concepts
- **IAsyncEnumerable&lt;T&gt;**: Asynchronous version of IEnumerable&lt;T&gt;
- **async iterators**: Methods that yield values asynchronously
- **await foreach**: Asynchronous iteration syntax
- **Cancellation**: Cancelling async stream enumeration
- **Async LINQ**: LINQ operations on async streams
- **Memory efficiency**: Processing large datasets without loading everything into memory

## Implementation
This lab demonstrates essential async stream patterns:

1. **Basic Async Iterator**: Creating and consuming async streams
2. **Cancellation**: Cancelling async stream operations
3. **File Processing**: Reading large files line-by-line asynchronously
4. **Async LINQ**: WhereAwait, SelectAwait, and AggregateAsync operations
5. **Infinite Streams**: Continuous data generation with cancellation
6. **Stream Combination**: Interleaving and merging multiple async streams
7. **Custom Implementation**: Building custom async enumerable classes

## Key Features Demonstrated
- `async IAsyncEnumerable<T>` method syntax
- `yield return` in async methods
- `await foreach` for consumption
- `[EnumeratorCancellation]` attribute for proper cancellation
- Custom async LINQ extension methods
- Memory-efficient large file processing
- Stream interleaving and merging patterns
- Fibonacci sequence generation with async delays

## Expected Output
The program will show:
- Sequential async value generation and consumption
- Cancellation of async stream enumeration
- Line-by-line file processing without memory issues
- Async filtering, transformation, and aggregation
- Timestamp generation with automatic cancellation
- Combined stream operations (interleaving and merging)
- Custom async enumerable implementation

## Important Notes
- Use async streams for large or infinite data sequences
- Always support cancellation in async iterators
- Use `[EnumeratorCancellation]` for proper cancellation token flow
- Prefer async streams over synchronous collections for I/O-bound operations
- Async LINQ operations require custom extension methods
- `await foreach` automatically handles disposal of enumerators
- Combine async streams with backpressure for resource management

## References
- [IAsyncEnumerable&lt;T&gt; Interface](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.iasyncenumerable-1)
- [Async Streams (C#)](https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-8#asynchronous-streams)
- [How to: Consume an Async Stream](https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-8#how-to-consume-an-async-stream)