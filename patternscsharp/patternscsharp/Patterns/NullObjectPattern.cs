namespace Patterns;

public interface ILogger { void Log(string message); }
public class ConsoleLogger : ILogger { public void Log(string message) => Console.WriteLine(message); }
public class NullLogger : ILogger { public void Log(string message) { /* do nothing */ } }

public static class NullObjectPattern
{
    public static void Run()
    {
        ILogger logger = new NullLogger();
        logger.Log("will not appear");
        logger = new ConsoleLogger();
        logger.Log("appears");
    }
}
