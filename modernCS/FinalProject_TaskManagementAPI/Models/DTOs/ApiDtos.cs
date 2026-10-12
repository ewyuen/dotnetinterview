using System.ComponentModel.DataAnnotations;
using TaskManagementAPI.Models.Domain;

namespace TaskManagementAPI.Models.DTOs;

/// <summary>
/// Service result wrapper for operations
/// </summary>
public record ServiceResult(
    bool Success,
    string? Message,
    Guid? TaskId = null
);

/// <summary>
/// Modern C# record for user registration - immutable DTO
/// </summary>
public record RegisterRequest(
    [Required, EmailAddress, StringLength(256)]
    string Email,

    [Required, StringLength(100, MinimumLength = 6)]
    string Password,

    [Required, StringLength(50)]
    string FirstName,

    [Required, StringLength(50)]
    string LastName,

    [StringLength(500)]
    string? Bio
);

/// <summary>
/// Modern C# record for login request
/// </summary>
public record LoginRequest(
    [Required, EmailAddress]
    string Email,

    [Required]
    string Password
);

/// <summary>
/// Modern C# record for authentication response
/// </summary>
public record AuthResponse(
    bool Success,
    string Message,
    string? Token,
    string? RefreshToken,
    UserDto? User,
    string? UserId
);

/// <summary>
/// User DTO with modern C# features
/// </summary>
public record UserDto(
    string Id,
    string UserName,
    string Email,
    string FirstName,
    string LastName,
    string? Bio,
    DateTime? DateOfBirth,
    DateTime CreatedAt,
    DateTime? LastLoginAt,
    bool IsActive,
    IList<string> Roles
)
{
    public string FullName => $"{FirstName} {LastName}".Trim();
    public string DisplayName => string.IsNullOrWhiteSpace(FullName) ? UserName : FullName;
    public int? Age => DateOfBirth.HasValue ? DateTime.UtcNow.Year - DateOfBirth.Value.Year : null;
    public string Role => Roles?.FirstOrDefault() ?? "User";
}

/// <summary>
/// Task creation DTO using modern C# record
/// </summary>
public record CreateTaskRequest(
    [Required, StringLength(200)]
    string Title,

    [StringLength(2000)]
    string? Description,

    TaskPriority Priority = TaskPriority.Medium,
    DateTime? DueDate = null,
    string? AssignedToId = null,
    Guid? ProjectId = null
);

/// <summary>
/// Task update DTO
/// </summary>
public record UpdateTaskRequest(
    [StringLength(200)]
    string? Title,

    [StringLength(2000)]
    string? Description,

    TaskPriority? Priority,
    Models.Domain.TaskStatus? Status,
    DateTime? DueDate,
    string? AssignedToId
);

/// <summary>
/// Task status update request
/// </summary>
public record UpdateTaskStatusRequest(
    [Required]
    Models.Domain.TaskStatus Status
);

/// <summary>
/// Task assignment request
/// </summary>
public record AssignTaskRequest(
    [Required]
    string AssignedToUserId
);

/// <summary>
/// Task filter for querying tasks
/// </summary>
public record TaskFilter(
    Models.Domain.TaskStatus? Status = null,
    TaskPriority? Priority = null,
    string? AssignedToId = null,
    Guid? ProjectId = null,
    bool? IsOverdue = null,
    DateTime? DueDateFrom = null,
    DateTime? DueDateTo = null,
    string? SearchTerm = null,
    int Page = 1,
    int PageSize = 10
);

/// <summary>
/// Profile update request
/// </summary>
public record UpdateProfileRequest(
    [StringLength(50)]
    string? FirstName,

    [StringLength(50)]
    string? LastName,

    [StringLength(500)]
    string? Bio,

    DateTime? DateOfBirth
);

/// <summary>
/// Change password request
/// </summary>
public record ChangePasswordRequest(
    [Required]
    string CurrentPassword,

    [Required, StringLength(100, MinimumLength = 6)]
    string NewPassword,

    [Required]
    string ConfirmNewPassword
);

