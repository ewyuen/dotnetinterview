namespace Lab5._2_DI_Middleware.Services;

public interface ILoggingService
{
    void Log(string message);
    IEnumerable<string> GetLogs();
}

public class LoggingService : ILoggingService
{
    private readonly List<string> _logs = new();
    private readonly object _lock = new();

    public void Log(string message)
    {
        var logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}: {message}";
        lock (_lock)
        {
            _logs.Add(logEntry);
            Console.WriteLine(logEntry);
        }
    }

    public IEnumerable<string> GetLogs()
    {
        lock (_lock)
        {
            return _logs.ToList();
        }
    }
}