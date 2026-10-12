using Lab5._2_DI_Middleware.Services;

namespace Lab5._2_DI_Middleware.Services;

public class WeatherService : IWeatherService
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    public async Task<IEnumerable<WeatherForecast>> GetWeatherForecastsAsync(int days = 5)
    {
        // Simulate async operation
        await Task.Delay(100);

        return Enumerable.Range(1, days).Select(index =>
            new WeatherForecast
            (
                DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                Random.Shared.Next(-20, 55),
                Summaries[Random.Shared.Next(Summaries.Length)]
            ))
            .ToArray();
    }

    public async Task<WeatherForecast?> GetWeatherForecastAsync(DateOnly date)
    {
        // Simulate async operation
        await Task.Delay(50);

        // Check if the date is within the next 5 days
        var daysDiff = date.DayNumber - DateOnly.FromDateTime(DateTime.Now).DayNumber;
        if (daysDiff >= 1 && daysDiff <= 5)
        {
            return new WeatherForecast
            (
                date,
                Random.Shared.Next(-20, 55),
                Summaries[Random.Shared.Next(Summaries.Length)]
            );
        }

        return null;
    }
}