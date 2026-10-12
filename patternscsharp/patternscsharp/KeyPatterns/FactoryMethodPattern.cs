// Factory Method Pattern
// Intent: Define an interface for creating an object, but let subclasses decide which class to instantiate.
// Use when a class can't anticipate the class of objects it must create or when you want to localize knowledge of which helper subclass is the delegate.

namespace Patterns;

public abstract class Product
{
    public abstract string GetName();
}

public class ConcreteProductA : Product
{
    public override string GetName() => "Product A";
}

public class ConcreteProductB : Product
{
    public override string GetName() => "Product B";
}

public abstract class Creator
{
    public abstract Product FactoryMethod();

    public void AnOperation()
    {
        var product = FactoryMethod();
        Console.WriteLine($"FactoryMethod created: {product.GetName()}");
    }
}

public class ConcreteCreatorA : Creator
{
    public override Product FactoryMethod() => new ConcreteProductA();
}

public class ConcreteCreatorB : Creator
{
    public override Product FactoryMethod() => new ConcreteProductB();
}

public static class FactoryMethodPattern
{
    public static void Run()
    {
        Creator c1 = new ConcreteCreatorA();
        Creator c2 = new ConcreteCreatorB();
        c1.AnOperation();
        c2.AnOperation();
    }
}
