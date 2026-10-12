using System.Collections.Concurrent;
using System.Threading.Channels;

namespace Lab4._3_AsyncPipeline;

public class DataItem
{
    public int Id { get; set; }
    public string Data { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsProcessed { get; set; }
    public string? Error { get; set; }
}

public class PipelineResult
{
    public int ProcessedItems { get; set; }
    public int FailedItems { get; set; }
    public TimeSpan Duration { get; set; }
    public List<string> Errors { get; set; } = new();
}

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("=== Lab 4.3: Async Pipeline ===\n");

        // Demonstrate basic pipeline
        await DemonstrateBasicPipelineAsync();

        // Show pipeline with progress reporting
        await DemonstrateProgressReportingPipelineAsync();

        // Demonstrate error handling in pipelines
        await DemonstrateErrorHandlingPipelineAsync();

        // Show cancellation support
        await DemonstrateCancellationPipelineAsync();

        // Demonstrate producer-consumer with backpressure
        await DemonstrateProducerConsumerPipelineAsync();

        // Show resource cleanup
        await DemonstrateResourceCleanupAsync();

        Console.WriteLine("\n=== Lab Complete ===");
    }

    static async Task DemonstrateBasicPipelineAsync()
    {
        Console.WriteLine("1. Basic Async Pipeline");
        Console.WriteLine("-----------------------");

        var items = GenerateTestData(5);

        Console.WriteLine("Processing items through basic pipeline...");
        var results = new List<DataItem>();

        foreach (var item in items)
        {
            var result = await ProcessItemThroughPipelineAsync(item);
            results.Add(result);
            Console.WriteLine($"  Processed item {result.Id}: {result.IsProcessed}");
        }

        Console.WriteLine($"Pipeline completed: {results.Count(r => r.IsProcessed)}/{results.Count} items processed");
        Console.WriteLine();
    }

    static async Task<DataItem> ProcessItemThroughPipelineAsync(DataItem item)
    {
        // Stage 1: Validation
        await Task.Delay(100); // Simulate validation time
        if (string.IsNullOrEmpty(item.Data))
        {
            item.Error = "Data is empty";
            return item;
        }

        // Stage 2: Transformation
        await Task.Delay(150); // Simulate transformation time
        item.Data = item.Data.ToUpper();

        // Stage 3: Processing
        await Task.Delay(200); // Simulate processing time
        item.IsProcessed = true;

        return item;
    }

    static async Task DemonstrateProgressReportingPipelineAsync()
    {
        Console.WriteLine("2. Pipeline with Progress Reporting");
        Console.WriteLine("------------------------------------");

        var items = GenerateTestData(10);
        var progress = new Progress<PipelineProgress>(p =>
        {
            Console.WriteLine($"  Progress: Stage {p.CurrentStage}, Item {p.CurrentItem}/{p.TotalItems} ({p.Percentage}%)");
        });

        Console.WriteLine("Processing items with progress reporting...");
        var result = await ProcessItemsWithProgressAsync(items, progress);

        Console.WriteLine($"Progress pipeline completed: {result.ProcessedItems}/{result.ProcessedItems + result.FailedItems} items processed in {result.Duration.TotalSeconds:F2}s");
        Console.WriteLine();
    }

    static async Task<PipelineResult> ProcessItemsWithProgressAsync(List<DataItem> items, IProgress<PipelineProgress> progress)
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        var result = new PipelineResult();

        for (int i = 0; i < items.Count; i++)
        {
            var item = items[i];

            // Stage 1: Validation
            progress.Report(new PipelineProgress(1, i + 1, items.Count, "Validation"));
            await Task.Delay(100);
            if (string.IsNullOrEmpty(item.Data))
            {
                item.Error = "Data is empty";
                result.FailedItems++;
                continue;
            }

            // Stage 2: Transformation
            progress.Report(new PipelineProgress(2, i + 1, items.Count, "Transformation"));
            await Task.Delay(150);
            item.Data = $"[TRANSFORMED] {item.Data}";

            // Stage 3: Processing
            progress.Report(new PipelineProgress(3, i + 1, items.Count, "Processing"));
            await Task.Delay(200);
            item.IsProcessed = true;
            result.ProcessedItems++;
        }

        stopwatch.Stop();
        result.Duration = stopwatch.Elapsed;
        return result;
    }

    static async Task DemonstrateErrorHandlingPipelineAsync()
    {
        Console.WriteLine("3. Pipeline with Error Handling and Recovery");
        Console.WriteLine("--------------------------------------------");

        var items = GenerateTestData(8);
        // Add some items that will cause errors
        items.AddRange(new[]
        {
            new DataItem { Id = 9, Data = "", Error = "Intentionally empty" },
            new DataItem { Id = 10, Data = "valid data" }
        });

        Console.WriteLine("Processing items with error handling and recovery...");
        var result = await ProcessItemsWithErrorHandlingAsync(items);

        Console.WriteLine($"Error handling pipeline completed:");
        Console.WriteLine($"  Processed: {result.ProcessedItems}");
        Console.WriteLine($"  Failed: {result.FailedItems}");
        Console.WriteLine($"  Duration: {result.Duration.TotalSeconds:F2}s");

        if (result.Errors.Any())
        {
            Console.WriteLine("  Errors encountered:");
            foreach (var error in result.Errors)
            {
                Console.WriteLine($"    - {error}");
            }
        }

        Console.WriteLine();
    }

    static async Task<PipelineResult> ProcessItemsWithErrorHandlingAsync(List<DataItem> items)
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        var result = new PipelineResult();

        foreach (var item in items)
        {
            try
            {
                // Stage 1: Validation with error handling
                await ValidateItemAsync(item);

                // Stage 2: Transformation with compensation
                var originalData = item.Data;
                await TransformItemAsync(item);

                // Stage 3: Processing with rollback on failure
                await ProcessItemAsync(item);

                result.ProcessedItems++;
            }
            catch (ValidationException ex)
            {
                item.Error = ex.Message;
                result.FailedItems++;
                result.Errors.Add($"Validation failed for item {item.Id}: {ex.Message}");
            }
            catch (TransformationException ex)
            {
                item.Error = ex.Message;
                // Attempt compensation
                await CompensateTransformationAsync(item);
                result.FailedItems++;
                result.Errors.Add($"Transformation failed for item {item.Id}: {ex.Message}");
            }
            catch (ProcessingException ex)
            {
                item.Error = ex.Message;
                // Attempt rollback
                await RollbackProcessingAsync(item);
                result.FailedItems++;
                result.Errors.Add($"Processing failed for item {item.Id}: {ex.Message}");
            }
            catch (Exception ex)
            {
                item.Error = $"Unexpected error: {ex.Message}";
                result.FailedItems++;
                result.Errors.Add($"Unexpected error for item {item.Id}: {ex.Message}");
            }
        }

        stopwatch.Stop();
        result.Duration = stopwatch.Elapsed;
        return result;
    }

    static async Task DemonstrateCancellationPipelineAsync()
    {
        Console.WriteLine("4. Pipeline with Cancellation Support");
        Console.WriteLine("-------------------------------------");

        var cts = new CancellationTokenSource();
        var items = GenerateTestData(20); // More items to allow for cancellation

        // Start pipeline
        Console.WriteLine("Starting cancellable pipeline...");
        var pipelineTask = ProcessItemsWithCancellationAsync(items, cts.Token);

        // Cancel after 3 seconds
        await Task.Delay(3000);
        Console.WriteLine("Cancelling pipeline...");
        cts.Cancel();

        try
        {
            var result = await pipelineTask;
            Console.WriteLine("Pipeline completed (unexpected)");
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Pipeline was cancelled successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
        }

        Console.WriteLine();
    }

    static async Task<PipelineResult> ProcessItemsWithCancellationAsync(List<DataItem> items, CancellationToken cancellationToken)
    {
        var result = new PipelineResult();
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        foreach (var item in items)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                // Each stage checks for cancellation
                await ValidateItemAsync(item, cancellationToken);
                await TransformItemAsync(item, cancellationToken);
                await ProcessItemAsync(item, cancellationToken);

                result.ProcessedItems++;
                Console.WriteLine($"  Processed item {item.Id}");
            }
            catch (OperationCanceledException)
            {
                throw; // Re-throw cancellation
            }
            catch (Exception ex)
            {
                item.Error = ex.Message;
                result.FailedItems++;
                result.Errors.Add($"Failed to process item {item.Id}: {ex.Message}");
            }
        }

        stopwatch.Stop();
        result.Duration = stopwatch.Elapsed;
        return result;
    }

    static async Task DemonstrateProducerConsumerPipelineAsync()
    {
        Console.WriteLine("5. Producer-Consumer Pipeline with Backpressure");
        Console.WriteLine("------------------------------------------------");

        const int itemCount = 15;
        const int bufferSize = 5; // Small buffer to demonstrate backpressure

        // Create bounded channel for backpressure
        var channel = Channel.CreateBounded<DataItem>(new BoundedChannelOptions(bufferSize)
        {
            FullMode = BoundedChannelFullMode.Wait
        });

        var cts = new CancellationTokenSource();
        var token = cts.Token;

        Console.WriteLine($"Starting producer-consumer pipeline (buffer size: {bufferSize})...");

        // Start consumer
        var consumerTask = Task.Run(async () =>
        {
            await foreach (var item in channel.Reader.ReadAllAsync(token))
            {
                Console.WriteLine($"  Consumer: Processing item {item.Id}");
                await ProcessItemAsync(item, token);
                await Task.Delay(500, token); // Simulate slower consumer
            }
        });

        // Start producer
        var producerTask = Task.Run(async () =>
        {
            for (int i = 1; i <= itemCount; i++)
            {
                var item = new DataItem { Id = i, Data = $"Producer data {i}" };
                Console.WriteLine($"  Producer: Writing item {item.Id}");

                await channel.Writer.WriteAsync(item, token);
                await Task.Delay(200, token); // Simulate producer timing
            }

            channel.Writer.Complete();
        });

        // Wait for both to complete
        await Task.WhenAll(producerTask, consumerTask);

        Console.WriteLine("Producer-consumer pipeline completed");
        Console.WriteLine();
    }

    static async Task DemonstrateResourceCleanupAsync()
    {
        Console.WriteLine("6. Pipeline with Resource Cleanup");
        Console.WriteLine("----------------------------------");

        Console.WriteLine("Processing items with proper resource cleanup...");

        await using (var pipeline = new AsyncPipeline())
        {
            var items = GenerateTestData(5);
            var result = await pipeline.ProcessItemsAsync(items);

            Console.WriteLine($"Resource cleanup pipeline completed:");
            Console.WriteLine($"  Processed: {result.ProcessedItems}");
            Console.WriteLine($"  Failed: {result.FailedItems}");
            Console.WriteLine($"  Resources disposed: {pipeline.ResourcesDisposed}");
        }

        Console.WriteLine();
    }

    // Helper methods for pipeline stages
    static async Task ValidateItemAsync(DataItem item, CancellationToken token = default)
    {
        await Task.Delay(50, token);
        if (string.IsNullOrEmpty(item.Data))
        {
            throw new ValidationException("Item data cannot be empty");
        }
    }

    static async Task TransformItemAsync(DataItem item, CancellationToken token = default)
    {
        await Task.Delay(75, token);
        item.Data = $"[PROCESSED] {item.Data.ToUpper()}";
    }

    static async Task ProcessItemAsync(DataItem item, CancellationToken token = default)
    {
        await Task.Delay(100, token);
        item.IsProcessed = true;
    }

    static async Task CompensateTransformationAsync(DataItem item)
    {
        await Task.Delay(25);
        // Compensation logic would go here
    }

    static async Task RollbackProcessingAsync(DataItem item)
    {
        await Task.Delay(25);
        item.IsProcessed = false;
        // Rollback logic would go here
    }

    static List<DataItem> GenerateTestData(int count)
    {
        return Enumerable.Range(1, count)
            .Select(i => new DataItem
            {
                Id = i,
                Data = $"Test data {i}"
            })
            .ToList();
    }
}

