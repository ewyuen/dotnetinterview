using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.Data;
using TaskManagementAPI.Models.Domain;
using TaskManagementAPI.Models.DTOs;
using TaskManagementAPI.Services.Interfaces;
using TaskStatus = TaskManagementAPI.Models.Domain.TaskStatus;

namespace TaskManagementAPI.Services;

/// <summary>
/// Dashboard service with concurrent operations and data aggregation
/// </summary>
public class DashboardService : IDashboardService
{
    private readonly ApplicationDbContext _context;

    public DashboardService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardStats> GetDashboardStatsAsync(string userId)
    {
        // Get task stats concurrently
        var taskStatsTask = GetUserTaskStatsAsync(userId);
        var projectStatsTask = GetUserProjectStatsAsync(userId);
        var recentTasksTask = GetRecentUserTasksAsync(userId);
        var recentProjectsTask = GetRecentUserProjectsAsync(userId);

        await Task.WhenAll(taskStatsTask, projectStatsTask, recentTasksTask, recentProjectsTask);

        var (totalTasks, completedTasks, overdueTasks, inProgressTasks) = await taskStatsTask;
        var (totalProjects, activeProjects) = await projectStatsTask;
        var recentTasks = await recentTasksTask;
        var recentProjects = await recentProjectsTask;

        return new DashboardStats(
            totalTasks,
            completedTasks,
            overdueTasks,
            inProgressTasks,
            totalProjects,
            activeProjects,
            recentTasks,
            recentProjects
        );
    }

    private async Task<(int TotalTasks, int CompletedTasks, int OverdueTasks, int InProgressTasks)> GetUserTaskStatsAsync(string userId)
    {
        var userTasks = await _context.Tasks
            .Where(t => !t.IsDeleted && (t.AssignedToId == userId || t.CreatedById == userId))
            .ToListAsync();

        var totalTasks = userTasks.Count;
        var completedTasks = userTasks.Count(t => t.Status == TaskStatus.Completed);
        var overdueTasks = userTasks.Count(t => t.DueDate.HasValue && t.DueDate < DateTime.UtcNow && t.Status != TaskStatus.Completed);
        var inProgressTasks = userTasks.Count(t => t.Status == TaskStatus.InProgress);

        return (totalTasks, completedTasks, overdueTasks, inProgressTasks);
    }

    private async Task<(int TotalProjects, int ActiveProjects)> GetUserProjectStatsAsync(string userId)
    {
        var userProjects = await _context.Projects
            .Where(p => !p.IsDeleted && (p.OwnerId == userId || p.Members.Any(m => m.UserId == userId)))
            .ToListAsync();

        var totalProjects = userProjects.Count;
        var activeProjects = userProjects.Count(p => p.IsActive);

        return (totalProjects, activeProjects);
    }

    private async Task<List<TaskSummary>> GetRecentUserTasksAsync(string userId)
    {
        var tasks = await _context.Tasks
            .Include(t => t.AssignedTo)
            .Where(t => !t.IsDeleted && (t.AssignedToId == userId || t.CreatedById == userId))
            .OrderByDescending(t => t.UpdatedAt ?? t.CreatedAt)
            .Take(5)
            .ToListAsync();

        return tasks.Select(t => new TaskSummary(
            t.Id,
            t.Title,
            t.Status,
            t.Priority,
            t.AssignedTo != null ? $"{t.AssignedTo.FirstName} {t.AssignedTo.LastName}".Trim() : "Unassigned",
            t.DueDate,
            t.IsOverdue
        )).ToList();
    }

    private async Task<List<ProjectSummary>> GetRecentUserProjectsAsync(string userId)
    {
        var projects = await _context.Projects
            .Include(p => p.Owner)
            .Include(p => p.Tasks)
            .Where(p => !p.IsDeleted && (p.OwnerId == userId || p.Members.Any(m => m.UserId == userId)))
            .OrderByDescending(p => p.UpdatedAt ?? p.CreatedAt)
            .Take(3)
            .ToListAsync();

        return projects.Select(p => new ProjectSummary(
            p.Id,
            p.Name,
            p.Description,
            p.Owner != null ? $"{p.Owner.FirstName} {p.Owner.LastName}".Trim() : "Unknown",
            p.TotalTasks,
            p.CompletedTasks,
            p.CompletionPercentage,
            p.EndDate,
            p.IsActive
        )).ToList();
    }

