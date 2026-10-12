# Lab 2.3: Init-Only Setters

## Goal
Learn and practice init-only setters in C# 9.0, which allow properties to be set only during object initialization.

## Key Concepts
- **Init-Only Properties**: Properties that can only be set during object creation
- **Object Initializers**: Using `{ }` syntax to set init-only properties
- **Immutability**: Creating objects that cannot be modified after creation
- **With Expressions**: Creating modified copies of records

## Implementation
This lab demonstrates init-only setters with a `TodoTask` record:

1. **Init-Only Properties**: `Id`, `Title`, and `Description` can only be set during initialization
2. **Mutable Properties**: `IsCompleted` and `CreatedDate` can still be modified
3. **Object Initializers**: Setting init-only properties using `{ }` syntax
4. **With Expressions**: Creating new instances with modified properties

## Key Features Demonstrated
- `public int Id { get; init; }` - Init-only property syntax
- Object initializer syntax: `new TodoTask(1, "Title") { Description = "Desc" }`
- Attempting to modify init-only properties after creation (would cause compile errors)
- Using `with` expressions to create modified copies

## Expected Output
The program creates tasks with init-only properties, demonstrates that they cannot be modified after creation, shows how mutable properties can still be changed, and demonstrates 'with' expressions for creating new instances.