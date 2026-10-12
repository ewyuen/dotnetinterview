namespace Patterns;

public class Memento
{
    public string State { get; }
    public Memento(string state) { State = state; }
}

public class Originator
{
    public string State { get; set; } = "";
    public Memento Save() => new Memento(State);
    public void Restore(Memento m) => State = m.State;

    public static void Run()
    {
        var o = new Originator();
        o.State = "State1";
        var m = o.Save();
        o.State = "State2";
        o.Restore(m);
        Console.WriteLine(o.State);
    }
}
