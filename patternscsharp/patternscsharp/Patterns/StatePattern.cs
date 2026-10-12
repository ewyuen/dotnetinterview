namespace Patterns;

public interface IState { void Handle(Context c); }

public class Context
{
    private IState _state;
    public Context(IState state) { _state = state; }
    public void SetState(IState s) => _state = s;
    public void Request() => _state.Handle(this);
}

public class ConcreteStateA : IState
{
    public void Handle(Context c) { Console.WriteLine("State A handles and switches to B"); c.SetState(new ConcreteStateB()); }
}

public class ConcreteStateB : IState
{
    public void Handle(Context c) { Console.WriteLine("State B handles"); }

    public static void Run()
    {
        var ctx = new Context(new ConcreteStateA());
        ctx.Request();
        ctx.Request();
    }
}
