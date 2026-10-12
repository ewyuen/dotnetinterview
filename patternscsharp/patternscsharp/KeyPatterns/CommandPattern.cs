// Command Pattern
// Intent: Encapsulate a request as an object, thereby letting you parametrize clients with different requests, queue or log requests, and support undoable operations.
// Use when you want to decouple sender of a request from its receiver.

namespace Patterns;

public interface ICommand { void Execute(); }

public class Receiver
{
    public void Action() => Console.WriteLine("Receiver: Action executed");
}

public class ConcreteCommand : ICommand
{
    private readonly Receiver _receiver;
    public ConcreteCommand(Receiver r) { _receiver = r; }
    public void Execute() => _receiver.Action();
}

public class Invoker
{
    public ICommand? Command { get; set; }
    public void Run() => Command?.Execute();

    public static void RunDemo()
    {
        var invoker = new Invoker();
        var receiver = new Receiver();
        invoker.Command = new ConcreteCommand(receiver);
        invoker.Run();
    }
}
