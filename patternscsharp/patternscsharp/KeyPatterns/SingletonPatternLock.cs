namespace Patterns;

// Thread-safe Singleton using a simple lock.
// Intent: Ensure only one instance is created even when accessed by multiple threads concurrently.
public sealed class SingletonPatternLock
{
    private static SingletonPatternLock? _instance;
    private static readonly object _sync = new object();

    // Private constructor prevents instantiation from outside
    private SingletonPatternLock() { }

    public static SingletonPatternLock Instance
    {
        get
        {
            lock (_sync)
            {
                if (_instance == null)
                {
                    _instance = new SingletonPatternLock();
                }
                return _instance;
            }
        }
    }

    public string GetData() => "Thread-safe singleton data";

    public static void Run()
    {
        var a = SingletonPatternLock.Instance;
        var b = SingletonPatternLock.Instance;
        Console.WriteLine($"Singleton (lock): same instance = {ReferenceEquals(a, b)}; data={a.GetData()}");
    }
}
