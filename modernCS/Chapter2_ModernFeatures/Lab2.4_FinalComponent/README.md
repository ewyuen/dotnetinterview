# Lab 2.4: Final Component - Task Manager

## Goal
Create a comprehensive task management system that combines all Chapter 2 features: Records, Nullable Types, Pattern Matching, and Init-Only Setters.

## Key Concepts
- **Records**: Immutable data structures with value-based equality
- **Nullable Types**: Handling optional values safely
- **Pattern Matching**: Advanced conditional logic with switch expressions
- **Init-Only Setters**: Properties set only during object initialization
- **With Expressions**: Creating modified copies of immutable objects

## Implementation
This lab implements a complete `TaskItem` record with:

1. **Init-Only Properties**: `Id`, `Title`, `Description`, `Priority`, `DueDate`, `Tags`
2. **Nullable Types**: `Description?`, `DueDate?` for optional values
3. **Pattern Matching**: Task analysis and filtering using switch expressions
4. **Immutability**: All updates create new instances using 'with' expressions

## Key Features Demonstrated
- **Record Creation**: Using object initializers with init-only properties
- **Pattern Matching**: Complex conditional logic in `TaskAnalyzer.AnalyzeTask()`
- **Filtering**: LINQ queries with pattern matching in `TaskFilter`
- **Immutability**: Methods like `MarkCompleted()`, `UpdateDescription()` return new instances
- **Advanced Patterns**: Property patterns, when clauses, type patterns

## Expected Output
The program creates a task management system that:
- Creates tasks with various priorities and nullable properties
- Analyzes tasks using pattern matching
- Filters tasks by different criteria
- Demonstrates immutability with 'with' expressions
- Shows comprehensive task management statistics
- Uses advanced pattern matching for status determination

## Architecture
- `TaskItem`: Core record with all Chapter 2 features
- `TaskAnalyzer`: Static class with pattern matching methods
- `TaskFilter`: Record for filtering criteria
- `TaskPriority` & `TaskStatus`: Enums for type safety