    private async Task<(int TotalTasks, int CompletedTasks, int OverdueTasks, int InProgressTasks)> GetSystemTaskStatsAsync()
    {
        var stats = await _context.Tasks
            .Where(t => !t.IsDeleted)
            .GroupBy(_ => 1)
            .Select(g => new
            {
                TotalTasks = g.Count(),
                CompletedTasks = g.Count(t => t.Status == TaskStatus.Completed),
                OverdueTasks = g.Count(t => t.DueDate.HasValue && t.DueDate < DateTime.UtcNow && t.Status != TaskStatus.Completed),
                InProgressTasks = g.Count(t => t.Status == TaskStatus.InProgress)
            })
            .FirstOrDefaultAsync();

        return stats != null
            ? (stats.TotalTasks, stats.CompletedTasks, stats.OverdueTasks, stats.InProgressTasks)
            : (0, 0, 0, 0);
    }

    private async Task<(int TotalProjects, int ActiveProjects)> GetSystemProjectStatsAsync()
    {
        var stats = await _context.Projects
            .Where(p => !p.IsDeleted)
            .GroupBy(_ => 1)
            .Select(g => new
            {
                TotalProjects = g.Count(),
                ActiveProjects = g.Count(p => p.IsActive)
            })
            .FirstOrDefaultAsync();

        return stats != null
            ? (stats.TotalProjects, stats.ActiveProjects)
            : (0, 0);
    }

    private async Task<List<TaskSummary>> GetRecentSystemTasksAsync()
    {
        var tasks = await _context.Tasks
            .Include(t => t.AssignedTo)
            .Include(t => t.Project)
            .Where(t => !t.IsDeleted)
            .OrderByDescending(t => t.UpdatedAt ?? t.CreatedAt)
            .Take(10)
            .ToListAsync();

        return tasks.Select(t => new TaskSummary(
            t.Id,
            t.Title,
            t.Status,
            t.Priority,
            t.AssignedTo?.DisplayName ?? "Unassigned",
            t.DueDate,
            t.IsOverdue
        )).ToList();
    }

    private async Task<List<ProjectSummary>> GetRecentSystemProjectsAsync()
    {
        var projects = await _context.Projects
            .Include(p => p.Owner)
            .Where(p => !p.IsDeleted)
            .OrderByDescending(p => p.UpdatedAt ?? p.CreatedAt)
            .Take(5)
            .ToListAsync();

        return projects.Select(p => new ProjectSummary(
            p.Id,
            p.Name,
            p.Description,
            p.Owner?.DisplayName ?? "Unknown",
            p.TotalTasks,
            p.CompletedTasks,
            p.CompletionPercentage,
            p.EndDate,
            p.IsActive
        )).ToList();
    }

