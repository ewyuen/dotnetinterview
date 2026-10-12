using System.Collections.Concurrent;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace FinalComponent_AsyncOperations;

public record TaskItem(
    Guid Id,
    string Title,
    string Description,
    string Status,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    DateTime? DueDate,
    bool IsCompleted
);

public record CreateTaskRequest(
    string Title,
    string Description,
    DateTime? DueDate
);

public record UpdateTaskRequest(
    string? Title,
    string? Description,
    string? Status,
    DateTime? DueDate
);

public class TaskService
{
    private readonly string _dataFile = "tasks.json";
    private readonly HttpClient _httpClient;
    private readonly SemaphoreSlim _fileLock = new(1, 1);

    public TaskService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<TaskItem>> GetAllTasksAsync()
    {
        await using var _ = await _fileLock.WaitAsyncDisposable();

        if (!File.Exists(_dataFile))
            return new List<TaskItem>();

        var json = await File.ReadAllTextAsync(_dataFile);
        return JsonSerializer.Deserialize<List<TaskItem>>(json) ?? new List<TaskItem>();
    }

    public async Task<TaskItem?> GetTaskByIdAsync(Guid id)
    {
        var tasks = await GetAllTasksAsync();
        return tasks.FirstOrDefault(t => t.Id == id);
    }

    public async Task<TaskItem> CreateTaskAsync(CreateTaskRequest request)
    {
        await using var _ = await _fileLock.WaitAsyncDisposable();

        var tasks = await GetAllTasksAsync();
        var newTask = new TaskItem(
            Guid.NewGuid(),
            request.Title,
            request.Description,
            "Created",
            DateTime.UtcNow,
            null,
            request.DueDate,
            false
        );

        tasks.Add(newTask);
        await SaveTasksAsync(tasks);

        return newTask;
    }

    public async Task<TaskItem?> UpdateTaskAsync(Guid id, UpdateTaskRequest request)
    {
        await using var _ = await _fileLock.WaitAsyncDisposable();

        var tasks = await GetAllTasksAsync();
        var taskIndex = tasks.FindIndex(t => t.Id == id);

        if (taskIndex == -1)
            return null;

        var existingTask = tasks[taskIndex];
        var updatedTask = existingTask with
        {
            Title = request.Title ?? existingTask.Title,
            Description = request.Description ?? existingTask.Description,
            Status = request.Status ?? existingTask.Status,
            DueDate = request.DueDate ?? existingTask.DueDate,
            UpdatedAt = DateTime.UtcNow,
            IsCompleted = request.Status == "Completed" || existingTask.IsCompleted
        };

        tasks[taskIndex] = updatedTask;
        await SaveTasksAsync(tasks);

        return updatedTask;
    }

    public async Task<bool> DeleteTaskAsync(Guid id)
    {
        await using var _ = await _fileLock.WaitAsyncDisposable();

        var tasks = await GetAllTasksAsync();
        var initialCount = tasks.Count;
        tasks.RemoveAll(t => t.Id == id);

        if (tasks.Count < initialCount)
        {
            await SaveTasksAsync(tasks);
            return true;
        }

        return false;
    }

    private async Task SaveTasksAsync(List<TaskItem> tasks)
    {
        var json = JsonSerializer.Serialize(tasks, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(_dataFile, json);
    }

    public async Task SyncWithRemoteAsync()
    {
        try
        {
            // Simulate syncing with remote API
            var localTasks = await GetAllTasksAsync();

            // This would normally sync with a real API
            Console.WriteLine($"  Synced {localTasks.Count} tasks with remote server");

            // Simulate API call
            await Task.Delay(500);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"  Sync failed: {ex.Message}");
        }
    }
}

public class TaskProcessingPipeline
{
    private readonly TaskService _taskService;
    private readonly Channel<TaskItem> _processingChannel;

    public TaskProcessingPipeline(TaskService taskService)
    {
        _taskService = taskService;
        _processingChannel = Channel.CreateBounded<TaskItem>(new BoundedChannelOptions(10)
        {
            FullMode = BoundedChannelFullMode.Wait
        });
    }

    public async Task StartProcessingAsync(CancellationToken cancellationToken)
    {
        var tasks = new List<Task>
        {
            ProcessTasksAsync(cancellationToken),
            UpdateOverdueTasksAsync(cancellationToken)
        };

        await Task.WhenAll(tasks);
    }

    public async Task QueueTaskForProcessingAsync(TaskItem task)
    {
        await _processingChannel.Writer.WriteAsync(task);
    }

    public void Complete()
    {
        _processingChannel.Writer.Complete();
    }

