// Bridge Pattern
// Intent: Decouple an abstraction from its implementation so that the two can vary independently.
// Use when you want to avoid a permanent binding between an abstraction and its implementation and when both should be extensible.

namespace Patterns;

public interface IImplementor
{
    string OperationImpl();
}

public class ConcreteImplementorA : IImplementor { public string OperationImpl() => "Implementation A"; }
public class ConcreteImplementorB : IImplementor { public string OperationImpl() => "Implementation B"; }

public abstract class Abstraction
{
    protected IImplementor _implementor;
    protected Abstraction(IImplementor implementor) { _implementor = implementor; }
    public abstract string Operation();
}

public class RefinedAbstraction : Abstraction
{
    public RefinedAbstraction(IImplementor implementor) : base(implementor) { }
    public override string Operation() => _implementor.OperationImpl();

    public static void Run()
    {
        var a = new RefinedAbstraction(new ConcreteImplementorA());
        var b = new RefinedAbstraction(new ConcreteImplementorB());
        Console.WriteLine(a.Operation());
        Console.WriteLine(b.Operation());
    }
}
