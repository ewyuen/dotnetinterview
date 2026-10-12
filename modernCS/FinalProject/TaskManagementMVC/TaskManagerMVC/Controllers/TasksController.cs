using Microsoft.AspNetCore.Mvc;
using TaskManager.Shared.Services;
using TaskManager.Shared.Models;
using TaskManager.Shared.Services;

namespace TaskManagerMVC.Controllers;

public class TasksController : Controller
{
    private readonly ITaskService _taskService;
    private readonly IAuthService _authService;

    public TasksController(ITaskService taskService, IAuthService authService)
    {
        _taskService = taskService;
        _authService = authService;
    }

    // GET: Tasks
    public async Task<IActionResult> Index()
    {
        var isAuthenticated = await _authService.IsAuthenticatedAsync();
        if (!isAuthenticated)
        {
            return RedirectToAction("Login", "Account");
        }

        var tasks = await _taskService.GetAllTasksAsync();
        return View(tasks);
    }

    // GET: Tasks/Details/5
    public async Task<IActionResult> Details(Guid id)
    {
        var isAuthenticated = await _authService.IsAuthenticatedAsync();
        if (!isAuthenticated)
        {
            return RedirectToAction("Login", "Account");
        }

        var task = await _taskService.GetTaskByIdAsync(id);
        if (task == null)
        {
            return NotFound();
        }
        return View(task);
    }

    // GET: Tasks/Create
    public async Task<IActionResult> Create()
    {
        var isAuthenticated = await _authService.IsAuthenticatedAsync();
        if (!isAuthenticated)
        {
            return RedirectToAction("Login", "Account");
        }

        return View();
    }

    // POST: Tasks/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateTaskRequest request)
    {
        var isAuthenticated = await _authService.IsAuthenticatedAsync();
        if (!isAuthenticated)
        {
            return RedirectToAction("Login", "Account");
        }

        if (ModelState.IsValid)
        {
            await _taskService.CreateTaskAsync(request);
            return RedirectToAction(nameof(Index));
        }
        return View(request);
    }

    // GET: Tasks/Edit/5
    public async Task<IActionResult> Edit(Guid id)
    {
        var isAuthenticated = await _authService.IsAuthenticatedAsync();
        if (!isAuthenticated)
        {
            return RedirectToAction("Login", "Account");
        }

        var task = await _taskService.GetTaskByIdAsync(id);
        if (task == null)
        {
            return NotFound();
        }
        var updateRequest = new UpdateTaskRequest(
            Title: task.Title,
            Description: task.Description,
            Priority: task.Priority,
            Status: task.Status,
            DueDate: task.DueDate,
            AssignedToId: task.AssignedToId
        );
        return View(updateRequest);
    }

    // POST: Tasks/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, UpdateTaskRequest request)
    {
        var isAuthenticated = await _authService.IsAuthenticatedAsync();
        if (!isAuthenticated)
        {
            return RedirectToAction("Login", "Account");
        }

        if (ModelState.IsValid)
        {
            await _taskService.UpdateTaskAsync(id, request);
            return RedirectToAction(nameof(Index));
        }
        return View(request);
    }

    // GET: Tasks/Delete/5
    public async Task<IActionResult> Delete(Guid id)
    {
        var isAuthenticated = await _authService.IsAuthenticatedAsync();
        if (!isAuthenticated)
        {
            return RedirectToAction("Login", "Account");
        }

        var task = await _taskService.GetTaskByIdAsync(id);
        if (task == null)
        {
            return NotFound();
        }
        return View(task);
    }

    // POST: Tasks/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var isAuthenticated = await _authService.IsAuthenticatedAsync();
        if (!isAuthenticated)
        {
            return RedirectToAction("Login", "Account");
        }

        await _taskService.DeleteTaskAsync(id);
        return RedirectToAction(nameof(Index));
    }
}