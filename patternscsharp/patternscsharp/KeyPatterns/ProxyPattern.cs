namespace Patterns;

// Use a different interface name to avoid collision with ObserverPattern's ISubject
public interface IProxySubject { string Request(); }
public class RealSubject : IProxySubject { public string Request() => "Real Subject"; }
public class Proxy : IProxySubject
{
    private RealSubject? _real;
    public string Request()
    {
        _real ??= new RealSubject();
        return $"Proxy -> {_real.Request()}";
    }

    public static void Run()
    {
        IProxySubject proxy = new Proxy();
        Console.WriteLine(proxy.Request());
    }
}
