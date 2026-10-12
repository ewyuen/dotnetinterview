using Lab5._2_DI_Middleware.Services;

namespace Lab5._2_DI_Middleware.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILoggingService _loggingService;

    public RequestLoggingMiddleware(RequestDelegate next, ILoggingService loggingService)
    {
        _next = next;
        _loggingService = loggingService;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Log the incoming request
        _loggingService.Log($"Request: {context.Request.Method} {context.Request.Path}");

        // Call the next middleware in the pipeline
        await _next(context);

        // Log the response (only if response hasn't started)
        if (!context.Response.HasStarted)
        {
            _loggingService.Log($"Response: {context.Response.StatusCode}");
        }
    }
}

// Extension method to add the middleware to the pipeline
public static class RequestLoggingMiddlewareExtensions
{
    public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequestLoggingMiddleware>();
    }
}