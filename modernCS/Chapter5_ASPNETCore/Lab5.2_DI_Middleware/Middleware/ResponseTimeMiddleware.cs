using Lab5._2_DI_Middleware.Services;

namespace Lab5._2_DI_Middleware.Middleware;

public class ResponseTimeMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILoggingService _loggingService;

    public ResponseTimeMiddleware(RequestDelegate next, ILoggingService loggingService)
    {
        _next = next;
        _loggingService = loggingService;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var startTime = DateTime.Now;

        // Call the next middleware in the pipeline
        await _next(context);

        var endTime = DateTime.Now;
        var responseTime = endTime - startTime;

        // Add response time header (only if response hasn't started)
        if (!context.Response.HasStarted)
        {
            context.Response.Headers["X-Response-Time"] = $"{responseTime.TotalMilliseconds}ms";
        }

        // Log the response time
        _loggingService.Log($"Response time for {context.Request.Path}: {responseTime.TotalMilliseconds}ms");
    }
}

// Extension method to add the middleware to the pipeline
public static class ResponseTimeMiddlewareExtensions
{
    public static IApplicationBuilder UseResponseTime(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ResponseTimeMiddleware>();
    }
}