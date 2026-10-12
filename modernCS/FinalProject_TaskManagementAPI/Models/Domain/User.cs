using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TaskManagementAPI.Models.Domain;

/// <summary>
/// Extended user entity demonstrating modern C# features and OOP principles
/// </summary>
public class User : IdentityUser
{
    [Required]
    [StringLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string LastName { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Bio { get; set; }

    public DateTime? DateOfBirth { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastLoginAt { get; set; }
    public bool IsActive { get; set; } = true;

    // Refresh token properties for JWT
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }

    // Navigation Properties
    public virtual ICollection<TaskItem> CreatedTasks { get; set; } = new List<TaskItem>();
    public virtual ICollection<TaskItem> AssignedTasks { get; set; } = new List<TaskItem>();
    public virtual ICollection<TaskComment> Comments { get; set; } = new List<TaskComment>();
    public virtual ICollection<Project> OwnedProjects { get; set; } = new List<Project>();
    public virtual ICollection<ProjectMember> ProjectMemberships { get; set; } = new List<ProjectMember>();

    // Computed Properties using modern C# features
    public string FullName => $"{FirstName} {LastName}".Trim();
    public string DisplayName => string.IsNullOrWhiteSpace(FullName) ? UserName ?? "Unknown User" : FullName;
    public int Age => DateOfBirth.HasValue ? DateTime.UtcNow.Year - DateOfBirth.Value.Year : 0;

    // Business Logic Methods
    public bool CanAccessProject(Guid projectId) =>
        OwnedProjects.Any(p => p.Id == projectId) ||
        ProjectMemberships.Any(pm => pm.ProjectId == projectId);

    public bool CanManageProject(Guid projectId) =>
        OwnedProjects.Any(p => p.Id == projectId) ||
        ProjectMemberships.Any(pm => pm.ProjectId == projectId && pm.IsAdmin);

    public void UpdateLastLogin()
    {
        LastLoginAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void Reactivate()
    {
        IsActive = true;
    }

    // Pattern matching with modern C# - checking user status
    public string GetStatusDescription() => this switch
    {
        { IsActive: false } => "Inactive",
        { LastLoginAt: null } => "Never logged in",
        { LastLoginAt: var lastLogin } when lastLogin < DateTime.UtcNow.AddDays(-30) => "Inactive (30+ days)",
        { LastLoginAt: var lastLogin } when lastLogin < DateTime.UtcNow.AddDays(-7) => "Inactive (7+ days)",
        _ => "Active"
    };

    // Method demonstrating null safety and modern features
    public bool IsValidForTaskAssignment() =>
        IsActive &&
        !string.IsNullOrWhiteSpace(Email) &&
        EmailConfirmed;
}

/// <summary>
/// Modern C# record for user statistics - immutable data structure
/// </summary>
public record UserStatistics(
    string UserId,
    string UserName,
    string FullName,
    int TotalTasksCreated,
    int TotalTasksAssigned,
    int CompletedTasks,
    int OverdueTasks,
    int OwnedProjects,
    DateTime? LastActivity
)
{
    // Computed properties in records
    public double CompletionRate => TotalTasksAssigned > 0 ? (double)CompletedTasks / TotalTasksAssigned * 100 : 0;
    public bool IsActive => LastActivity.HasValue && LastActivity.Value > DateTime.UtcNow.AddDays(-30);
}

/// <summary>
/// Modern C# record for task summary
/// </summary>
public record TaskSummary(
    Guid TaskId,
    string Title,
    TaskStatus Status,
    TaskPriority Priority,
    string? AssignedToName,
    DateTime? DueDate,
    bool IsOverdue
)
{
    public string StatusDescription => Status switch
    {
        TaskStatus.Created => "Not Started",
        TaskStatus.InProgress => "In Progress",
        TaskStatus.OnHold => "On Hold",
        TaskStatus.Completed => "Completed",
        TaskStatus.Cancelled => "Cancelled",
        _ => "Unknown"
    };

    public string PriorityColor => Priority switch
    {
        TaskPriority.Low => "green",
        TaskPriority.Medium => "yellow",
        TaskPriority.High => "orange",
        TaskPriority.Critical => "red",
        _ => "gray"
    };
}

/// <summary>
/// Modern C# record for project summary
/// </summary>
public record ProjectSummary(
    Guid ProjectId,
    string Name,
    string? Description,
    string OwnerName,
    int TotalTasks,
    int CompletedTasks,
    double CompletionPercentage,
    DateTime? EndDate,
    bool IsActive
);