    public async Task<List<TaskCompletionTrend>> GetTaskCompletionTrendsAsync(string userId, int days)
    {
        var startDate = DateTime.UtcNow.AddDays(-days);
        var endDate = DateTime.UtcNow;

        var trends = new List<TaskCompletionTrend>();

        for (var date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
        {
            var completedTasks = await _context.Tasks
                .Where(t => !t.IsDeleted &&
                    (t.CreatedById == userId || t.AssignedToId == userId) &&
                    t.Status == TaskStatus.Completed &&
                    t.CompletedAt.HasValue &&
                    t.CompletedAt.Value.Date == date)
                .CountAsync();

            var createdTasks = await _context.Tasks
                .Where(t => !t.IsDeleted &&
                    t.CreatedById == userId &&
                    t.CreatedAt.Date == date)
                .CountAsync();

            trends.Add(new TaskCompletionTrend(date, completedTasks, createdTasks));
        }

        return trends;
    }

    public async Task<ProductivityMetrics> GetProductivityMetricsAsync(string userId)
    {
        var userTasks = await _context.Tasks
            .Where(t => !t.IsDeleted && (t.CreatedById == userId || t.AssignedToId == userId))
            .ToListAsync();

        var completedTasks = userTasks.Where(t => t.Status == TaskStatus.Completed).ToList();
        var averageTasksPerDay = userTasks.Count / Math.Max(1, (DateTime.UtcNow - userTasks.Min(t => t.CreatedAt)).TotalDays);

        var completionRate = userTasks.Count > 0 ? (double)completedTasks.Count / userTasks.Count : 0;

        var currentStreak = CalculateCurrentStreak(completedTasks);
        var longestStreak = CalculateLongestStreak(completedTasks);

        var averageCompletionTime = completedTasks.Any() && completedTasks.Any(t => t.CompletedAt.HasValue && t.CreatedAt != default)
            ? completedTasks.Where(t => t.CompletedAt.HasValue).Average(t => (t.CompletedAt!.Value - t.CreatedAt).TotalHours)
            : 0;

        return new ProductivityMetrics(
            averageTasksPerDay,
            completionRate,
            currentStreak,
            longestStreak,
            TimeSpan.FromHours(averageCompletionTime)
        );
    }

    public async Task<List<ActivityDto>> GetRecentActivityAsync(string userId, int limit)
    {
        var activities = new List<ActivityDto>();

        // Get recent task creations
        var recentTasks = await _context.Tasks
            .Where(t => !t.IsDeleted && t.CreatedById == userId)
            .OrderByDescending(t => t.CreatedAt)
            .Take(limit / 2)
            .ToListAsync();

        activities.AddRange(recentTasks.Select(t => new ActivityDto(
            $"task-created-{t.Id}",
            "Task Created",
            $"Created task: {t.Title}",
            t.CreatedAt,
            t.Id.ToString(),
            t.Title
        )));

        // Get recent task completions
        var recentCompletions = await _context.Tasks
            .Where(t => !t.IsDeleted && t.Status == TaskStatus.Completed &&
                (t.CreatedById == userId || t.AssignedToId == userId) && t.CompletedAt.HasValue)
            .OrderByDescending(t => t.CompletedAt)
            .Take(limit / 2)
            .ToListAsync();

        activities.AddRange(recentCompletions.Select(t => new ActivityDto(
            $"task-completed-{t.Id}",
            "Task Completed",
            $"Completed task: {t.Title}",
            t.CompletedAt!.Value,
            t.Id.ToString(),
            t.Title
        )));

        return activities.OrderByDescending(a => a.Timestamp).Take(limit).ToList();
    }

    public async Task<SystemStats> GetSystemStatsAsync()
    {
        var totalUsers = await _context.Users.CountAsync(u => u.IsActive);
        var totalTasks = await _context.Tasks.CountAsync(t => !t.IsDeleted);
        var totalProjects = await _context.Projects.CountAsync(p => !p.IsDeleted);
        var activeUsers = await _context.Users.CountAsync(u => u.IsActive && u.LastLoginAt > DateTime.UtcNow.AddDays(-7));
        var completedTasksToday = await _context.Tasks.CountAsync(t => !t.IsDeleted && t.Status == TaskStatus.Completed &&
            t.CompletedAt.HasValue && t.CompletedAt.Value.Date == DateTime.UtcNow.Date);
        var overdueTasks = await _context.Tasks.CountAsync(t => !t.IsDeleted && t.DueDate < DateTime.UtcNow && t.Status != TaskStatus.Completed);

        return new SystemStats(
            totalUsers,
            totalTasks,
            totalProjects,
            activeUsers,
            completedTasksToday,
            overdueTasks,
            DateTime.UtcNow
        );
    }

    private int CalculateCurrentStreak(List<TaskItem> completedTasks)
    {
        if (!completedTasks.Any()) return 0;

        var sortedTasks = completedTasks.Where(t => t.CompletedAt.HasValue)
            .OrderByDescending(t => t.CompletedAt!.Value.Date)
            .ToList();

        var currentDate = DateTime.UtcNow.Date;
        var streak = 0;

        foreach (var task in sortedTasks)
        {
            if (task.CompletedAt!.Value.Date == currentDate)
            {
                streak++;
                currentDate = currentDate.AddDays(-1);
            }
            else if (task.CompletedAt.Value.Date < currentDate)
            {
                break;
            }
        }

        return streak;
    }

    private int CalculateLongestStreak(List<TaskItem> completedTasks)
    {
        if (!completedTasks.Any()) return 0;

        var sortedTasks = completedTasks.Where(t => t.CompletedAt.HasValue)
            .OrderBy(t => t.CompletedAt!.Value.Date)
            .ToList();

        var longestStreak = 0;
        var currentStreak = 1;
        var previousDate = sortedTasks[0].CompletedAt!.Value.Date;

        for (int i = 1; i < sortedTasks.Count; i++)
        {
            var currentDate = sortedTasks[i].CompletedAt!.Value.Date;
            if (currentDate == previousDate.AddDays(1) || currentDate == previousDate)
            {
                if (currentDate != previousDate) currentStreak++;
            }
            else
            {
                longestStreak = Math.Max(longestStreak, currentStreak);
                currentStreak = 1;
            }
            previousDate = currentDate;
        }

        return Math.Max(longestStreak, currentStreak);
    }
}