# Lab 3.1: Basic Threading

## Goal
Learn the fundamentals of threading in C#, including thread creation, synchronization, and communication between threads.

## Key Concepts
- **Thread Creation**: Creating and starting threads manually
- **Thread Synchronization**: Using locks to prevent race conditions
- **Thread Communication**: Producer-consumer pattern with Monitor
- **Thread Safety**: Writing code that works correctly in concurrent environments
- **Thread Lifecycle**: Understanding thread states and management

## Implementation
This lab demonstrates essential threading concepts:

1. **Thread Creation**: Creating threads with methods and lambdas
2. **Race Conditions**: Demonstrating unsafe shared state access
3. **Thread Synchronization**: Using `lock` for thread safety
4. **Producer-Consumer**: Thread communication with bounded buffer
5. **Thread Properties**: Priority and background thread behavior

## Key Features Demonstrated
- `Thread` class for manual thread management
- `lock` statement for mutual exclusion
- `Monitor.Wait()` and `Monitor.Pulse()` for thread communication
- Thread naming and priority settings
- Background vs foreground threads

## Expected Output
The program will show:
- Multiple threads executing concurrently
- Race condition effects (or lack thereof in simple cases)
- Thread-safe counter operations
- Producer-consumer communication
- Thread priority and background behavior

## Important Notes
- Race conditions may not always manifest due to timing
- Thread scheduling is non-deterministic
- Always consider thread safety when sharing state
- Use higher-level abstractions (Tasks) for new development