// Decorator Pattern
// Intent: Attach additional responsibilities to an object dynamically. Decorators provide a flexible alternative to subclassing for extending functionality.
// Use when you need to add responsibilities to individual objects dynamically and transparently.
namespace Patterns;

public abstract class ComponentBase
{
    public abstract string Operation();
}

public class ConcreteComponent : ComponentBase
{
    public override string Operation() => "ConcreteComponent";
}

public abstract class DecoratorBase : ComponentBase
{
    protected ComponentBase _component;
    protected DecoratorBase(ComponentBase component) { _component = component; }
}

public class ConcreteDecoratorA : DecoratorBase
{
    public ConcreteDecoratorA(ComponentBase component) : base(component) { }
    public override string Operation() => $"DecoratorA({_component.Operation()})";
}

public static class DecoratorPattern
{
    public static void Run()
    {
        ComponentBase component = new ConcreteComponent();
        var decorated = new ConcreteDecoratorA(component);
        Console.WriteLine(decorated.Operation());
    }
}
