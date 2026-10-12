// Composite Pattern
// Intent: Compose objects into tree structures to represent part-whole hierarchies. Composite lets clients treat individual objects and compositions uniformly.
// Use to work with tree structures of objects and treat leaves and composites the same way.

namespace Patterns;

public abstract class Component
{
    public string Name { get; }
    protected Component(string name) { Name = name; }
    public abstract void Add(Component c);
    public abstract void Remove(Component c);
    public abstract void Display(int depth);
}

public class Leaf : Component
{
    public Leaf(string name) : base(name) { }
    public override void Add(Component c) => throw new InvalidOperationException();
    public override void Remove(Component c) => throw new InvalidOperationException();
    public override void Display(int depth) => Console.WriteLine(new string('-', depth) + Name);
}

public class Composite : Component
{
    private readonly List<Component> _children = new();
    public Composite(string name) : base(name) { }
    public override void Add(Component c) => _children.Add(c);
    public override void Remove(Component c) => _children.Remove(c);
    public override void Display(int depth)
    {
        Console.WriteLine(new string('-', depth) + Name);
        foreach (var child in _children) child.Display(depth + 2);
    }

    public static void Run()
    {
        var root = new Composite("root");
        var leaf1 = new Leaf("Leaf A");
        var leaf2 = new Leaf("Leaf B");
        var comp = new Composite("Composite X");
        comp.Add(leaf1);
        root.Add(comp);
        root.Add(leaf2);
        root.Display(1);
    }
}
