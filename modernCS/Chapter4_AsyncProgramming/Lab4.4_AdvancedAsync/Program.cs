using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Lab4_4_AdvancedAsync
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== Lab 4.4: Advanced Async Scenarios ===\n");

            // Demo 1: ValueTask for high-performance scenarios
            Console.WriteLine("1. ValueTask for High-Performance Scenarios");
            await DemonstrateValueTask();

            // Demo 2: Custom Awaitable Types
            Console.WriteLine("\n2. Custom Awaitable Types");
            await DemonstrateCustomAwaitable();

            // Demo 3: Async Method Builder
            Console.WriteLine("\n3. Async Method Builder");
            await DemonstrateAsyncMethodBuilder();

            // Demo 4: TaskCompletionSource
            Console.WriteLine("\n4. TaskCompletionSource");
            await DemonstrateTaskCompletionSource();

            // Demo 5: Async Local Storage
            Console.WriteLine("\n5. Async Local Storage");
            await DemonstrateAsyncLocal();

            // Demo 6: Dynamic Task Parallelism
            Console.WriteLine("\n6. Dynamic Task Parallelism");
            await DemonstrateDynamicParallelism();

            // Demo 7: Custom Synchronization Context
            Console.WriteLine("\n7. Custom Synchronization Context");
            await DemonstrateCustomSyncContext();

            Console.WriteLine("\n=== Lab 4.4 Complete ===");
        }

        static async Task DemonstrateValueTask()
        {
            Console.WriteLine("Demonstrating ValueTask for performance optimization...");

            var cache = new ValueTaskCache();

            // First call - creates task
            Console.WriteLine("First call (creates task):");
            var result1 = await cache.GetValueAsync("key1");
            Console.WriteLine($"Result: {result1}");

            // Second call - returns cached ValueTask
            Console.WriteLine("Second call (cached ValueTask):");
            var result2 = await cache.GetValueAsync("key1");
            Console.WriteLine($"Result: {result2}");

            // Synchronous fast path
            Console.WriteLine("Fast path call:");
            var result3 = await cache.GetValueFastAsync("fast");
            Console.WriteLine($"Result: {result3}");

            // Performance comparison
            Console.WriteLine("\nPerformance comparison:");
            await CompareTaskVsValueTask();
        }

        static async Task CompareTaskVsValueTask()
        {
            const int iterations = 100000;

            // Task-based approach
            var stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                await ReturnTask();
            }
            stopwatch.Stop();
            Console.WriteLine($"Task-based: {stopwatch.ElapsedMilliseconds}ms");

            // ValueTask-based approach
            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                await ReturnValueTask();
            }
            stopwatch.Stop();
            Console.WriteLine($"ValueTask-based: {stopwatch.ElapsedMilliseconds}ms");
        }

        static async Task<int> ReturnTask() => 42;
        static async ValueTask<int> ReturnValueTask() => 42;

        static async Task DemonstrateCustomAwaitable()
        {
            Console.WriteLine("Demonstrating custom awaitable types...");

            // Custom awaitable that completes after a delay
            var awaitable1 = new DelayAwaitable(TimeSpan.FromSeconds(1));
            Console.WriteLine("Starting custom awaitable...");
            await awaitable1;
            Console.WriteLine("Custom awaitable completed");

            // Custom awaitable with result
            var awaitable2 = new ResultAwaitable<string>("Custom result");
            var result2 = await awaitable2;
            Console.WriteLine($"Custom result: {result2}");

            // Custom awaitable with cancellation
            using var cts = new CancellationTokenSource();
            var awaitable3 = new CancellableAwaitable(cts.Token);

            // Cancel after 500ms
            _ = Task.Delay(500).ContinueWith(_ => cts.Cancel());

            try
            {
                await awaitable3;
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Custom awaitable was cancelled");
            }
        }

        static async Task DemonstrateAsyncMethodBuilder()
        {
            Console.WriteLine("Demonstrating custom async method builder...");

            // Using custom async method
            var result = await CustomAsyncMethod();
            Console.WriteLine($"Custom async method result: {result}");

            // Custom task-like type
            var customTask = new CustomTask<int>();
            customTask.SetResult(456);
            var customResult = await customTask;
            Console.WriteLine($"Custom task result: {customResult}");
        }

        static async CustomTask<int> CustomAsyncMethod()
        {
            await Task.Delay(200);
            return 123;
        }

        static async Task DemonstrateTaskCompletionSource()
        {
            Console.WriteLine("Demonstrating TaskCompletionSource...");

            // Manual task completion
            var tcs = new TaskCompletionSource<string>();

            // Complete the task asynchronously
            _ = Task.Delay(500).ContinueWith(_ =>
            {
                tcs.SetResult("Task completed manually");
            });

            var result = await tcs.Task;
            Console.WriteLine($"TCS result: {result}");

            // Exception handling
            var tcs2 = new TaskCompletionSource<int>();
            _ = Task.Delay(300).ContinueWith(_ =>
            {
                tcs2.SetException(new InvalidOperationException("Something went wrong"));
            });

            try
            {
                await tcs2.Task;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"TCS exception: {ex.Message}");
            }

            // Cancellation
            var tcs3 = new TaskCompletionSource<bool>();
            using var cts = new CancellationTokenSource(200);
            cts.Token.Register(() => tcs3.SetCanceled());

            try
            {
                await tcs3.Task;
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("TCS task was cancelled");
            }
        }

        static async Task DemonstrateAsyncLocal()
        {
            Console.WriteLine("Demonstrating AsyncLocal for ambient data...");

            var asyncLocal = new AsyncLocal<string>();

            // Set value in main context
            asyncLocal.Value = "Main context";

            Console.WriteLine($"Main context: {asyncLocal.Value}");

            // Start async operations that inherit the context
            var task1 = Task.Run(async () =>
            {
                Console.WriteLine($"Task 1 initial: {asyncLocal.Value}");
                asyncLocal.Value = "Task 1 context";
                await Task.Delay(100);
                Console.WriteLine($"Task 1 after change: {asyncLocal.Value}");

                // Nested operation
                await Task.Run(() =>
                {
                    Console.WriteLine($"Nested in Task 1: {asyncLocal.Value}");
                });
            });

            var task2 = Task.Run(async () =>
            {
                Console.WriteLine($"Task 2 initial: {asyncLocal.Value}");
                asyncLocal.Value = "Task 2 context";
                await Task.Delay(50);
                Console.WriteLine($"Task 2 after change: {asyncLocal.Value}");
            });

            await Task.WhenAll(task1, task2);

            // Back in main context
            Console.WriteLine($"Back in main: {asyncLocal.Value}");
        }

        static async Task DemonstrateDynamicParallelism()
        {
            Console.WriteLine("Demonstrating dynamic task parallelism...");

            var workItems = new Queue<int>(Enumerable.Range(1, 20));
            var results = new List<int>();
            var semaphore = new SemaphoreSlim(3); // Max 3 concurrent operations

            var tasks = new List<Task>();

            for (int i = 0; i < 3; i++) // 3 worker tasks
            {
                tasks.Add(Task.Run(async () =>
                {
                    while (true)
                    {
                        await semaphore.WaitAsync();

                        int workItem;
                        lock (workItems)
                        {
                            if (workItems.Count == 0)
                            {
                                semaphore.Release();
                                break;
                            }
                            workItem = workItems.Dequeue();
                        }

                        try
                        {
                            // Simulate work
                            await Task.Delay(workItem * 10);
                            var result = workItem * workItem;

                            lock (results)
                            {
                                results.Add(result);
                            }

                            Console.WriteLine($"Processed {workItem} -> {result}");
                        }
                        finally
                        {
                            semaphore.Release();
                        }
                    }
                }));
            }

            await Task.WhenAll(tasks);
            Console.WriteLine($"Total results: {results.Count}, Sum: {results.Sum()}");
        }

        static async Task DemonstrateCustomSyncContext()
        {
            Console.WriteLine("Demonstrating custom synchronization context...");

            var customContext = new CountingSynchronizationContext();
            var previousContext = SynchronizationContext.Current;
            SynchronizationContext.SetSynchronizationContext(customContext);

            try
            {
                // Operations in custom context
                await Task.Delay(100);
                await Task.Yield();

                var task = Task.Run(() => 42);
                var result = await task;

                Console.WriteLine($"Result: {result}");
                Console.WriteLine($"Custom context operations: {customContext.OperationCount}");
            }
            finally
            {
                SynchronizationContext.SetSynchronizationContext(previousContext);
            }
        }
    }

    // ValueTask cache example
    public class ValueTaskCache
    {
        private readonly Dictionary<string, ValueTask<int>> _cache = new();

        public ValueTask<int> GetValueAsync(string key)
        {
            if (_cache.TryGetValue(key, out var cached))
            {
                return cached;
            }

            var task = ComputeValueAsync(key);
            _cache[key] = new ValueTask<int>(task);
            return new ValueTask<int>(task);
        }

        public ValueTask<int> GetValueFastAsync(string key)
        {
            // Fast path - return synchronously
            return new ValueTask<int>(42);
        }

        private async Task<int> ComputeValueAsync(string key)
        {
            await Task.Delay(100); // Simulate computation
            return key.Length * 10;
        }
    }

    // Custom awaitable types
    public class DelayAwaitable
    {
        private readonly TimeSpan _delay;

        public DelayAwaitable(TimeSpan delay)
        {
            _delay = delay;
        }

        public DelayAwaiter GetAwaiter() => new DelayAwaiter(_delay);
    }

    public class DelayAwaiter : INotifyCompletion
    {
        private readonly TimeSpan _delay;
        private Task? _task;

        public DelayAwaiter(TimeSpan delay)
        {
            _delay = delay;
        }

        public bool IsCompleted => (_task?.IsCompleted ?? false);

        public void OnCompleted(Action continuation)
        {
            _task = Task.Delay(_delay).ContinueWith(_ => continuation());
        }

        public void GetResult() { }
    }

    public class ResultAwaitable<T>
    {
        private readonly T _result;

        public ResultAwaitable(T result)
        {
            _result = result;
        }

        public ResultAwaiter<T> GetAwaiter() => new ResultAwaiter<T>(_result);
    }

    public class ResultAwaiter<T> : INotifyCompletion
    {
        private readonly T _result;

        public ResultAwaiter(T result)
        {
            _result = result;
        }

        public bool IsCompleted => true;
        public T GetResult() => _result;
        public void OnCompleted(Action continuation) => continuation();
    }

    public class CancellableAwaitable
    {
        private readonly CancellationToken _token;

        public CancellableAwaitable(CancellationToken token)
        {
            _token = token;
        }

        public CancellableAwaiter GetAwaiter() => new CancellableAwaiter(_token);
    }

    public class CancellableAwaiter : INotifyCompletion
    {
        private readonly CancellationToken _token;

        public CancellableAwaiter(CancellationToken token)
        {
            _token = token;
        }

        public bool IsCompleted => _token.IsCancellationRequested;
        public void GetResult() => _token.ThrowIfCancellationRequested();
        public void OnCompleted(Action continuation) => continuation();
    }

    // Custom async method builder
    [AsyncMethodBuilder(typeof(CustomTaskMethodBuilder<>))]
    public class CustomTask<T>
    {
        private readonly TaskCompletionSource<T> _tcs = new();

        public CustomTaskAwaiter<T> GetAwaiter() => new CustomTaskAwaiter<T>(_tcs.Task);

        public void SetResult(T result) => _tcs.SetResult(result);
        public void SetException(Exception ex) => _tcs.SetException(ex);
    }

    public class CustomTaskAwaiter<T> : INotifyCompletion
    {
        private readonly Task<T> _task;

        public CustomTaskAwaiter(Task<T> task)
        {
            _task = task;
        }

        public bool IsCompleted => _task.IsCompleted;
        public T GetResult() => _task.GetAwaiter().GetResult();
        public void OnCompleted(Action continuation) => _task.GetAwaiter().OnCompleted(continuation);
    }

    public class CustomTaskMethodBuilder<T>
    {
        private CustomTask<T> _task = new();

        public static CustomTaskMethodBuilder<T> Create() => new();

        public void Start<TStateMachine>(ref TStateMachine stateMachine)
            where TStateMachine : IAsyncStateMachine
        {
            stateMachine.MoveNext();
        }

        public void SetStateMachine(IAsyncStateMachine stateMachine) { }

        public void SetResult(T result) => _task.SetResult(result);

        public void SetException(Exception exception) => _task.SetException(exception);

        public CustomTask<T> Task => _task;

        public void AwaitOnCompleted<TAwaiter, TStateMachine>(
            ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : INotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            awaiter.OnCompleted(stateMachine.MoveNext);
        }

        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(
            ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : INotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            AwaitOnCompleted(ref awaiter, ref stateMachine);
        }
    }

    // Custom synchronization context
    public class CountingSynchronizationContext : SynchronizationContext
    {
        public int OperationCount { get; private set; }

        public override void Post(SendOrPostCallback d, object? state)
        {
            OperationCount++;
            base.Post(d, state);
        }

        public override void Send(SendOrPostCallback d, object? state)
        {
            OperationCount++;
            base.Send(d, state);
        }
    }
}