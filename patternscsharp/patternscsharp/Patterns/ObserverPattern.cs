using System.Collections.Generic;

namespace Patterns;

public interface IObserver { void Update(string message); }
public interface ISubject { void Attach(IObserver o); void Detach(IObserver o); void Notify(); }

public class ConcreteSubject : ISubject
{
    private readonly List<IObserver> _observers = new();
    public string State { get; set; } = "";
    public void Attach(IObserver o) => _observers.Add(o);
    public void Detach(IObserver o) => _observers.Remove(o);
    public void Notify() => _observers.ForEach(o => o.Update(State));
}

public class ConcreteObserver : IObserver
{
    private readonly string _name;
    public ConcreteObserver(string name) { _name = name; }
    public void Update(string message) => Console.WriteLine($"{_name} received: {message}");

    public static void Run()
    {
        var sub = new ConcreteSubject();
        var o1 = new ConcreteObserver("o1");
        sub.Attach(o1);
        sub.State = "new";
        sub.Notify();
    }
}
