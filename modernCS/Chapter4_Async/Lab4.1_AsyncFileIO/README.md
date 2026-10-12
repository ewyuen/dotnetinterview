# Lab 4.1: Async File I/O

## Goal
Learn asynchronous file operations in C# and understand the performance benefits over synchronous operations.

## Key Concepts
- **Async File I/O**: Non-blocking file operations using `File.ReadAllTextAsync()`, `File.WriteAllTextAsync()`
- **Performance Comparison**: Comparing async vs sync file operations
- **Error Handling**: Proper exception handling in async file operations
- **Large File Processing**: Handling large files efficiently with streaming
- **Cancellation**: Supporting cancellation in long-running file operations

## Implementation
This lab demonstrates essential async file I/O patterns:

1. **Basic Async File Operations**: Reading and writing files asynchronously
2. **Performance Comparison**: Measuring sync vs async performance
3. **Large File Processing**: Streaming large files without loading into memory
4. **Error Handling**: Proper exception handling for file operations
5. **Cancellation Support**: Cancelling long-running file operations
6. **Progress Reporting**: Reporting progress for large file operations

## Key Features Demonstrated
- `File.ReadAllTextAsync()` and `File.WriteAllTextAsync()`
- `File.ReadAllLinesAsync()` and `File.WriteAllLinesAsync()`
- `File.OpenRead()` with `StreamReader` for large files
- Performance benchmarking with `Stopwatch`
- `CancellationToken` support in async operations
- `IProgress<T>` for progress reporting
- Proper resource disposal with `using` statements

## Expected Output
The program will show:
- Performance comparison between sync and async file operations
- Successful file read/write operations
- Large file processing with progress updates
- Error handling demonstrations
- Cancellation of long-running operations

## Important Notes
- Always prefer async file I/O methods over synchronous ones
- Use streaming for large files to avoid memory issues
- Always dispose of file streams properly
- Handle common file exceptions (FileNotFoundException, UnauthorizedAccessException, etc.)
- Use CancellationToken for cancellable operations
- Consider using `FileStream` with `BufferedStream` for optimal performance

## References
- [File and Stream I/O](https://docs.microsoft.com/en-us/dotnet/standard/io/)
- [Asynchronous File I/O](https://docs.microsoft.com/en-us/dotnet/standard/io/asynchronous-file-i-o)
- [Cancellation in Managed Threads](https://docs.microsoft.com/en-us/dotnet/standard/threading/cancellation-in-managed-threads)