// Flyweight Pattern
// Intent: Use sharing to support large numbers of fine-grained objects efficiently by separating intrinsic (shared) from extrinsic (context-dependent) state.
// Use when many objects share common state that can be factored out to save memory.

namespace Patterns;

public class Flyweight
{
    public string Intrinsic { get; }
    public Flyweight(string intrinsic) { Intrinsic = intrinsic; }
    public string Operation(string extrinsic) => $"[{Intrinsic}] with {extrinsic}";
}

public class FlyweightFactory
{
    private readonly Dictionary<string, Flyweight> _pool = new();
    public Flyweight Get(string key)
    {
        if (!_pool.ContainsKey(key)) _pool[key] = new Flyweight(key);
        return _pool[key];
    }
}

public static class FlyweightPattern
{
    public static void Run()
    {
        var factory = new FlyweightFactory();
        var f1 = factory.Get("X");
        var f2 = factory.Get("X");
        Console.WriteLine(f1.Operation("first"));
        Console.WriteLine($"same instance: {ReferenceEquals(f1,f2)}");
    }
}
