// Adapter Pattern
// Intent: Convert the interface of a class (Adaptee) into another interface (ITarget) clients expect.
// Use when you need to make existing classes work with a new interface without modifying them.

namespace Patterns;

public class Adaptee
{
    public string SpecificRequest() => "Adaptee behavior";
}

public interface ITarget
{
    string Request();
}

public class Adapter : ITarget
{
    private readonly Adaptee _adaptee = new Adaptee();
    public string Request() => _adaptee.SpecificRequest();

    public static void Run()
    {
        ITarget target = new Adapter();
        Console.WriteLine($"Adapter: {target.Request()}");
    }
}
