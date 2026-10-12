using TaskManagementAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TaskManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    /// <summary>
    /// Get dashboard statistics for the current user
    /// </summary>
    [HttpGet("stats")]
    public async Task<IActionResult> GetDashboardStats()
    {
        try
        {
            var userId = User.FindFirst("userId")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "Invalid token" });
            }

            var stats = await _dashboardService.GetDashboardStatsAsync(userId);
            return Ok(stats);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving dashboard statistics", error = ex.Message });
        }
    }

    /// <summary>
    /// Get task completion trends for the last 30 days
    /// </summary>
    [HttpGet("trends")]
    public async Task<IActionResult> GetTaskTrends()
    {
        try
        {
            var userId = User.FindFirst("userId")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "Invalid token" });
            }

            var trends = await _dashboardService.GetTaskCompletionTrendsAsync(userId, 30);
            return Ok(trends);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving task trends", error = ex.Message });
        }
    }

    /// <summary>
    /// Get productivity metrics for the current user
    /// </summary>
    [HttpGet("productivity")]
    public async Task<IActionResult> GetProductivityMetrics()
    {
        try
        {
            var userId = User.FindFirst("userId")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "Invalid token" });
            }

            var metrics = await _dashboardService.GetProductivityMetricsAsync(userId);
            return Ok(metrics);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving productivity metrics", error = ex.Message });
        }
    }

    /// <summary>
    /// Get recent activity for the current user
    /// </summary>
    [HttpGet("activity")]
    public async Task<IActionResult> GetRecentActivity([FromQuery] int limit = 10)
    {
        try
        {
            var userId = User.FindFirst("userId")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "Invalid token" });
            }

            var activities = await _dashboardService.GetRecentActivityAsync(userId, limit);
            return Ok(activities);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving recent activity", error = ex.Message });
        }
    }

    /// <summary>
    /// Get system-wide statistics (Admin only)
    /// </summary>
    [HttpGet("system-stats")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> GetSystemStats()
    {
        try
        {
            var stats = await _dashboardService.GetSystemStatsAsync();
            return Ok(stats);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving system statistics", error = ex.Message });
        }
    }
}