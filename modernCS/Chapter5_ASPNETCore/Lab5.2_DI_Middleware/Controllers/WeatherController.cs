using Lab5._2_DI_Middleware.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lab5._2_DI_Middleware.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeatherController : ControllerBase
{
    private readonly IWeatherService _weatherService;
    private readonly ILoggingService _loggingService;

    public WeatherController(IWeatherService weatherService, ILoggingService loggingService)
    {
        _weatherService = weatherService;
        _loggingService = loggingService;
    }

    [HttpGet("forecast")]
    public async Task<IActionResult> GetWeatherForecast([FromQuery] int days = 5)
    {
        _loggingService.Log($"Getting weather forecast for {days} days");

        var forecasts = await _weatherService.GetWeatherForecastsAsync(days);
        return Ok(forecasts);
    }

    [HttpGet("forecast/{date}")]
    public async Task<IActionResult> GetWeatherForecastByDate(DateOnly date)
    {
        _loggingService.Log($"Getting weather forecast for date: {date}");

        var forecast = await _weatherService.GetWeatherForecastAsync(date);

        if (forecast == null)
        {
            return NotFound($"No forecast available for date: {date}");
        }

        return Ok(forecast);
    }

    [HttpGet("logs")]
    public IActionResult GetLogs()
    {
        var logs = _loggingService.GetLogs();
        return Ok(logs);
    }
}