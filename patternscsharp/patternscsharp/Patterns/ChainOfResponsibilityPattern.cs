// Chain of Responsibility Pattern
// Intent: Pass a request along a chain of handlers. Upon receiving a request, each handler decides either to process the request or to pass it to the next handler.
// Use when multiple objects may handle a request and the handler isn't known a priori.

namespace Patterns;

public abstract class Handler
{
    protected Handler? Next;
    public Handler SetNext(Handler next) { Next = next; return next; }
    public abstract void Handle(string request);
}

public class ConcreteHandlerA : Handler
{
    public override void Handle(string request)
    {
        if (request == "A") Console.WriteLine("Handled by A");
        else Next?.Handle(request);
    }
}

public class ConcreteHandlerB : Handler
{
    public override void Handle(string request)
    {
        if (request == "B") Console.WriteLine("Handled by B");
        else Next?.Handle(request);
    }
}

public static class ChainOfResponsibilityPattern
{
    public static void Run()
    {
        var a = new ConcreteHandlerA();
        var b = new ConcreteHandlerB();
        a.SetNext(b);
        a.Handle("B");
    }
}
