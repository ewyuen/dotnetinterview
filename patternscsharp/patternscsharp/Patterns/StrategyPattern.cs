namespace Patterns;

public interface IStrategy { string DoAlgorithm(string data); }

public class ConcreteStrategyA : IStrategy { public string DoAlgorithm(string data) => data.ToUpper(); }
public class ConcreteStrategyB : IStrategy { public string DoAlgorithm(string data) => data.ToLower(); }

public class ContextStrategy
{
    private IStrategy _strategy;
    public ContextStrategy(IStrategy s) { _strategy = s; }
    public void SetStrategy(IStrategy s) => _strategy = s;
    public void Do(string data) => Console.WriteLine(_strategy.DoAlgorithm(data));

    public static void Run()
    {
        var ctx = new ContextStrategy(new ConcreteStrategyA());
        ctx.Do("Hello");
        ctx.SetStrategy(new ConcreteStrategyB());
        ctx.Do("Hello");
    }
}
