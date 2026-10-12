using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.Data;
using TaskManagementAPI.Models.Domain;
using TaskManagementAPI.Models.DTOs;
using TaskManagementAPI.Services.Interfaces;
using TaskStatus = TaskManagementAPI.Models.Domain.TaskStatus;

namespace TaskManagementAPI.Services;

/// <summary>
/// Task service implementation with async operations
/// </summary>
public class TaskService : ITaskService
{
    private readonly ApplicationDbContext _context;

    public TaskService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<TaskDto>> GetTasksAsync(string userId, TaskFilter filter)
    {
        var query = _context.Tasks
            .Include(t => t.AssignedTo)
            .Include(t => t.CreatedBy)
            .Include(t => t.Project)
            .Where(t => !t.IsDeleted && (t.CreatedById == userId || t.AssignedToId == userId));

        // Apply status filter
        if (filter.Status.HasValue)
        {
            query = query.Where(t => t.Status == filter.Status.Value);
        }

        // Apply priority filter
        if (filter.Priority.HasValue)
        {
            query = query.Where(t => t.Priority == filter.Priority.Value);
        }

        // Apply assigned to filter
        if (!string.IsNullOrEmpty(filter.AssignedToId))
        {
            query = query.Where(t => t.AssignedToId == filter.AssignedToId);
        }

        // Apply project filter
        if (filter.ProjectId.HasValue)
        {
            query = query.Where(t => t.ProjectId == filter.ProjectId.Value);
        }

        // Apply overdue filter
        if (filter.IsOverdue == true)
        {
            query = query.Where(t => t.DueDate < DateTime.UtcNow && t.Status != TaskStatus.Completed);
        }

        // Apply date range filters
        if (filter.DueDateFrom.HasValue)
        {
            query = query.Where(t => t.DueDate >= filter.DueDateFrom.Value);
        }

        if (filter.DueDateTo.HasValue)
        {
            query = query.Where(t => t.DueDate <= filter.DueDateTo.Value);
        }

        // Apply search filter
        if (!string.IsNullOrEmpty(filter.SearchTerm))
        {
            var searchTerm = filter.SearchTerm.ToLower();
            query = query.Where(t =>
                t.Title.ToLower().Contains(searchTerm) ||
                (t.Description != null && t.Description.ToLower().Contains(searchTerm)));
        }

        var tasks = await query
            .OrderByDescending(t => t.CreatedAt)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        return tasks.Select(MapToDto).ToList();
    }

    public async Task<TaskDto?> GetTaskByIdAsync(Guid taskId, string userId)
    {
        var task = await _context.Tasks
            .Include(t => t.AssignedTo)
            .Include(t => t.CreatedBy)
            .Include(t => t.Project)
            .Include(t => t.Comments)
            .Include(t => t.Attachments)
            .FirstOrDefaultAsync(t => t.Id == taskId && !t.IsDeleted &&
                (t.CreatedById == userId || t.AssignedToId == userId));

        return task != null ? MapToDto(task) : null;
    }

