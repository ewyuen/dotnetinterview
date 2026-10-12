using TaskManagementAPI.Models.Domain;
using TaskManagementAPI.Models.DTOs;
using TaskManagementAPI.Models.DTOs;

namespace TaskManagementAPI.Services.Interfaces;

/// <summary>
/// Interface for JWT token service
/// </summary>
public interface IJwtService
{
    string GenerateToken(User user);
    string? ValidateToken(string token);
    string GenerateRefreshToken();
}

/// <summary>
/// Interface for authentication service
/// </summary>
public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task<UserDto?> GetUserByIdAsync(string userId);
    Task<ServiceResult> UpdateProfileAsync(string userId, UpdateProfileRequest request);
    Task<ServiceResult> ChangePasswordAsync(string userId, ChangePasswordRequest request);
}

/// <summary>
/// Interface for task service with async operations
/// </summary>
public interface ITaskService
{
    Task<List<TaskDto>> GetTasksAsync(string userId, TaskFilter filter);
    Task<TaskDto?> GetTaskByIdAsync(Guid taskId, string userId);
    Task<ServiceResult> CreateTaskAsync(CreateTaskRequest request, string createdById);
    Task<ServiceResult> UpdateTaskAsync(Guid taskId, UpdateTaskRequest request, string updatedById);
    Task<ServiceResult> UpdateTaskStatusAsync(Guid taskId, Models.Domain.TaskStatus status, string updatedById);
    Task<ServiceResult> DeleteTaskAsync(Guid taskId, string deletedById);
    Task<ServiceResult> AssignTaskAsync(Guid taskId, string assignedToId, string assignedById);
    Task<List<TaskDto>> GetAssignedTasksAsync(string userId);
    Task<List<TaskDto>> GetOverdueTasksAsync(string userId);
}

/// <summary>
/// Interface for project service
/// </summary>
public interface IProjectService
{
    Task<PaginatedResponse<ProjectDto>> GetProjectsAsync(PaginationParams pagination, string? userId = null);
    Task<ProjectDto?> GetProjectByIdAsync(Guid projectId, string? userId = null);
    Task<ProjectDto> CreateProjectAsync(CreateProjectRequest request, string createdById);
    Task<ProjectDto?> UpdateProjectAsync(Guid projectId, CreateProjectRequest request, string updatedById);
    Task<bool> DeleteProjectAsync(Guid projectId, string deletedById);
    Task<bool> AddProjectMemberAsync(Guid projectId, string userId, bool isAdmin, string addedById);
    Task<bool> RemoveProjectMemberAsync(Guid projectId, string userId, string removedById);
    Task<List<ProjectSummary>> GetUserProjectsAsync(string userId);
}

/// <summary>
/// Interface for comment service
/// </summary>
public interface ICommentService
{
    Task<List<CommentDto>> GetTaskCommentsAsync(Guid taskId);
    Task<CommentDto?> AddCommentAsync(Guid taskId, CreateCommentRequest request, string authorId);
    Task<bool> UpdateCommentAsync(Guid commentId, CreateCommentRequest request, string updatedById);
    Task<bool> DeleteCommentAsync(Guid commentId, string deletedById);
}

/// <summary>
/// Interface for dashboard service
/// </summary>
public interface IDashboardService
{
    Task<DashboardStats> GetDashboardStatsAsync(string userId);
    Task<List<TaskCompletionTrend>> GetTaskCompletionTrendsAsync(string userId, int days);
    Task<ProductivityMetrics> GetProductivityMetricsAsync(string userId);
    Task<List<ActivityDto>> GetRecentActivityAsync(string userId, int limit);
    Task<SystemStats> GetSystemStatsAsync();
}

/// <summary>
/// Interface for background task processing service
/// </summary>
public interface IBackgroundTaskService
{
    Task ProcessOverdueTasksAsync();
    Task SendTaskRemindersAsync();
    Task UpdateProjectStatisticsAsync();
    Task CleanupOldDataAsync();
}

/// <summary>
/// Interface for notification service
/// </summary>
public interface INotificationService
{
    Task NotifyTaskAssignedAsync(Guid taskId);
    Task NotifyTaskCompletedAsync(Guid taskId);
    Task NotifyTaskOverdueAsync(Guid taskId);
    Task NotifyProjectDeadlineAsync(Guid projectId);
}