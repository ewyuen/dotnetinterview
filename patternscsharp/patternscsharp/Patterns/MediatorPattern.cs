// Mediator Pattern
// Intent: Encapsulate how a set of objects interact. The Mediator centralizes communication
// so Colleague objects do not refer to each other directly.
//
// Key responsibilities:
// - Mediator: knows the colleagues and routes messages between them.
// - ColleagueA / ColleagueB: hold a reference to the Mediator and use it to send messages.
//
// Usage example: create a Mediator, create ColleagueA and ColleagueB passing the mediator,
// then call Send on a colleague; the mediator will forward the message to the other colleague.

namespace Patterns;

/// <summary>
/// Central mediator that coordinates communication between colleagues.
/// It exposes a Notify method used by colleagues to send messages.
/// The mediator holds references to colleagues it coordinates (A and B).
/// </summary>
public class Mediator
{
    // Public properties expose references to the colleagues this mediator coordinates.
    // In a more robust implementation the mediator might store a collection of colleagues
    // and perform more advanced routing logic.
    public ColleagueA A { get; set; }
    public ColleagueB B { get; set; }

    // Notify receives the sender identifier and a message, then dispatches the message
    // to the appropriate colleague. This method centralizes the routing logic.
    public void Notify(string sender, string message)
    {
        if (sender == "A") B.Receive(message);
        if (sender == "B") A.Receive(message);
    }
}

/// <summary>
/// A colleague that communicates only through the mediator.
/// It sends messages by calling <see cref="Mediator.Notify"/> and receives messages
/// via the Receive method.
/// </summary>
public class ColleagueA
{
    private Mediator _mediator;

    // When constructed, the colleague registers itself with the mediator so the
    // mediator can route messages to it. The registration here is simple assignment.
    public ColleagueA(Mediator m) { _mediator = m; _mediator.A = this; }

    // Send a message by delegating to the mediator. The string "A" is used
    // to identify the sender to the mediator's routing logic.
    public void Send(string message) => _mediator.Notify("A", message);

    // Receive is invoked by the mediator when a message for this colleague arrives.
    public void Receive(string message) => Console.WriteLine($"A received: {message}");
}

/// <summary>
/// Another colleague. Mirrors ColleagueA's behavior but uses the identifier "B" when sending.
/// </summary>
public class ColleagueB
{
    private Mediator _mediator;

    public ColleagueB(Mediator m) { _mediator = m; _mediator.B = this; }
    public void Send(string message) => _mediator.Notify("B", message);
    public void Receive(string message) => Console.WriteLine($"B received: {message}");

    // Run demonstrates creating a mediator and two colleagues, then sending a message
    // from A. The mediator will forward the message to B, which will print it.
    public static void Run()
    {
        var m = new Mediator();
        var a = new ColleagueA(m);
        var b = new ColleagueB(m);
        a.Send("hello");
    }
}
