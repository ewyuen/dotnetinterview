// Facade Pattern
// Intent: Provide a unified interface to a set of interfaces in a subsystem. Facade defines a higher-level interface that makes the subsystem easier to use.
// Use to simplify complex subsystems and provide a single entry point.

namespace Patterns;

public class SubsystemA { public string A() => "A"; }
public class SubsystemB { public string B() => "B"; }
public class SubsystemC { public string C() => "C"; }

public class Facade
{
    private readonly SubsystemA _a = new SubsystemA();
    private readonly SubsystemB _b = new SubsystemB();
    private readonly SubsystemC _c = new SubsystemC();

    public string Operation() => $"Facade: {_a.A()}+{_b.B()}+{_c.C()}";

    public static void Run()
    {
        var f = new Facade();
        Console.WriteLine(f.Operation());
    }
}
