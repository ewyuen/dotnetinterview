using Microsoft.AspNetCore.Identity;
using TaskManagementAPI.Models.Domain;
using TaskStatus = TaskManagementAPI.Models.Domain.TaskStatus;

namespace TaskManagementAPI.Data;

/// <summary>
/// Database initializer for seeding initial data
/// </summary>
public static class DbInitializer
{
    public static async Task SeedRolesAndAdminUser(
        RoleManager<IdentityRole> roleManager,
        UserManager<User> userManager)
    {
        // Create roles if they don't exist
        string[] roleNames = { "Admin", "Manager", "User" };

        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        // Create admin user if it doesn't exist
        var adminEmail = "admin@taskmanagement.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            adminUser = new User
            {
                UserName = adminEmail,
                Email = adminEmail,
                FirstName = "System",
                LastName = "Administrator",
                Bio = "System administrator with full access",
                EmailConfirmed = true,
                IsActive = true
            };

            var result = await userManager.CreateAsync(adminUser, "Admin123!");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }

        // Create demo manager user
        var managerEmail = "manager@taskmanagement.com";
        var managerUser = await userManager.FindByEmailAsync(managerEmail);

        if (managerUser == null)
        {
            managerUser = new User
            {
                UserName = managerEmail,
                Email = managerEmail,
                FirstName = "Project",
                LastName = "Manager",
                Bio = "Project manager with team oversight capabilities",
                EmailConfirmed = true,
                IsActive = true
            };

            var result = await userManager.CreateAsync(managerUser, "Manager123!");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(managerUser, "Manager");
            }
        }

        // Create demo regular user
        var userEmail = "user@taskmanagement.com";
        var regularUser = await userManager.FindByEmailAsync(userEmail);

        if (regularUser == null)
        {
            regularUser = new User
            {
                UserName = userEmail,
                Email = userEmail,
                FirstName = "Demo",
                LastName = "User",
                Bio = "Regular user for task management",
                EmailConfirmed = true,
                IsActive = true
            };

            var result = await userManager.CreateAsync(regularUser, "User123!");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(regularUser, "User");
            }
        }
    }

    /// <summary>
    /// Seeds demo data for development and testing
    /// </summary>
    public static async Task SeedDemoData(ApplicationDbContext context, UserManager<User> userManager)
    {
        // Only seed demo data if no projects exist
        if (context.Projects.Any()) return;

        var adminUser = await userManager.FindByEmailAsync("admin@taskmanagement.com");
        var managerUser = await userManager.FindByEmailAsync("manager@taskmanagement.com");
        var regularUser = await userManager.FindByEmailAsync("user@taskmanagement.com");

        if (adminUser == null || managerUser == null || regularUser == null) return;

        // Create demo projects
        var project1 = new Project
        {
            Id = Guid.NewGuid(),
            Name = "Website Redesign",
            Description = "Complete redesign of the company website with modern UI/UX",
            StartDate = DateTime.UtcNow.AddDays(-30),
            EndDate = DateTime.UtcNow.AddDays(30),
            OwnerId = managerUser.Id,
            CreatedBy = managerUser.Id
        };

        var project2 = new Project
        {
            Id = Guid.NewGuid(),
            Name = "Mobile App Development",
            Description = "Develop a cross-platform mobile application",
            StartDate = DateTime.UtcNow.AddDays(-15),
            EndDate = DateTime.UtcNow.AddDays(45),
            OwnerId = adminUser.Id,
            CreatedBy = adminUser.Id
        };

        context.Projects.AddRange(project1, project2);
        await context.SaveChangesAsync();

        // Create demo tasks
        var tasks = new List<TaskItem>
        {
            new TaskItem
            {
                Title = "Design new homepage layout",
                Description = "Create wireframes and mockups for the new homepage",
                Priority = TaskPriority.High,
                Status = TaskStatus.InProgress,
                DueDate = DateTime.UtcNow.AddDays(7),
                AssignedToId = regularUser.Id,
                CreatedById = managerUser.Id,
                ProjectId = project1.Id
            },
            new TaskItem
            {
                Title = "Implement responsive design",
                Description = "Ensure the website works on all device sizes",
                Priority = TaskPriority.Medium,
                Status = TaskStatus.Created,
                DueDate = DateTime.UtcNow.AddDays(14),
                AssignedToId = regularUser.Id,
                CreatedById = managerUser.Id,
                ProjectId = project1.Id
            },
            new TaskItem
            {
                Title = "Set up CI/CD pipeline",
                Description = "Configure automated testing and deployment",
                Priority = TaskPriority.Critical,
                Status = TaskStatus.Completed,
                DueDate = DateTime.UtcNow.AddDays(-5),
                AssignedToId = adminUser.Id,
                CreatedById = adminUser.Id,
                ProjectId = project2.Id
            },
            new TaskItem
            {
                Title = "API development",
                Description = "Build RESTful APIs for the mobile app",
                Priority = TaskPriority.High,
                Status = TaskStatus.InProgress,
                DueDate = DateTime.UtcNow.AddDays(10),
                AssignedToId = adminUser.Id,
                CreatedById = adminUser.Id,
                ProjectId = project2.Id
            }
        };

        context.Tasks.AddRange(tasks);
        await context.SaveChangesAsync();

        // Add project members
        var projectMembers = new List<ProjectMember>
        {
            new ProjectMember
            {
                ProjectId = project1.Id,
                UserId = regularUser.Id,
                IsAdmin = false
            },
            new ProjectMember
            {
                ProjectId = project2.Id,
                UserId = adminUser.Id,
                IsAdmin = true
            },
            new ProjectMember
            {
                ProjectId = project2.Id,
                UserId = managerUser.Id,
                IsAdmin = false
            }
        };

        context.ProjectMembers.AddRange(projectMembers);
        await context.SaveChangesAsync();

        // Add some comments
        var comments = new List<TaskComment>
        {
            new TaskComment
            {
                Content = "Great progress on the homepage design! The new layout looks much cleaner.",
                TaskId = tasks[0].Id,
                AuthorId = managerUser.Id
            },
            new TaskComment
            {
                Content = "I've completed the CI/CD setup. All tests are now running automatically.",
                TaskId = tasks[2].Id,
                AuthorId = adminUser.Id
            }
        };

        context.TaskComments.AddRange(comments);
        await context.SaveChangesAsync();
    }
}