public class PipelineProgress
{
    public int CurrentStage { get; }
    public int CurrentItem { get; }
    public int TotalItems { get; }
    public string StageName { get; }
    public int Percentage => (int)((double)CurrentItem / TotalItems * 100);

    public PipelineProgress(int stage, int currentItem, int totalItems, string stageName)
    {
        CurrentStage = stage;
        CurrentItem = currentItem;
        TotalItems = totalItems;
        StageName = stageName;
    }
}

public class ValidationException : Exception
{
    public ValidationException(string message) : base(message) { }
}

public class TransformationException : Exception
{
    public TransformationException(string message) : base(message) { }
}

public class ProcessingException : Exception
{
    public ProcessingException(string message) : base(message) { }
}

public class AsyncPipeline : IAsyncDisposable
{
    private readonly List<IAsyncDisposable> _resources = new();
    public int ResourcesDisposed { get; private set; }

    public async Task<PipelineResult> ProcessItemsAsync(List<DataItem> items)
    {
        var result = new PipelineResult();

        // Simulate acquiring resources
        for (int i = 0; i < 3; i++)
        {
            _resources.Add(new AsyncResource($"Resource-{i}"));
        }

        foreach (var item in items)
        {
            try
            {
                await ValidateItemAsync(item);
                await TransformItemAsync(item);
                await ProcessItemAsync(item);
                result.ProcessedItems++;
            }
            catch (Exception)
            {
                result.FailedItems++;
            }
        }

        return result;
    }

    public async ValueTask DisposeAsync()
    {
        foreach (var resource in _resources)
        {
            await resource.DisposeAsync();
            ResourcesDisposed++;
        }
        _resources.Clear();
    }
}

public class AsyncResource : IAsyncDisposable
{
    private readonly string _name;

    public AsyncResource(string name)
    {
        _name = name;
        Console.WriteLine($"  Acquired resource: {_name}");
    }

    public async ValueTask DisposeAsync()
    {
        await Task.Delay(10); // Simulate async cleanup
        Console.WriteLine($"  Disposed resource: {_name}");
    }
}