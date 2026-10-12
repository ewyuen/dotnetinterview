using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.Models.Domain;

namespace TaskManagementAPI.Data;

/// <summary>
/// Application database context with Identity integration
/// </summary>
public class ApplicationDbContext : IdentityDbContext<User>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // Domain entities
    public DbSet<TaskItem> Tasks { get; set; } = null!;
    public DbSet<Project> Projects { get; set; } = null!;
    public DbSet<TaskComment> TaskComments { get; set; } = null!;
    public DbSet<TaskAttachment> TaskAttachments { get; set; } = null!;
    public DbSet<ProjectMember> ProjectMembers { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Configure entity relationships and constraints
        ConfigureTaskEntity(builder);
        ConfigureProjectEntity(builder);
        ConfigureCommentEntity(builder);
        ConfigureAttachmentEntity(builder);
        ConfigureProjectMemberEntity(builder);

        // Seed initial data
        SeedData(builder);
    }

    private static void ConfigureTaskEntity(ModelBuilder builder)
    {
        builder.Entity<TaskItem>(entity =>
        {
            entity.HasKey(t => t.Id);

            entity.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(t => t.Description)
                .HasMaxLength(2000);

            entity.Property(t => t.Status)
                .HasConversion<int>();

            entity.Property(t => t.Priority)
                .HasConversion<int>();

            // Relationships
            entity.HasOne(t => t.AssignedTo)
                .WithMany(u => u.AssignedTasks)
                .HasForeignKey(t => t.AssignedToId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(t => t.CreatedBy)
                .WithMany(u => u.CreatedTasks)
                .HasForeignKey(t => t.CreatedById)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(t => t.Project)
                .WithMany(p => p.Tasks)
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.SetNull);

            // Indexes for performance
            entity.HasIndex(t => t.AssignedToId);
            entity.HasIndex(t => t.CreatedById);
            entity.HasIndex(t => t.ProjectId);
            entity.HasIndex(t => t.Status);
            entity.HasIndex(t => t.DueDate);
            entity.HasIndex(t => new { t.Status, t.DueDate });
        });
    }

    private static void ConfigureProjectEntity(ModelBuilder builder)
    {
        builder.Entity<Project>(entity =>
        {
            entity.HasKey(p => p.Id);

            entity.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(p => p.Description)
                .HasMaxLength(500);

            // Relationships
            entity.HasOne(p => p.Owner)
                .WithMany(u => u.OwnedProjects)
                .HasForeignKey(p => p.OwnerId)
                .OnDelete(DeleteBehavior.SetNull);

            // Indexes
            entity.HasIndex(p => p.OwnerId);
            entity.HasIndex(p => p.Name);
        });
    }

    private static void ConfigureCommentEntity(ModelBuilder builder)
    {
        builder.Entity<TaskComment>(entity =>
        {
            entity.HasKey(c => c.Id);

            entity.Property(c => c.Content)
                .IsRequired()
                .HasMaxLength(1000);

            // Relationships
            entity.HasOne(c => c.Task)
                .WithMany(t => t.Comments)
                .HasForeignKey(c => c.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(c => c.Author)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.AuthorId)
                .OnDelete(DeleteBehavior.SetNull);

            // Indexes
            entity.HasIndex(c => c.TaskId);
            entity.HasIndex(c => c.AuthorId);
            entity.HasIndex(c => c.CreatedAt);
        });
    }

    private static void ConfigureAttachmentEntity(ModelBuilder builder)
    {
        builder.Entity<TaskAttachment>(entity =>
        {
            entity.HasKey(a => a.Id);

            entity.Property(a => a.FileName)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(a => a.FilePath)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(a => a.ContentType)
                .HasMaxLength(100);

            // Relationships
            entity.HasOne(a => a.Task)
                .WithMany(t => t.Attachments)
                .HasForeignKey(a => a.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            entity.HasIndex(a => a.TaskId);
            entity.HasIndex(a => a.FileName);
        });
    }

    private static void ConfigureProjectMemberEntity(ModelBuilder builder)
    {
        builder.Entity<ProjectMember>(entity =>
        {
            entity.HasKey(pm => pm.Id);

            // Composite unique constraint
            entity.HasIndex(pm => new { pm.ProjectId, pm.UserId })
                .IsUnique();

            // Relationships
            entity.HasOne(pm => pm.Project)
                .WithMany(p => p.Members)
                .HasForeignKey(pm => pm.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(pm => pm.User)
                .WithMany(u => u.ProjectMemberships)
                .HasForeignKey(pm => pm.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            entity.HasIndex(pm => pm.ProjectId);
            entity.HasIndex(pm => pm.UserId);
        });
    }

    private static void SeedData(ModelBuilder builder)
    {
        // Note: Identity roles and users are seeded in DbInitializer
        // This method can be used for additional domain data seeding if needed
    }
}