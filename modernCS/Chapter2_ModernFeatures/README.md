# Chapter 2: Modern C# Features (C# 8.0+)

## Overview
This chapter explores the modern language features introduced in C# 8.0 and later versions, focusing on improving code expressiveness, type safety, and developer productivity.

## Learning Objectives
- Master C# record types for immutable data
- Understand nullable reference types for safer code
- Implement advanced pattern matching techniques
- Use init-only setters for object initialization
- Combine modern features in real-world scenarios

## Key Concepts

### 1. Records (C# 9.0+)
- **Immutable Data Structures**: Value-based equality and immutability
- **Positional Records**: Concise syntax for data transfer objects
- **With Expressions**: Creating modified copies of records
- **Record Inheritance**: Building record hierarchies

### 2. Nullable Reference Types (C# 8.0+)
- **Null Safety**: Compiler warnings for potential null dereferences
- **Nullable Annotations**: `string?` vs `string` distinction
- **Null-Forgiving Operator**: `!` for overriding compiler warnings
- **Nullable Contexts**: Controlling nullable analysis

### 3. Pattern Matching (C# 8.0+)
- **Type Patterns**: Matching on types with variable declarations
- **Property Patterns**: Matching on object properties
- **Positional Patterns**: Deconstructing tuples and records
- **When Clauses**: Adding conditional guards to patterns
- **Switch Expressions**: Modern switch syntax returning values

### 4. Init-Only Setters (C# 9.0+)
- **Object Initializers**: Setting properties during construction only
- **Immutable Properties**: Properties that cannot be modified after creation
- **Init Accessor**: `init` keyword for property setters
- **Construction Flexibility**: Combining immutability with initialization ease

## Important Learning Points

### Records vs Classes
- **Records**: Use for immutable data, value equality, DTOs
- **Classes**: Use for mutable objects, reference equality, complex behavior
- **Performance**: Records can be more efficient for certain scenarios
- **Serialization**: Records work well with JSON serialization

### Nullable Reference Types Benefits
- **Early Detection**: Catch null reference exceptions at compile time
- **API Contracts**: Express nullability in method signatures
- **Documentation**: Type system documents null expectations
- **Migration**: Gradual adoption with nullable contexts

### Pattern Matching Power
- **Expressiveness**: Replace complex if-else chains with declarative patterns
- **Type Safety**: Compiler ensures exhaustive matching
- **Performance**: Can be optimized by the compiler
- **Readability**: Intent is clearer with pattern matching

### Init-Only Setters Philosophy
- **Immutability**: Create objects that cannot be modified after construction
- **Thread Safety**: Immutable objects are inherently thread-safe
- **Functional Programming**: Aligns with functional programming principles
- **Testing**: Easier to reason about and test immutable objects

## Labs Overview

### Lab 2.1: Records and Nullable Types
- Implement record types for data structures
- Work with nullable reference types
- Practice object initialization patterns
- Learn value-based equality

### Lab 2.2: Pattern Matching
- Implement switch expressions with property patterns
- Use type patterns for polymorphic behavior
- Apply when clauses for conditional matching
- Create fluent pattern matching logic

### Lab 2.3: Init-Only Setters
- Design classes with init-only properties
- Use object initializers effectively
- Implement immutable update patterns
- Combine with record types

### Lab 2.4: Final Component - Task Manager
- Integrate all Chapter 2 features
- Build a comprehensive task management system
- Demonstrate real-world usage patterns
- Combine records, nullable types, patterns, and init-only setters

## References and Further Reading

### Official Microsoft Documentation
- [Records (C# reference)](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/record)
- [Nullable reference types](https://docs.microsoft.com/en-us/dotnet/csharp/nullable-references)
- [Pattern matching overview](https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/functional/pattern-matching)
- [Object and collection initializers](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/object-and-collection-initializers)

### Language Feature Specifications
- [C# 8.0 Specification - Nullable Reference Types](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/proposals/csharp-8.0/nullable-reference-types)
- [C# 9.0 Specification - Records](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/proposals/csharp-9.0/records)
- [C# 9.0 Specification - Init-only setters](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/proposals/csharp-9.0/init)

### Books and Articles
- "C# 10 and .NET 6 – Modern Cross-Platform Development" by Mark J. Price
- [What's New in C# 8.0](https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-8)
- [What's New in C# 9.0](https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-9)
- [What's New in C# 10.0](https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-10)

### Design Patterns with Modern C#
- [Functional Programming in C#](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/functional-programming/)
- [Immutable Collections](https://docs.microsoft.com/en-us/dotnet/api/system.collections.immutable)
- [Discriminated Unions with Records](https://devblogs.microsoft.com/dotnet/c-9-0-on-the-record/)

## Best Practices

### Record Usage
- Use records for data transfer objects (DTOs)
- Prefer records for immutable domain models
- Override equality when needed for business logic
- Use positional records for simple data structures

### Nullable Types
- Enable nullable context in new projects
- Use `[NotNull]` attributes for external APIs
- Handle null checks explicitly rather than using `!`
- Document nullable parameters in XML comments

### Pattern Matching
- Use switch expressions over switch statements when returning values
- Prefer property patterns over complex if-else chains
- Use `when` clauses sparingly to avoid complex conditions
- Consider exhaustiveness for enum and union types

### Init-Only Properties
- Use init-only setters for required initialization data
- Combine with primary constructors in records
- Avoid init-only setters for frequently changing data
- Consider validation in init accessors

## Migration Considerations
- **Gradual Adoption**: Enable features incrementally
- **Backwards Compatibility**: Modern features don't break existing code
- **Nullable Migration**: Use compiler warnings to guide migration
- **Testing**: Ensure comprehensive test coverage during migration

## Common Patterns and Anti-Patterns

### Good Patterns
- Records for configuration objects
- Pattern matching in factory methods
- Init-only setters for validation
- Nullable types for optional dependencies

### Anti-Patterns to Avoid
- Overusing records for everything (classes still have their place)
- Complex pattern matching that hurts readability
- Ignoring nullable warnings
- Mutable records (defeats the purpose)