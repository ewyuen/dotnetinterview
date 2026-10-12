namespace Patterns;

public class Aggregate
{
    private readonly List<string> _items = new();
    public void Add(string s) => _items.Add(s);
    public IIterator GetIterator() => new ConcreteIterator(_items);
}

public interface IIterator { string? Next(); bool HasNext(); }

public class ConcreteIterator : IIterator
{
    private readonly List<string> _items;
    private int _position = 0;
    public ConcreteIterator(List<string> items) { _items = items; }
    public string? Next() => _position < _items.Count ? _items[_position++] : null;
    public bool HasNext() => _position < _items.Count;

    public static void Run()
    {
        var ag = new Aggregate();
        ag.Add("one");
        ag.Add("two");
        var it = ag.GetIterator();
        while (it.HasNext()) Console.WriteLine(it.Next());
    }
}
