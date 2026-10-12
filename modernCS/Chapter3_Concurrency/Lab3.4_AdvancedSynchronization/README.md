# Lab 3.4: Advanced Synchronization

## Goal
Learn advanced synchronization primitives beyond basic locks for complex concurrent scenarios.

## Key Concepts
- **Semaphore**: Controlling access to a pool of resources
- **SemaphoreSlim**: Lightweight semaphore for async scenarios
- **Mutex**: Cross-process synchronization and mutual exclusion
- **ReaderWriterLock**: Optimized for read-heavy scenarios
- **ReaderWriterLockSlim**: Improved performance version of ReaderWriterLock
- **Combining Primitives**: Using multiple synchronization types together

## Implementation
This lab demonstrates advanced synchronization primitives:

1. **Semaphore**: Limits concurrent access to resources (e.g., database connections)
2. **SemaphoreSlim**: Async-compatible semaphore with better performance
3. **Mutex**: Named mutexes for cross-process synchronization
4. **ReaderWriterLock**: Allows multiple readers or single writer access
5. **ReaderWriterLockSlim**: Modern, faster version with upgradeable locks
6. **Combined Usage**: Resource pool with semaphore and reader-writer lock

## Key Features Demonstrated
- `Semaphore` for resource pool management
- `SemaphoreSlim.WaitAsync()` for async-compatible synchronization
- `Mutex` for exclusive cross-process access
- `ReaderWriterLock` for read-heavy concurrent access patterns
- `ReaderWriterLockSlim` for better performance
- Combining multiple primitives for complex synchronization scenarios

## Expected Output
The program will show:
- Semaphore controlling concurrent resource access
- SemaphoreSlim with async/await patterns
- Mutex providing exclusive access across operations
- Multiple readers accessing data simultaneously
- Writers blocking all other access during updates
- Combined primitives managing a producer-consumer resource pool

## Important Notes
- Choose the right primitive for your access pattern (read-heavy vs write-heavy)
- ReaderWriterLock is ideal when reads >> writes
- Semaphore is perfect for resource pools with fixed capacity
- Mutex provides cross-process synchronization but has higher overhead
- Always use the Slim versions when possible for better performance
- Consider lock hierarchy to avoid deadlocks when combining primitives

## References
- [Semaphore and SemaphoreSlim](https://docs.microsoft.com/en-us/dotnet/api/system.threading.semaphore)
- [Mutex Class](https://docs.microsoft.com/en-us/dotnet/api/system.threading.mutex)
- [ReaderWriterLock](https://docs.microsoft.com/en-us/dotnet/api/system.threading.readerwriterlock)
- [ReaderWriterLockSlim](https://docs.microsoft.com/en-us/dotnet/api/system.threading.readerwriterlockslim)