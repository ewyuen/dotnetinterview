namespace Lab5._2_DI_Middleware.Services;

public interface IWeatherService
{
    Task<IEnumerable<WeatherForecast>> GetWeatherForecastsAsync(int days = 5);
    Task<WeatherForecast?> GetWeatherForecastAsync(DateOnly date);
}

public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}