    public async Task<ServiceResult> CreateTaskAsync(CreateTaskRequest request, string createdById)
    {
        try
        {
            // Validate project access if specified
            if (request.ProjectId.HasValue)
            {
                var hasAccess = await _context.ProjectMembers
                    .AnyAsync(pm => pm.ProjectId == request.ProjectId && pm.UserId == createdById);

                if (!hasAccess)
                {
                    return new ServiceResult(false, "You don't have access to this project");
                }
            }

            var task = new TaskItem
            {
                Title = request.Title,
                Description = request.Description,
                Priority = request.Priority,
                Status = TaskStatus.Created,
                DueDate = request.DueDate,
                AssignedToId = request.AssignedToId,
                CreatedById = createdById,
                ProjectId = request.ProjectId
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return new ServiceResult(true, "Task created successfully", task.Id);
        }
        catch (Exception ex)
        {
            return new ServiceResult(false, $"Failed to create task: {ex.Message}");
        }
    }

    public async Task<ServiceResult> UpdateTaskAsync(Guid taskId, UpdateTaskRequest request, string updatedById)
    {
        try
        {
            var task = await _context.Tasks.FindAsync(taskId);
            if (task == null || task.IsDeleted)
            {
                return new ServiceResult(false, "Task not found");
            }

            // Check permissions
            if (!task.CanBeEditedBy(updatedById))
            {
                return new ServiceResult(false, "You don't have permission to edit this task");
            }

            // Update properties
            if (!string.IsNullOrEmpty(request.Title))
                task.Title = request.Title;

            if (request.Description != null)
                task.Description = request.Description;

            if (request.Priority.HasValue)
                task.Priority = request.Priority.Value;

            if (request.Status.HasValue)
                task.Status = request.Status.Value;

            if (request.DueDate.HasValue)
                task.DueDate = request.DueDate;

            if (!string.IsNullOrEmpty(request.AssignedToId))
                task.AssignedToId = request.AssignedToId;

            task.MarkAsUpdated(updatedById);

            await _context.SaveChangesAsync();

            return new ServiceResult(true, "Task updated successfully");
        }
        catch (Exception ex)
        {
            return new ServiceResult(false, $"Failed to update task: {ex.Message}");
        }
    }

    public async Task<ServiceResult> UpdateTaskStatusAsync(Guid taskId, Models.Domain.TaskStatus status, string updatedById)
    {
        try
        {
            var task = await _context.Tasks.FindAsync(taskId);
            if (task == null || task.IsDeleted)
            {
                return new ServiceResult(false, "Task not found");
            }

            // Check permissions
            if (!task.CanBeEditedBy(updatedById))
            {
                return new ServiceResult(false, "You don't have permission to edit this task");
            }

            task.Status = status;
            if (status == TaskStatus.Completed)
            {
                // CompletedAt is automatically set by the Status property setter
            }

            task.MarkAsUpdated(updatedById);
            await _context.SaveChangesAsync();

            return new ServiceResult(true, "Task status updated successfully");
        }
        catch (Exception ex)
        {
            return new ServiceResult(false, $"Failed to update task status: {ex.Message}");
        }
    }

    public async Task<ServiceResult> DeleteTaskAsync(Guid taskId, string deletedById)
    {
        try
        {
            var task = await _context.Tasks.FindAsync(taskId);
            if (task == null || task.IsDeleted)
            {
                return new ServiceResult(false, "Task not found");
            }

            // Check permissions
            if (task.CreatedById != deletedById)
            {
                return new ServiceResult(false, "Only the task creator can delete this task");
            }

            task.IsDeleted = true;
            task.MarkAsUpdated(deletedById);

            await _context.SaveChangesAsync();
            return new ServiceResult(true, "Task deleted successfully");
        }
        catch (Exception ex)
        {
            return new ServiceResult(false, $"Failed to delete task: {ex.Message}");
        }
    }

    public async Task<ServiceResult> AssignTaskAsync(Guid taskId, string assignedToId, string assignedById)
    {
        try
        {
            var task = await _context.Tasks.FindAsync(taskId);
            if (task == null || task.IsDeleted)
            {
                return new ServiceResult(false, "Task not found");
            }

            task.AssignTo(assignedToId, assignedById);
            await _context.SaveChangesAsync();

            return new ServiceResult(true, "Task assigned successfully");
        }
        catch (Exception ex)
        {
            return new ServiceResult(false, $"Failed to assign task: {ex.Message}");
        }
    }

    public async Task<List<TaskDto>> GetAssignedTasksAsync(string userId)
    {
        var tasks = await _context.Tasks
            .Include(t => t.AssignedTo)
            .Include(t => t.CreatedBy)
            .Include(t => t.Project)
            .Where(t => !t.IsDeleted && t.AssignedToId == userId)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();

        return tasks.Select(MapToDto).ToList();
    }

    public async Task<List<TaskDto>> GetOverdueTasksAsync(string userId)
    {
        var tasks = await _context.Tasks
            .Include(t => t.AssignedTo)
            .Include(t => t.CreatedBy)
            .Include(t => t.Project)
            .Where(t => !t.IsDeleted && t.DueDate.HasValue && t.DueDate < DateTime.UtcNow &&
                t.Status != TaskStatus.Completed && (t.AssignedToId == userId || t.CreatedById == userId))
            .OrderBy(t => t.DueDate)
            .ToListAsync();

        return tasks.Select(MapToDto).ToList();
    }

    private static TaskDto MapToDto(TaskItem task) => new(
        task.Id,
        task.Title,
        task.Description,
        task.Priority,
        task.Status,
        task.DueDate,
        task.CompletedAt,
        task.CreatedAt,
        task.UpdatedAt,
        task.AssignedToId,
        task.AssignedTo?.DisplayName,
        task.CreatedById,
        task.CreatedBy?.DisplayName,
        task.ProjectId,
        task.Project?.Name,
        task.IsOverdue,
        task.Comments.Count,
        task.Attachments.Count
    );
}