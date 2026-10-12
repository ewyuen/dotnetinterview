// Abstract Factory Pattern
// Intent: Provide an interface for creating families of related or dependent objects without specifying their concrete classes.
// Use when your system needs to be independent of how its products are created and when you want to provide multiple families of products.

namespace Patterns;

// Abstract Factory
public interface IGUIFactory
{
    IButton CreateButton();
    ICheckbox CreateCheckbox();
}

public interface IButton { string Paint(); }
public interface ICheckbox { string Paint(); }

public class WinFactory : IGUIFactory
{
    public IButton CreateButton() => new WinButton();
    public ICheckbox CreateCheckbox() => new WinCheckbox();
}

public class MacFactory : IGUIFactory
{
    public IButton CreateButton() => new MacButton();
    public ICheckbox CreateCheckbox() => new MacCheckbox();
}

public class WinButton : IButton { public string Paint() => "Windows Button"; }
public class MacButton : IButton { public string Paint() => "Mac Button"; }
public class WinCheckbox : ICheckbox { public string Paint() => "Windows Checkbox"; }
public class MacCheckbox : ICheckbox { public string Paint() => "Mac Checkbox"; }

public static class AbstractFactoryPattern
{
    public static void Run()
    {
        IGUIFactory factory = new WinFactory();
        Console.WriteLine(factory.CreateButton().Paint());
        Console.WriteLine(factory.CreateCheckbox().Paint());

        factory = new MacFactory();
        Console.WriteLine(factory.CreateButton().Paint());
        Console.WriteLine(factory.CreateCheckbox().Paint());
    }
}
