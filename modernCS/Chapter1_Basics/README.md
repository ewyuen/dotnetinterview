# Chapter 1: C# Object-Oriented Programming Basics

## Overview
This chapter covers the fundamental concepts of object-oriented programming in C#, providing a solid foundation for building robust and maintainable applications.

## Learning Objectives
- Understand the core principles of object-oriented programming
- Master class design and implementation
- Learn inheritance and polymorphism concepts
- Work with interfaces and abstract classes
- Implement generic types and collections
- Practice encapsulation and abstraction

## Key Concepts

### 1. Classes and Objects
- **Class Definition**: Blueprint for creating objects
- **Object Instantiation**: Creating instances of classes
- **Properties**: Controlled access to class data
- **Methods**: Behaviors and operations of objects

### 2. Inheritance
- **Base and Derived Classes**: Building hierarchies
- **Method Overriding**: Customizing inherited behavior
- **Polymorphism**: Treating derived objects as base types
- **Access Modifiers**: Controlling visibility (`public`, `private`, `protected`)

### 3. Interfaces and Abstract Classes
- **Interface Definition**: Contracts for class behavior
- **Abstract Classes**: Partial implementations with abstract methods
- **Multiple Interface Implementation**: A class implementing multiple contracts
- **Dependency Inversion**: Programming against abstractions

### 4. Generics
- **Generic Classes**: Type-safe reusable components
- **Generic Methods**: Type-parameterized operations
- **Type Constraints**: Limiting generic type parameters
- **Benefits**: Type safety, performance, code reuse

### 5. Collections and LINQ
- **Generic Collections**: `List<T>`, `Dictionary<TKey, TValue>`, etc.
- **LINQ Queries**: Declarative data manipulation
- **Lambda Expressions**: Anonymous methods for concise code
- **Extension Methods**: Adding functionality to existing types

## Important Learning Points

### Encapsulation
- Hide internal implementation details
- Expose only necessary interfaces
- Use properties for controlled access
- Maintain object invariants

### Polymorphism Benefits
- Write more flexible and extensible code
- Support plugin architectures
- Enable dependency injection
- Simplify complex conditional logic

### Interface Segregation
- Keep interfaces focused and minimal
- Prefer small, specific interfaces
- Avoid "fat" interfaces that do everything
- Enable better testability and mocking

### Generic Constraints
- Use `where T : class` for reference types
- Use `where T : struct` for value types
- Use `where T : new()` for types with parameterless constructors
- Use `where T : BaseClass` for inheritance constraints

## Labs Overview

### Lab 1.1: Classes and Objects
- Implement a `Book` class with properties and methods
- Practice object creation and method calls
- Learn basic encapsulation principles

### Lab 1.2: Inheritance
- Create a vehicle hierarchy with base and derived classes
- Implement method overriding
- Demonstrate polymorphism in action

### Lab 1.3: Interfaces and Generics
- Design and implement interfaces
- Create generic classes and methods
- Combine inheritance with interface implementation

### Lab 1.4: Collections and Final Integration
- Work with generic collections
- Implement a library management system
- Combine all concepts into a cohesive application

## References and Further Reading

### Official Microsoft Documentation
- [C# Programming Guide](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/)
- [Object-Oriented Programming Concepts](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/object-oriented-programming)
- [Generics in C#](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/generics/)
- [Interfaces in C#](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/interface)

### Books
- "C# 10 and .NET 6 – Modern Cross-Platform Development" by Mark J. Price
- "Pro C# 10 with .NET 6" by Andrew Troelsen and Phil Japikse
- "Head First C#" by Jennifer Greene and Andrew Stellman

### Design Patterns
- [Gang of Four Design Patterns](https://en.wikipedia.org/wiki/Design_Patterns)
- [SOLID Principles](https://en.wikipedia.org/wiki/SOLID)
- [Dependency Injection](https://martinfowler.com/articles/injection.html)

## Best Practices
- Always program against interfaces, not concrete implementations
- Use composition over inheritance when possible
- Keep classes focused on single responsibilities
- Write unit tests for all public methods
- Use meaningful names that reflect business domain
- Document complex business logic with comments

## Common Pitfalls to Avoid
- Deep inheritance hierarchies (favor composition)
- Large classes with multiple responsibilities
- Tight coupling between classes
- Overusing inheritance instead of interfaces
- Ignoring access modifiers (everything public by default)