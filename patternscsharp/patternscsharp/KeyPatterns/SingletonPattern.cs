namespace Patterns;

public sealed class SingletonPattern
{
    private static readonly SingletonPattern _instance = new SingletonPattern();
    public static SingletonPattern Instance => _instance;

    // Private constructor
    private SingletonPattern() { }

    public string GetData() => "Singleton instance data";

    public static void Run()
    {
        var a = SingletonPattern.Instance;
        var b = SingletonPattern.Instance;
        Console.WriteLine($"Singleton: same instance = {ReferenceEquals(a, b)}; data={a.GetData()}");
    }
}
