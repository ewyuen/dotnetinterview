# Lab 3.3: Parallel Programming

## Goal
Learn parallel programming concepts using Parallel LINQ (PLINQ) and Parallel class methods in C#.

## Key Concepts
- **Parallel.ForEach**: Parallel iteration over collections
- **Parallel LINQ (PLINQ)**: Parallel execution of LINQ queries
- **Parallel.For**: Parallel for loops with thread-local state
- **Parallel Options**: Controlling degree of parallelism and cancellation
- **Exception Handling**: Managing exceptions in parallel operations
- **Performance Considerations**: When to use parallel processing

## Implementation
This lab demonstrates essential parallel programming concepts:

1. **Parallel.ForEach**: Processing collections in parallel with thread safety
2. **PLINQ Queries**: Converting LINQ to parallel with `.AsParallel()`
3. **Parallel.For**: Advanced parallel loops with local state management
4. **Parallel Options**: Configuring parallelism and cancellation
5. **Exception Handling**: Using `AggregateException` for parallel errors
6. **Performance Analysis**: Comparing sequential vs parallel execution

## Key Features Demonstrated
- `Parallel.ForEach()` for parallel collection processing
- `AsParallel()` extension for LINQ parallelization
- `Parallel.For()` with thread-local state and final actions
- `ParallelOptions` for controlling concurrency
- `CancellationToken` integration with parallel operations
- `AggregateException` handling for multiple parallel exceptions
- Performance benchmarking of parallel vs sequential code

## Expected Output
The program will show:
- Items processed on different threads simultaneously
- PLINQ query results with performance comparisons
- Parallel.For operations with thread-local computations
- Controlled parallelism with cancellation
- Exception aggregation from parallel operations
- Performance metrics comparing sequential and parallel execution

## Important Notes
- Parallel processing adds overhead - use only for CPU-intensive work
- PLINQ doesn't guarantee ordering unless `AsOrdered()` is used
- Thread safety is crucial when sharing state between parallel operations
- Use `AggregateException` to handle multiple exceptions from parallel tasks
- Cancellation is cooperative and requires checking tokens
- Measure performance to ensure parallel execution provides benefits

## References
- [Parallel Programming in .NET](https://docs.microsoft.com/en-us/dotnet/standard/parallel-programming/)
- [Parallel LINQ (PLINQ)](https://docs.microsoft.com/en-us/dotnet/standard/parallel-programming/parallel-linq-plinq)
- [Parallel Class](https://docs.microsoft.com/en-us/dotnet/api/system.threading.tasks.parallel)