    private async Task ProcessTasksAsync(CancellationToken cancellationToken)
    {
        await foreach (var task in _processingChannel.Reader.ReadAllAsync(cancellationToken))
        {
            try
            {
                // Simulate processing pipeline
                await ValidateTaskAsync(task, cancellationToken);
                await ProcessTaskAsync(task, cancellationToken);
                await UpdateTaskStatusAsync(task, cancellationToken);

                Console.WriteLine($"  Processed task: {task.Title}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  Failed to process task {task.Id}: {ex.Message}");
            }
        }
    }

    private async Task UpdateOverdueTasksAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var tasks = await _taskService.GetAllTasksAsync();
                var overdueTasks = tasks.Where(t =>
                    t.DueDate.HasValue &&
                    t.DueDate.Value < DateTime.UtcNow &&
                    !t.IsCompleted &&
                    t.Status != "Completed").ToList();

                foreach (var task in overdueTasks)
                {
                    await _taskService.UpdateTaskAsync(task.Id, new UpdateTaskRequest(Status: "Overdue"));
                    Console.WriteLine($"  Marked task as overdue: {task.Title}");
                }

                await Task.Delay(TimeSpan.FromMinutes(1), cancellationToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  Error in overdue task update: {ex.Message}");
                await Task.Delay(TimeSpan.FromSeconds(30), cancellationToken);
            }
        }
    }

    private async Task ValidateTaskAsync(TaskItem task, CancellationToken cancellationToken)
    {
        await Task.Delay(100, cancellationToken);
        if (string.IsNullOrEmpty(task.Title))
            throw new InvalidOperationException("Task title cannot be empty");
    }

    private async Task ProcessTaskAsync(TaskItem task, CancellationToken cancellationToken)
    {
        await Task.Delay(200, cancellationToken);
        // Simulate processing logic
    }

    private async Task UpdateTaskStatusAsync(TaskItem task, CancellationToken cancellationToken)
    {
        await Task.Delay(100, cancellationToken);
        // Status update would happen here if needed
    }
}

public class RetryPolicy
{
    public async Task<T> ExecuteAsync<T>(Func<Task<T>> operation, int maxRetries = 3)
    {
        var delay = TimeSpan.FromSeconds(1);

        for (int attempt = 1; attempt <= maxRetries + 1; attempt++)
        {
            try
            {
                return await operation();
            }
            catch (Exception ex) when (attempt <= maxRetries)
            {
                Console.WriteLine($"  Attempt {attempt} failed: {ex.Message}. Retrying in {delay.TotalSeconds}s...");
                await Task.Delay(delay);
                delay = delay * 2; // Exponential backoff
            }
        }

        throw new Exception($"Operation failed after {maxRetries + 1} attempts");
    }
}

class Program
{
    private static readonly HttpClient _httpClient = new()
    {
        Timeout = TimeSpan.FromSeconds(30)
    };

    static async Task Main(string[] args)
    {
        Console.WriteLine("=== Final Component: Async Operations ===\n");

        var taskService = new TaskService(_httpClient);
        var pipeline = new TaskProcessingPipeline(taskService);
        var retryPolicy = new RetryPolicy();

        using var cts = new CancellationTokenSource();

        try
        {
            // Start background processing
            var processingTask = pipeline.StartProcessingAsync(cts.Token);

            // Demonstrate task CRUD operations
            await DemonstrateTaskOperationsAsync(taskService, pipeline, retryPolicy);

            // Demonstrate bulk operations
            await DemonstrateBulkOperationsAsync(taskService, pipeline);

            // Demonstrate synchronization
            await DemonstrateSynchronizationAsync(taskService, retryPolicy);

            // Demonstrate concurrent operations
            await DemonstrateConcurrentOperationsAsync(taskService);

            // Stop background processing
            pipeline.Complete();
            await processingTask;

            Console.WriteLine("\n=== Final Component Complete ===");
        }
        finally
        {
            cts.Cancel();
            _httpClient.Dispose();
        }
    }

