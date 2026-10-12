using System;
using System.Collections.Generic;

namespace Patterns;

public interface IService { void Do(); }
public class Service : IService { public void Do() => Console.WriteLine("Service doing work"); }

// Simple DI container to demonstrate the pattern without external packages
public class SimpleContainer
{
    private readonly Dictionary<Type, Func<object>> _registrations = new();
    public void Register<TService, TImpl>() where TImpl : TService, new()
    {
        _registrations[typeof(TService)] = () => new TImpl();
    }

    public TService Resolve<TService>()
    {
        if (!_registrations.TryGetValue(typeof(TService), out var factory))
            throw new InvalidOperationException($"Service {typeof(TService)} not registered");
        return (TService)factory();
    }
}

public static class DependencyInjectionPattern
{
    public static void Run()
    {
        var container = new SimpleContainer();
        container.Register<IService, Service>();
        var s = container.Resolve<IService>();
        s.Do();
    }
}
