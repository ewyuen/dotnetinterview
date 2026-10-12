// Builder Pattern
// Intent: Separate the construction of a complex object from its representation so the same construction process can create different representations.
// Use when creating complex objects step by step and when you want to isolate construction code from the final representation.

namespace Patterns;

public class BuilderProduct
{
    public string PartA { get; set; } = "";
    public string PartB { get; set; } = "";
    public override string ToString() => $"Product with {PartA} and {PartB}";
}

public interface IBuilder
{
    void BuildPartA();
    void BuildPartB();
    BuilderProduct GetResult();
}

public class ConcreteBuilder : IBuilder
{
    private BuilderProduct _product = new BuilderProduct();
    public void BuildPartA() => _product.PartA = "PartA";
    public void BuildPartB() => _product.PartB = "PartB";
    public BuilderProduct GetResult() { var result = _product; _product = new BuilderProduct(); return result; }
}

public class Director
{
    public void Construct(IBuilder builder)
    {
        builder.BuildPartA();
        builder.BuildPartB();
    }
}

public static class BuilderPattern
{
    public static void Run()
    {
        var director = new Director();
        var builder = new ConcreteBuilder();
        director.Construct(builder);
        var product = builder.GetResult();
        Console.WriteLine(product);
    }
}
