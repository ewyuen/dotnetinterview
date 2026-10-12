using System;
using Patterns;


Console.WriteLine("Design Patterns Demo - running patterns from simple to complex\n");

static void RunPattern(string title, Action action)
{
	Console.WriteLine($"=== {title} ===");
	try
	{
		action();
	}
	catch (Exception ex)
	{
		Console.WriteLine($"Error running {title}: {ex.Message}");
	}
	Console.WriteLine();
}

// Creational patterns (simple -> more advanced)
RunPattern("Creational - Singleton (eager)", SingletonPattern.Run);
RunPattern("Creational - Singleton (thread-safe)", SingletonPatternLock.Run);
RunPattern("Creational - Singleton (lazy)", SingletonPatternLazy.Run);
RunPattern("Creational - Factory Method", FactoryMethodPattern.Run);
RunPattern("Creational - Abstract Factory", AbstractFactoryPattern.Run);
RunPattern("Creational - Builder", BuilderPattern.Run);
RunPattern("Creational - Prototype", Prototype.Run);
RunPattern("Creational - Dependency Injection (simple)", DependencyInjectionPattern.Run);

// Structural patterns
RunPattern("Structural - Adapter", Adapter.Run);
RunPattern("Structural - Bridge", RefinedAbstraction.Run);
RunPattern("Structural - Composite", Composite.Run);
RunPattern("Structural - Decorator", DecoratorPattern.Run);
RunPattern("Structural - Facade", Facade.Run);
RunPattern("Structural - Flyweight", FlyweightPattern.Run);
RunPattern("Structural - Proxy", Proxy.Run);

// Behavioral patterns
RunPattern("Behavioral - Chain of Responsibility", ChainOfResponsibilityPattern.Run);
RunPattern("Behavioral - Command", Invoker.RunDemo);
RunPattern("Behavioral - Interpreter", Interpreter.Run);
RunPattern("Behavioral - Iterator", ConcreteIterator.Run);
RunPattern("Behavioral - Mediator", ColleagueB.Run);
RunPattern("Behavioral - Memento", Originator.Run);
RunPattern("Behavioral - Observer", ConcreteObserver.Run);
RunPattern("Behavioral - Null Object", NullObjectPattern.Run);
RunPattern("Behavioral - State", ConcreteStateB.Run);
RunPattern("Behavioral - Strategy", ContextStrategy.Run);
RunPattern("Behavioral - Template Method", ConcreteClass.Run);

Console.WriteLine("All patterns executed.");
