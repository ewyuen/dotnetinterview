using TaskManager.Shared.Models;

namespace TaskManager.Shared.Services;

public interface ITaskService
{
    System.Threading.Tasks.Task<IEnumerable<TaskManager.Shared.Models.Task>> GetAllTasksAsync();
    System.Threading.Tasks.Task<TaskManager.Shared.Models.Task?> GetTaskByIdAsync(Guid id);
    System.Threading.Tasks.Task<TaskManager.Shared.Models.Task> CreateTaskAsync(CreateTaskRequest task);
    System.Threading.Tasks.Task<TaskManager.Shared.Models.Task?> UpdateTaskAsync(Guid id, UpdateTaskRequest task);
    System.Threading.Tasks.Task<bool> DeleteTaskAsync(Guid id);
    System.Threading.Tasks.Task<IEnumerable<TaskManager.Shared.Models.Task>> GetTasksByUserAsync(string userId);
    System.Threading.Tasks.Task<IEnumerable<TaskManager.Shared.Models.Task>> GetTasksByCategoryAsync(int categoryId);
}