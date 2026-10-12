using System.Diagnostics;
using System.Text;

namespace Lab4._1_AsyncFileIO;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("=== Lab 4.1: Async File I/O ===\n");

        // Create test data
        await CreateTestDataAsync();

        // Demonstrate basic async file operations
        await DemonstrateBasicFileOperationsAsync();

        // Compare performance between sync and async
        await CompareSyncVsAsyncPerformanceAsync();

        // Handle large files with streaming
        await ProcessLargeFileAsync();

        // Demonstrate error handling
        await DemonstrateErrorHandlingAsync();

        // Show cancellation support
        await DemonstrateCancellationAsync();

        Console.WriteLine("\n=== Lab Complete ===");
    }

    static async Task CreateTestDataAsync()
    {
        Console.WriteLine("Creating test data...");

        // Create a small test file
        string smallFilePath = "small_test.txt";
        string smallContent = "This is a small test file for async I/O operations.\n" +
                             "It contains multiple lines of text.\n" +
                             "Used for basic read/write demonstrations.";

        await File.WriteAllTextAsync(smallFilePath, smallContent);

        // Create a medium test file for performance comparison
        string mediumFilePath = "medium_test.txt";
        var mediumContent = new StringBuilder();
        for (int i = 0; i < 1000; i++)
        {
            mediumContent.AppendLine($"Line {i}: This is test data for performance comparison. {Guid.NewGuid()}");
        }
        await File.WriteAllTextAsync(mediumFilePath, mediumContent.ToString());

        Console.WriteLine("Test data created successfully.\n");
    }

    static async Task DemonstrateBasicFileOperationsAsync()
    {
        Console.WriteLine("1. Basic Async File Operations");
        Console.WriteLine("------------------------------");

        string filePath = "basic_demo.txt";
        string content = "Hello, Async World!\nThis file was written asynchronously.";

        // Write file asynchronously
        Console.WriteLine("Writing file asynchronously...");
        await File.WriteAllTextAsync(filePath, content);

        // Read file asynchronously
        Console.WriteLine("Reading file asynchronously...");
        string readContent = await File.ReadAllTextAsync(filePath);
        Console.WriteLine($"File content: {readContent}");

        // Read all lines asynchronously
        Console.WriteLine("Reading all lines asynchronously...");
        string[] lines = await File.ReadAllLinesAsync(filePath);
        Console.WriteLine($"Number of lines: {lines.Length}");

        // Append to file
        Console.WriteLine("Appending to file asynchronously...");
        await File.AppendAllTextAsync(filePath, "\nAppended line asynchronously.");

        // Read again to verify
        string updatedContent = await File.ReadAllTextAsync(filePath);
        Console.WriteLine($"Updated content: {updatedContent}");

        Console.WriteLine();
    }

    static async Task CompareSyncVsAsyncPerformanceAsync()
    {
        Console.WriteLine("2. Performance Comparison: Sync vs Async");
        Console.WriteLine("------------------------------------------");

        string filePath = "medium_test.txt";

        // Synchronous operations
        Console.WriteLine("Testing synchronous operations...");
        var syncStopwatch = Stopwatch.StartNew();
        for (int i = 0; i < 10; i++)
        {
            string content = File.ReadAllText(filePath);
            File.WriteAllText($"sync_temp_{i}.txt", content);
        }
        syncStopwatch.Stop();
        Console.WriteLine($"Synchronous operations took: {syncStopwatch.ElapsedMilliseconds}ms");

        // Clean up sync temp files
        for (int i = 0; i < 10; i++)
        {
            File.Delete($"sync_temp_{i}.txt");
        }

        // Asynchronous operations
        Console.WriteLine("Testing asynchronous operations...");
        var asyncStopwatch = Stopwatch.StartNew();
        var tasks = new List<Task>();
        for (int i = 0; i < 10; i++)
        {
            tasks.Add(ProcessFileAsync(filePath, $"async_temp_{i}.txt"));
        }
        await Task.WhenAll(tasks);
        asyncStopwatch.Stop();
        Console.WriteLine($"Asynchronous operations took: {asyncStopwatch.ElapsedMilliseconds}ms");

        // Clean up async temp files
        for (int i = 0; i < 10; i++)
        {
            File.Delete($"async_temp_{i}.txt");
        }

        Console.WriteLine();
    }

    static async Task ProcessFileAsync(string sourcePath, string destPath)
    {
        string content = await File.ReadAllTextAsync(sourcePath);
        await File.WriteAllTextAsync(destPath, content);
    }

    static async Task ProcessLargeFileAsync()
    {
        Console.WriteLine("3. Large File Processing with Streaming");
        Console.WriteLine("---------------------------------------");

        string largeFilePath = "large_test.txt";

        // Create a large file (approximately 10MB)
        Console.WriteLine("Creating large test file...");
        var largeContent = new StringBuilder();
        for (int i = 0; i < 100000; i++)
        {
            largeContent.AppendLine($"Line {i}: Large file content for streaming demonstration. {Guid.NewGuid()}");
        }
        await File.WriteAllTextAsync(largeFilePath, largeContent.ToString());

        // Process large file with streaming
        Console.WriteLine("Processing large file with streaming...");
        var progress = new Progress<int>(percent =>
        {
            Console.Write($"\rProcessing progress: {percent}%");
        });

        int lineCount = await CountLinesWithProgressAsync(largeFilePath, progress);
        Console.WriteLine($"\nLarge file has {lineCount} lines");

        // Demonstrate memory-efficient reading
        Console.WriteLine("Reading first 10 lines efficiently...");
        var firstLines = await ReadFirstLinesAsync(largeFilePath, 10);
        foreach (var line in firstLines)
        {
            Console.WriteLine($"  {line}");
        }

        Console.WriteLine();
    }

    static async Task<int> CountLinesWithProgressAsync(string filePath, IProgress<int> progress)
    {
        int totalLines = 0;
        int linesRead = 0;

        using (var stream = File.OpenRead(filePath))
        using (var reader = new StreamReader(stream))
        {
            string? line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                totalLines++;
                linesRead++;

                // Report progress every 1000 lines
                if (linesRead % 1000 == 0)
                {
                    int percent = (int)((double)stream.Position / stream.Length * 100);
                    progress.Report(percent);
                }
            }
        }

        progress.Report(100);
        return totalLines;
    }

    static async Task<List<string>> ReadFirstLinesAsync(string filePath, int count)
    {
        var lines = new List<string>();

        using (var stream = File.OpenRead(filePath))
        using (var reader = new StreamReader(stream))
        {
            string? line;
            int readCount = 0;
            while ((line = await reader.ReadLineAsync()) != null && readCount < count)
            {
                lines.Add(line);
                readCount++;
            }
        }

        return lines;
    }

    static async Task DemonstrateErrorHandlingAsync()
    {
        Console.WriteLine("4. Error Handling in Async File Operations");
        Console.WriteLine("------------------------------------------");

        // Try to read non-existent file
        Console.WriteLine("Attempting to read non-existent file...");
        try
        {
            await File.ReadAllTextAsync("non_existent_file.txt");
        }
        catch (FileNotFoundException ex)
        {
            Console.WriteLine($"FileNotFoundException caught: {ex.Message}");
        }

        // Try to write to read-only location (if possible)
        Console.WriteLine("Attempting to write to invalid location...");
        try
        {
            await File.WriteAllTextAsync("C:\\Windows\\System32\\test.txt", "This should fail");
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.WriteLine($"UnauthorizedAccessException caught: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Other exception caught: {ex.GetType().Name}: {ex.Message}");
        }

        Console.WriteLine();
    }

    static async Task DemonstrateCancellationAsync()
    {
        Console.WriteLine("5. Cancellation Support in Async Operations");
        Console.WriteLine("--------------------------------------------");

        var cts = new CancellationTokenSource();

        // Start a long-running operation
        Console.WriteLine("Starting cancellable file operation...");
        var operationTask = PerformLongRunningFileOperationAsync(cts.Token);

        // Cancel after 2 seconds
        await Task.Delay(2000);
        Console.WriteLine("Cancelling operation...");
        cts.Cancel();

        try
        {
            await operationTask;
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Operation was cancelled successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
        }

        Console.WriteLine();
    }

    static async Task PerformLongRunningFileOperationAsync(CancellationToken cancellationToken)
    {
        // Simulate a long-running file operation
        for (int i = 0; i < 100; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            // Simulate file processing work
            await Task.Delay(100, cancellationToken);

            if (i % 10 == 0)
            {
                Console.WriteLine($"  Processing chunk {i}...");
            }
        }

        Console.WriteLine("Long-running operation completed");
    }
}