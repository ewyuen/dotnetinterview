using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.Data;
using TaskManagementAPI.Models.Domain;
using TaskManagementAPI.Services.Interfaces;
using TaskStatus = TaskManagementAPI.Models.Domain.TaskStatus;

namespace TaskManagementAPI.BackgroundServices;

/// <summary>
/// Background service for processing tasks concurrently
/// Demonstrates multithreading and concurrent operations
/// </summary>
public class TaskProcessingService : BackgroundService, IBackgroundTaskService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<TaskProcessingService> _logger;
    private readonly ConcurrentDictionary<string, Task> _runningTasks;
    private readonly SemaphoreSlim _semaphore;

    public TaskProcessingService(
        IServiceProvider serviceProvider,
        ILogger<TaskProcessingService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _runningTasks = new ConcurrentDictionary<string, Task>();
        _semaphore = new SemaphoreSlim(3); // Limit concurrent operations
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Task Processing Service started");

        // Wait a bit before starting timers to avoid conflicts with app startup
        await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);

        // Schedule recurring tasks with longer initial delays
        var overdueTaskTimer = new Timer(
            async _ => await ProcessOverdueTasksAsync(),
            null,
            TimeSpan.FromMinutes(5), // Start after 5 minutes instead of immediately
            TimeSpan.FromHours(1)); // Check every hour

        var reminderTimer = new Timer(
            async _ => await SendTaskRemindersAsync(),
            null,
            TimeSpan.FromMinutes(35), // Start after 35 minutes
            TimeSpan.FromHours(6)); // Check every 6 hours

        var statisticsTimer = new Timer(
            async _ => await UpdateProjectStatisticsAsync(),
            null,
            TimeSpan.FromMinutes(20), // Start after 20 minutes
            TimeSpan.FromHours(2)); // Update every 2 hours

        var cleanupTimer = new Timer(
            async _ => await CleanupOldDataAsync(),
            null,
            TimeSpan.FromHours(1), // Start after 1 hour
            TimeSpan.FromDays(7)); // Clean up weekly

        stoppingToken.Register(() =>
        {
            overdueTaskTimer.Dispose();
            reminderTimer.Dispose();
            statisticsTimer.Dispose();
            cleanupTimer.Dispose();
            _logger.LogInformation("Task Processing Service stopping");
        });

        // Keep the service running
        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    public async Task ProcessOverdueTasksAsync()
    {
        var taskId = $"ProcessOverdueTasks_{Guid.NewGuid()}";

        if (!_runningTasks.TryAdd(taskId, Task.CompletedTask))
        {
            _logger.LogWarning("Overdue task processing already running");
            return;
        }

        try
        {
            await _semaphore.WaitAsync();

            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var overdueTasks = await context.Tasks
                .Where(t => !t.IsDeleted &&
                           t.DueDate.HasValue &&
                           t.DueDate < DateTime.UtcNow &&
                           t.Status != TaskStatus.Completed &&
                           t.Status != TaskStatus.Cancelled)
                .ToListAsync();

            if (overdueTasks.Any())
            {
                _logger.LogInformation($"Found {overdueTasks.Count} overdue tasks");

                // Process tasks concurrently but with limited parallelism
                var processingTasks = overdueTasks
                    .Select(async task =>
                    {
                        try
                        {
                            // Simulate processing time
                            await Task.Delay(100);

                            // Update task status if needed
                            if (task.Status == TaskStatus.Created)
                            {
                                task.Status = TaskStatus.OnHold;
                                task.MarkAsUpdated("System");
                            }

                            _logger.LogWarning($"Task '{task.Title}' is overdue (Due: {task.DueDate})");
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, $"Error processing overdue task {task.Id}");
                        }
                    })
                    .ToArray();

                await Task.WhenAll(processingTasks);

                await context.SaveChangesAsync();
                _logger.LogInformation($"Processed {overdueTasks.Count} overdue tasks");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in ProcessOverdueTasksAsync");
        }
        finally
        {
            _semaphore.Release();
            _runningTasks.TryRemove(taskId, out _);
        }
    }

    public async Task SendTaskRemindersAsync()
    {
        var taskId = $"SendTaskReminders_{Guid.NewGuid()}";

        if (!_runningTasks.TryAdd(taskId, Task.CompletedTask))
        {
            _logger.LogWarning("Task reminder sending already running");
            return;
        }

        try
        {
            await _semaphore.WaitAsync();

            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Find tasks due in the next 24 hours
            var tomorrow = DateTime.UtcNow.AddDays(1);
            var reminderTasks = await context.Tasks
                .Include(t => t.AssignedTo)
                .Where(t => !t.IsDeleted &&
                           t.DueDate.HasValue &&
                           t.DueDate <= tomorrow &&
                           t.DueDate > DateTime.UtcNow &&
                           t.Status != TaskStatus.Completed &&
                           t.Status != TaskStatus.Cancelled &&
                           t.AssignedTo != null)
                .ToListAsync();

            if (reminderTasks.Any())
            {
                _logger.LogInformation($"Sending reminders for {reminderTasks.Count} tasks");

                // Process reminders concurrently
                var reminderTasksList = reminderTasks
                    .Select(async task =>
                    {
                        try
                        {
                            // Simulate sending notification
                            await Task.Delay(50);

                            _logger.LogInformation($"Reminder sent to {task.AssignedTo!.Email} for task '{task.Title}' (Due: {task.DueDate})");

                            // In a real application, you would:
                            // - Send email notifications
                            // - Send push notifications
                            // - Create in-app notifications
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, $"Error sending reminder for task {task.Id}");
                        }
                    })
                    .ToArray();

                await Task.WhenAll(reminderTasksList);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in SendTaskRemindersAsync");
        }
        finally
        {
            _semaphore.Release();
            _runningTasks.TryRemove(taskId, out _);
        }
    }

    public async Task UpdateProjectStatisticsAsync()
    {
        var taskId = $"UpdateProjectStatistics_{Guid.NewGuid()}";

        if (!_runningTasks.TryAdd(taskId, Task.CompletedTask))
        {
            _logger.LogWarning("Project statistics update already running");
            return;
        }

        try
        {
            await _semaphore.WaitAsync();

            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var projects = await context.Projects
                .Include(p => p.Tasks)
                .Where(p => !p.IsDeleted)
                .ToListAsync();

            foreach (var project in projects)
            {
                // Update computed properties
                var tasks = project.Tasks.Where(t => !t.IsDeleted).ToList();
                var totalTasks = tasks.Count;
                var completedTasks = tasks.Count(t => t.Status == TaskStatus.Completed);

                // Only update if values have changed
                if (project.TotalTasks != totalTasks || project.CompletedTasks != completedTasks)
                {
                    project.TotalTasks.ShouldEqual(totalTasks);
                    project.CompletedTasks.ShouldEqual(completedTasks);
                    project.MarkAsUpdated("System");
                }
            }

            await context.SaveChangesAsync();
            _logger.LogInformation($"Updated statistics for {projects.Count} projects");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in UpdateProjectStatisticsAsync");
        }
        finally
        {
            _semaphore.Release();
            _runningTasks.TryRemove(taskId, out _);
        }
    }

    public async Task CleanupOldDataAsync()
    {
        var taskId = $"CleanupOldData_{Guid.NewGuid()}";

        if (!_runningTasks.TryAdd(taskId, Task.CompletedTask))
        {
            _logger.LogWarning("Data cleanup already running");
            return;
        }

        try
        {
            await _semaphore.WaitAsync();

            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var cutoffDate = DateTime.UtcNow.AddMonths(-6); // Keep data for 6 months

            // Soft delete old completed tasks
            var oldCompletedTasks = await context.Tasks
                .Where(t => !t.IsDeleted &&
                           t.Status == TaskStatus.Completed &&
                           t.CompletedAt < cutoffDate)
                .ToListAsync();

            foreach (var task in oldCompletedTasks)
            {
                task.IsDeleted = true;
                task.MarkAsUpdated("System");
            }

            // Remove old comments (hard delete after 1 year)
            var veryOldComments = await context.TaskComments
                .Where(c => c.CreatedAt < DateTime.UtcNow.AddYears(-1))
                .ToListAsync();

            context.TaskComments.RemoveRange(veryOldComments);

            await context.SaveChangesAsync();

            _logger.LogInformation($"Cleaned up {oldCompletedTasks.Count} old tasks and {veryOldComments.Count} old comments");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in CleanupOldDataAsync");
        }
        finally
        {
            _semaphore.Release();
            _runningTasks.TryRemove(taskId, out _);
        }
    }
}

// Extension method for demonstration
public static class TaskExtensions
{
    public static void ShouldEqual(this int actual, int expected)
    {
        if (actual != expected)
        {
            throw new InvalidOperationException($"Expected {expected}, but got {actual}");
        }
    }
}