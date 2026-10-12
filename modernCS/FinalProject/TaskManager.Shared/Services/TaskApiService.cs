using System.Net.Http.Json;
using TaskManager.Shared.Models;
using System.Threading.Tasks;

namespace TaskManager.Shared.Services;

public class TaskApiService : ITaskService
{
    private readonly HttpClient _httpClient;
    private readonly IAuthService _authService;

    public TaskApiService(HttpClient httpClient, IAuthService authService)
    {
        _httpClient = httpClient;
        _authService = authService;
    }

    private async System.Threading.Tasks.Task ConfigureAuthHeaderAsync()
    {
        var token = await _authService.GetTokenAsync();
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }
        else
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }

    public async System.Threading.Tasks.Task<IEnumerable<TaskManager.Shared.Models.Task>> GetAllTasksAsync()
    {
        await ConfigureAuthHeaderAsync();
        return await _httpClient.GetFromJsonAsync<IEnumerable<TaskManager.Shared.Models.Task>>("api/tasks") ?? Enumerable.Empty<TaskManager.Shared.Models.Task>();
    }

    public async System.Threading.Tasks.Task<TaskManager.Shared.Models.Task?> GetTaskByIdAsync(Guid id)
    {
        await ConfigureAuthHeaderAsync();
        return await _httpClient.GetFromJsonAsync<TaskManager.Shared.Models.Task>($"api/tasks/{id}");
    }

    public async System.Threading.Tasks.Task<TaskManager.Shared.Models.Task> CreateTaskAsync(CreateTaskRequest request)
    {
        await ConfigureAuthHeaderAsync();
        var response = await _httpClient.PostAsJsonAsync("api/tasks", request);
        response.EnsureSuccessStatusCode();
        // Assuming the API returns the created task
        return await response.Content.ReadFromJsonAsync<TaskManager.Shared.Models.Task>() ?? throw new Exception("Failed to create task");
    }

    public async System.Threading.Tasks.Task<TaskManager.Shared.Models.Task?> UpdateTaskAsync(Guid id, UpdateTaskRequest request)
    {
        await ConfigureAuthHeaderAsync();
        var response = await _httpClient.PutAsJsonAsync($"api/tasks/{id}", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TaskManager.Shared.Models.Task>();
    }

    public async System.Threading.Tasks.Task<bool> DeleteTaskAsync(Guid id)
    {
        await ConfigureAuthHeaderAsync();
        var response = await _httpClient.DeleteAsync($"api/tasks/{id}");
        return response.IsSuccessStatusCode;
    }

    public async System.Threading.Tasks.Task<IEnumerable<TaskManager.Shared.Models.Task>> GetTasksByUserAsync(string userId)
    {
        await ConfigureAuthHeaderAsync();
        return await _httpClient.GetFromJsonAsync<IEnumerable<TaskManager.Shared.Models.Task>>($"api/tasks?assignedToId={userId}") ?? Enumerable.Empty<TaskManager.Shared.Models.Task>();
    }

    public async System.Threading.Tasks.Task<IEnumerable<TaskManager.Shared.Models.Task>> GetTasksByCategoryAsync(int categoryId)
    {
        await ConfigureAuthHeaderAsync();
        // Assuming category is project
        return await _httpClient.GetFromJsonAsync<IEnumerable<TaskManager.Shared.Models.Task>>($"api/tasks?projectId={categoryId}") ?? Enumerable.Empty<TaskManager.Shared.Models.Task>();
    }
}