    static async Task DemonstrateTaskOperationsAsync(TaskService taskService, TaskProcessingPipeline pipeline, RetryPolicy retryPolicy)
    {
        Console.WriteLine("1. Task CRUD Operations");
        Console.WriteLine("-----------------------");

        // Create tasks
        Console.WriteLine("Creating tasks...");
        var task1 = await retryPolicy.ExecuteAsync(() =>
            taskService.CreateTaskAsync(new CreateTaskRequest("Learn Async Programming", "Study async/await patterns", DateTime.UtcNow.AddDays(7))));

        var task2 = await retryPolicy.ExecuteAsync(() =>
            taskService.CreateTaskAsync(new CreateTaskRequest("Build Async Pipeline", "Implement processing pipeline", DateTime.UtcNow.AddDays(3))));

        Console.WriteLine($"Created task: {task1.Title}");
        Console.WriteLine($"Created task: {task2.Title}");

        // Read tasks
        Console.WriteLine("\nReading tasks...");
        var allTasks = await taskService.GetAllTasksAsync();
        Console.WriteLine($"Total tasks: {allTasks.Count}");

        // Update task
        Console.WriteLine("\nUpdating task...");
        var updatedTask = await taskService.UpdateTaskAsync(task1.Id, new UpdateTaskRequest(Status: "In Progress"));
        if (updatedTask != null)
        {
            Console.WriteLine($"Updated task status: {updatedTask.Title} -> {updatedTask.Status}");
        }

        // Queue task for processing
        Console.WriteLine("\nQueueing task for processing...");
        await pipeline.QueueTaskForProcessingAsync(updatedTask!);

        // Delete task
        Console.WriteLine("\nDeleting task...");
        var deleted = await taskService.DeleteTaskAsync(task2.Id);
        Console.WriteLine($"Task deletion {(deleted ? "successful" : "failed")}");

        Console.WriteLine();
    }

    static async Task DemonstrateBulkOperationsAsync(TaskService taskService, TaskProcessingPipeline pipeline)
    {
        Console.WriteLine("2. Bulk Operations with Pipeline");
        Console.WriteLine("--------------------------------");

        // Create bulk tasks
        Console.WriteLine("Creating bulk tasks...");
        var bulkTasks = new List<TaskItem>();
        for (int i = 1; i <= 5; i++)
        {
            var task = await taskService.CreateTaskAsync(new CreateTaskRequest(
                $"Bulk Task {i}",
                $"Description for bulk task {i}",
                DateTime.UtcNow.AddDays(i)));
            bulkTasks.Add(task);
        }

        Console.WriteLine($"Created {bulkTasks.Count} bulk tasks");

        // Process through pipeline
        Console.WriteLine("Processing bulk tasks through pipeline...");
        var processingTasks = bulkTasks.Select(task => pipeline.QueueTaskForProcessingAsync(task));
        await Task.WhenAll(processingTasks);

        Console.WriteLine("Bulk processing completed");

        // Demonstrate pipeline with progress
        Console.WriteLine("\nProcessing with progress reporting...");
        var progress = new Progress<int>(percent =>
        {
            Console.Write($"\r  Progress: {percent}%");
        });

        await ProcessTasksWithProgressAsync(bulkTasks, progress);
        Console.WriteLine();

        Console.WriteLine();
    }

    static async Task DemonstrateSynchronizationAsync(TaskService taskService, RetryPolicy retryPolicy)
    {
        Console.WriteLine("3. Data Synchronization");
        Console.WriteLine("-----------------------");

        Console.WriteLine("Synchronizing data with remote server...");

        await retryPolicy.ExecuteAsync(async () =>
        {
            await taskService.SyncWithRemoteAsync();
            return true;
        });

        Console.WriteLine("Synchronization completed");

        Console.WriteLine();
    }

    static async Task DemonstrateConcurrentOperationsAsync(TaskService taskService)
    {
        Console.WriteLine("4. Concurrent Operations");
        Console.WriteLine("------------------------");

        Console.WriteLine("Testing concurrent task operations...");

        // Simulate multiple users creating tasks concurrently
        var concurrentTasks = new List<Task>();
        for (int i = 0; i < 10; i++)
        {
            int userId = i;
            concurrentTasks.Add(Task.Run(async () =>
            {
                var task = await taskService.CreateTaskAsync(new CreateTaskRequest(
                    $"Concurrent Task {userId}",
                    $"Created by user {userId}",
                    DateTime.UtcNow.AddDays(userId % 7)));

                Console.WriteLine($"  User {userId} created task: {task.Title}");
            }));
        }

        await Task.WhenAll(concurrentTasks);

        var finalCount = await taskService.GetAllTasksAsync();
        Console.WriteLine($"Final task count: {finalCount.Count}");

        Console.WriteLine();
    }

    static async Task ProcessTasksWithProgressAsync(List<TaskItem> tasks, IProgress<int> progress)
    {
        for (int i = 0; i < tasks.Count; i++)
        {
            // Simulate processing
            await Task.Delay(200);
            progress.Report((i + 1) * 100 / tasks.Count);
        }
    }
}