/// <summary>
/// Task DTO with computed properties
/// </summary>
public record TaskDto(
    Guid Id,
    string Title,
    string? Description,
    TaskPriority Priority,
    Models.Domain.TaskStatus Status,
    DateTime? DueDate,
    DateTime? CompletedAt,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    string? AssignedToId,
    string? AssignedToName,
    string? CreatedById,
    string? CreatedByName,
    Guid? ProjectId,
    string? ProjectName,
    bool IsOverdue,
    int CommentCount,
    int AttachmentCount
)
{
    public string StatusDescription => Status switch
    {
        Models.Domain.TaskStatus.Created => "Not Started",
        Models.Domain.TaskStatus.InProgress => "In Progress",
        Models.Domain.TaskStatus.OnHold => "On Hold",
        Models.Domain.TaskStatus.Completed => "Completed",
        Models.Domain.TaskStatus.Cancelled => "Cancelled",
        _ => "Unknown"
    };

    public string PriorityColor => Priority switch
    {
        TaskPriority.Low => "success",
        TaskPriority.Medium => "warning",
        TaskPriority.High => "danger",
        TaskPriority.Critical => "dark",
        _ => "secondary"
    };
}

/// <summary>
/// Project creation DTO
/// </summary>
public record CreateProjectRequest(
    [Required, StringLength(100)]
    string Name,

    [StringLength(500)]
    string? Description,

    DateTime? StartDate,
    DateTime? EndDate
);

/// <summary>
/// Project DTO
/// </summary>
public record ProjectDto(
    Guid Id,
    string Name,
    string? Description,
    DateTime? StartDate,
    DateTime? EndDate,
    string OwnerId,
    string OwnerName,
    DateTime CreatedAt,
    int TotalTasks,
    int CompletedTasks,
    double CompletionPercentage,
    bool IsActive,
    List<TaskSummary> RecentTasks
);

/// <summary>
/// Comment creation DTO
/// </summary>
public record CreateCommentRequest(
    [Required, StringLength(1000)]
    string Content
);

/// <summary>
/// Comment DTO
/// </summary>
public record CommentDto(
    Guid Id,
    string Content,
    DateTime CreatedAt,
    string AuthorId,
    string AuthorName,
    Guid TaskId
);

/// <summary>
/// Dashboard statistics DTO
/// </summary>
public record DashboardStats(
    int TotalTasks,
    int CompletedTasks,
    int OverdueTasks,
    int InProgressTasks,
    int TotalProjects,
    int ActiveProjects,
    List<TaskSummary> RecentTasks,
    List<ProjectSummary> RecentProjects
);

/// <summary>
/// Pagination parameters
/// </summary>
public record PaginationParams(
    int Page = 1,
    int PageSize = 10,
    string? SortBy = null,
    bool SortDescending = false,
    string? Filter = null
);

/// <summary>
/// Paginated response wrapper
/// </summary>
public record PaginatedResponse<T>(
    List<T> Items,
    int TotalCount,
    int Page,
    int PageSize,
    int TotalPages,
    bool HasNextPage,
    bool HasPreviousPage
)
{
    public static PaginatedResponse<T> Create(List<T> items, int totalCount, int page, int pageSize)
    {
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        return new PaginatedResponse<T>(
            items,
            totalCount,
            page,
            pageSize,
            totalPages,
            page < totalPages,
            page > 1
        );
    }
}

/// <summary>
/// Task completion trend DTO
/// </summary>
public record TaskCompletionTrend(
    DateTime Date,
    int CompletedTasks,
    int CreatedTasks
);

/// <summary>
/// Productivity metrics DTO
/// </summary>
public record ProductivityMetrics(
    double AverageTasksPerDay,
    double CompletionRate,
    int CurrentStreak,
    int LongestStreak,
    TimeSpan AverageCompletionTime
);

/// <summary>
/// Activity DTO
/// </summary>
public record ActivityDto(
    string Id,
    string Type,
    string Description,
    DateTime Timestamp,
    string? RelatedEntityId,
    string? RelatedEntityName
);

/// <summary>
/// System statistics DTO
/// </summary>
public record SystemStats(
    int TotalUsers,
    int TotalTasks,
    int TotalProjects,
    int ActiveUsers,
    int CompletedTasksToday,
    int OverdueTasks,
    DateTime LastUpdated
);