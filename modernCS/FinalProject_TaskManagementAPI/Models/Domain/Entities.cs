using System.ComponentModel.DataAnnotations;

namespace TaskManagementAPI.Models.Domain;

/// <summary>
/// Base entity class demonstrating inheritance and common properties
/// </summary>
public abstract class BaseEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public bool IsDeleted { get; set; } = false;

    public void MarkAsUpdated(string? updatedBy = null)
    {
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;
    }
}

/// <summary>
/// Enumeration for task priority using modern enum features
/// </summary>
public enum TaskPriority
{
    Low = 1,
    Medium = 2,
    High = 3,
    Critical = 4
}

/// <summary>
/// Enumeration for task status
/// </summary>
public enum TaskStatus
{
    Created = 1,
    InProgress = 2,
    OnHold = 3,
    Completed = 4,
    Cancelled = 5
}

/// <summary>
/// Task entity demonstrating encapsulation and business logic
/// </summary>
public class TaskItem : BaseEntity
{
    private TaskStatus _status;
    private DateTime? _completedAt;

    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [StringLength(2000)]
    public string? Description { get; set; }

    public TaskPriority Priority { get; set; } = TaskPriority.Medium;

    public TaskStatus Status
    {
        get => _status;
        set
        {
            if (_status != value)
            {
                _status = value;
                if (_status == TaskStatus.Completed && !_completedAt.HasValue)
                {
                    _completedAt = DateTime.UtcNow;
                }
                else if (_status != TaskStatus.Completed)
                {
                    _completedAt = null;
                }
            }
        }
    }

    public DateTime? DueDate { get; set; }
    public DateTime? CompletedAt => _completedAt;

    // Foreign Keys
    public string? AssignedToId { get; set; }
    public string? CreatedById { get; set; }
    public Guid? ProjectId { get; set; }

    // Navigation Properties
    public virtual User? AssignedTo { get; set; }
    public virtual User? CreatedBy { get; set; }
    public virtual Project? Project { get; set; }
    public virtual ICollection<TaskComment> Comments { get; set; } = new List<TaskComment>();
    public virtual ICollection<TaskAttachment> Attachments { get; set; } = new List<TaskAttachment>();

    // Business Logic Methods
    public bool IsOverdue => DueDate.HasValue && DueDate.Value < DateTime.UtcNow && Status != TaskStatus.Completed;
    public bool CanBeEditedBy(string userId) => CreatedById == userId || AssignedToId == userId;
    public bool CanBeAssignedBy(string userId) => CreatedById == userId;

    public void AssignTo(string userId, string assignedBy)
    {
        if (!CanBeAssignedBy(assignedBy))
            throw new InvalidOperationException("Only the task creator can assign tasks.");

        AssignedToId = userId;
        MarkAsUpdated(assignedBy);
    }

    public void Complete(string completedBy)
    {
        if (AssignedToId != completedBy && CreatedById != completedBy)
            throw new InvalidOperationException("Only assigned user or creator can complete the task.");

        Status = TaskStatus.Completed;
        MarkAsUpdated(completedBy);
    }
}

/// <summary>
/// Project entity for organizing tasks
/// </summary>
public class Project : BaseEntity
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    // Foreign Key
    public string? OwnerId { get; set; }

    // Navigation Properties
    public virtual User? Owner { get; set; }
    public virtual ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    public virtual ICollection<ProjectMember> Members { get; set; } = new List<ProjectMember>();

    // Computed Properties
    public int TotalTasks => Tasks.Count;
    public int CompletedTasks => Tasks.Count(t => t.Status == TaskStatus.Completed);
    public double CompletionPercentage => TotalTasks > 0 ? (double)CompletedTasks / TotalTasks * 100 : 0;

    public bool IsActive => !IsDeleted && (!EndDate.HasValue || EndDate.Value > DateTime.UtcNow);
}

/// <summary>
/// Task comment entity
/// </summary>
public class TaskComment : BaseEntity
{
    [Required]
    [StringLength(1000)]
    public string Content { get; set; } = string.Empty;

    // Foreign Keys
    public Guid TaskId { get; set; }
    public string? AuthorId { get; set; }

    // Navigation Properties
    public virtual TaskItem? Task { get; set; }
    public virtual User? Author { get; set; }
}

/// <summary>
/// Task attachment entity
/// </summary>
public class TaskAttachment : BaseEntity
{
    [Required]
    [StringLength(255)]
    public string FileName { get; set; } = string.Empty;

    [Required]
    [StringLength(500)]
    public string FilePath { get; set; } = string.Empty;

    [StringLength(100)]
    public string? ContentType { get; set; }

    public long FileSize { get; set; }

    // Foreign Key
    public Guid TaskId { get; set; }

    // Navigation Property
    public virtual TaskItem? Task { get; set; }
}

/// <summary>
/// Project member relationship
/// </summary>
public class ProjectMember : BaseEntity
{
    public Guid ProjectId { get; set; }
    public string? UserId { get; set; }
    public bool IsAdmin { get; set; } = false;

    // Navigation Properties
    public virtual Project? Project { get; set; }
    public virtual User? User { get; set; }
}