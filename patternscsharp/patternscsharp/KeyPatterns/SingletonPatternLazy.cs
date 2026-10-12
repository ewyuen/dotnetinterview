namespace Patterns;

using System;

// Thread-safe lazy-initialized singleton using .NET Lazy<T>.
// This is the recommended, concise pattern for lazy singletons in modern .NET.
public sealed class SingletonPatternLazy
{
    // Lazy<T> ensures that the instance is created only when Value is first accessed.
    // By default LazyThreadSafetyMode.ExecutionAndPublication is used which is safe for multi-threaded scenarios.
    private static readonly Lazy<SingletonPatternLazy> _instance =
        new Lazy<SingletonPatternLazy>(() => new SingletonPatternLazy());

    public static SingletonPatternLazy Instance => _instance.Value;

    // Private constructor prevents external instantiation.
    private SingletonPatternLazy() { }

    public string GetData() => "Lazy singleton instance data";

    public static void Run()
    {
        var a = SingletonPatternLazy.Instance;
        var b = SingletonPatternLazy.Instance;
        Console.WriteLine($"Singleton (Lazy): same instance = {ReferenceEquals(a, b)}; data={a.GetData()}");
    }
}
