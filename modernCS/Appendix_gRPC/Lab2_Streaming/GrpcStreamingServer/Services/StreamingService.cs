using Grpc.Core;
using GrpcStreaming;

namespace GrpcStreamingServer.Services;

public class StreamingService : Weather.WeatherBase
{
    private readonly ILogger<StreamingService> _logger;
    public StreamingService(ILogger<StreamingService> logger)
    {
        _logger = logger;
    }

    public override async Task GetWeatherStream(
    WeatherRequest request,
    IServerStreamWriter<WeatherUpdate> responseStream,
    ServerCallContext context)
    {
        for (int i = 0; i < 10; i++)
        {
            // Check if client cancelled
            if (context.CancellationToken.IsCancellationRequested)
                break;

            await responseStream.WriteAsync(new WeatherUpdate
            {
                Temperature = Random.Shared.Next(60, 90),
                Timestamp = DateTime.UtcNow.ToString(),
                Summary = string.IsNullOrEmpty(request.Location) ? "Weather update" : $"Weather update for {request.Location}"
            });
        }

        await Task.Delay(1000); // Send update every second
    }
}

