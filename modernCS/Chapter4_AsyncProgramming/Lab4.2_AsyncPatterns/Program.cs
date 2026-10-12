using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Lab4_2_AsyncPatterns
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== Lab 4.2: Async Patterns and Best Practices ===\n");

            // Demo 1: Exception Handling in Async Methods
            Console.WriteLine("1. Exception Handling in Async Methods");
            await DemonstrateAsyncExceptionHandling();

            // Demo 2: Cancellation with CancellationToken
            Console.WriteLine("\n2. Cancellation with CancellationToken");
            await DemonstrateCancellation();

            // Demo 3: Timeout Patterns
            Console.WriteLine("\n3. Timeout Patterns");
            await DemonstrateTimeout();

            // Demo 4: Retry Patterns
            Console.WriteLine("\n4. Retry Patterns");
            await DemonstrateRetry();

            // Demo 5: Progress Reporting
            Console.WriteLine("\n5. Progress Reporting");
            await DemonstrateProgress();

            // Demo 6: Async Initialization (Async Lazy)
            Console.WriteLine("\n6. Async Initialization (Async Lazy)");
            await DemonstrateAsyncLazy();

            // Demo 7: Avoiding Common Pitfalls
            Console.WriteLine("\n7. Avoiding Common Pitfalls");
            await DemonstrateCommonPitfalls();

            Console.WriteLine("\n=== Lab 4.2 Complete ===");
        }

        static async Task DemonstrateAsyncExceptionHandling()
        {
            Console.WriteLine("Demonstrating async exception handling...");

            try
            {
                await FailingAsyncOperation();
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Caught expected exception: {ex.Message}");
            }

            // Handling AggregateException from multiple tasks
            Console.WriteLine("\nHandling multiple async exceptions:");
            var tasks = new List<Task>
            {
                FailingAsyncOperation(),
                SuccessfulAsyncOperation(),
                FailingAsyncOperation()
            };

            try
            {
                await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Caught exception from Task.WhenAll: {ex.Message}");
                if (ex is AggregateException aggregateEx)
                {
                    Console.WriteLine($"AggregateException with {aggregateEx.InnerExceptions.Count} inner exceptions");
                }
            }
        }

        static async Task FailingAsyncOperation()
        {
            await Task.Delay(100);
            throw new InvalidOperationException("This operation always fails");
        }

        static async Task SuccessfulAsyncOperation()
        {
            await Task.Delay(50);
            Console.WriteLine("Successful operation completed");
        }

        static async Task DemonstrateCancellation()
        {
            Console.WriteLine("Demonstrating cancellation patterns...");

            using var cts = new CancellationTokenSource();

            // Start a cancellable operation
            var operationTask = CancellableOperationAsync(cts.Token);

            // Cancel after 500ms
            await Task.Delay(500);
            Console.WriteLine("Requesting cancellation...");
            cts.Cancel();

            try
            {
                await operationTask;
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Operation was cancelled successfully");
            }

            // Demonstrate linked cancellation tokens
            Console.WriteLine("\nDemonstrating linked cancellation tokens:");
            using var parentCts = new CancellationTokenSource();
            using var childCts = CancellationTokenSource.CreateLinkedTokenSource(parentCts.Token);

            var linkedTask = CancellableOperationAsync(childCts.Token);

            // Cancel parent token
            parentCts.Cancel();

            try
            {
                await linkedTask;
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Linked operation was cancelled via parent token");
            }
        }

        static async Task CancellableOperationAsync(CancellationToken token)
        {
            for (int i = 0; i < 10; i++)
            {
                token.ThrowIfCancellationRequested();
                Console.WriteLine($"Working... {i + 1}/10");
                await Task.Delay(100, token);
            }
            Console.WriteLine("Operation completed");
        }

        static async Task DemonstrateTimeout()
        {
            Console.WriteLine("Demonstrating timeout patterns...");

            // Method 1: Using CancellationTokenSource with timeout
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(1));

            try
            {
                await SlowOperationAsync(cts.Token);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Operation timed out using CancellationTokenSource");
            }

            // Method 2: Using Task.WhenAny with delay
            Console.WriteLine("\nUsing Task.WhenAny for timeout:");
            var operationTask = SlowOperationAsync(CancellationToken.None);
            var timeoutTask = Task.Delay(800);

            var completedTask = await Task.WhenAny(operationTask, timeoutTask);

            if (completedTask == timeoutTask)
            {
                Console.WriteLine("Operation timed out using Task.WhenAny");
            }
            else
            {
                await operationTask; // Ensure operation completes
                Console.WriteLine("Operation completed before timeout");
            }
        }

        static async Task SlowOperationAsync(CancellationToken token)
        {
            await Task.Delay(2000, token);
            Console.WriteLine("Slow operation completed");
        }

        static async Task DemonstrateRetry()
        {
            Console.WriteLine("Demonstrating retry patterns...");

            // Simple retry with exponential backoff
            int maxRetries = 3;
            int attempt = 0;

            while (attempt < maxRetries)
            {
                try
                {
                    attempt++;
                    Console.WriteLine($"Attempt {attempt}/{maxRetries}");
                    await UnreliableOperationAsync(attempt);
                    Console.WriteLine("Operation succeeded!");
                    break;
                }
                catch (Exception ex) when (attempt < maxRetries)
                {
                    int delayMs = (int)Math.Pow(2, attempt) * 100; // Exponential backoff
                    Console.WriteLine($"Attempt {attempt} failed: {ex.Message}. Retrying in {delayMs}ms...");
                    await Task.Delay(delayMs);
                }
            }

            if (attempt >= maxRetries)
            {
                Console.WriteLine("All retry attempts failed");
            }
        }

        static async Task UnreliableOperationAsync(int attempt)
        {
            await Task.Delay(100);

            // Fail first two attempts
            if (attempt <= 2)
            {
                throw new Exception($"Simulated failure on attempt {attempt}");
            }
        }

        static async Task DemonstrateProgress()
        {
            Console.WriteLine("Demonstrating progress reporting...");

            var progress = new Progress<int>(percent =>
            {
                Console.WriteLine($"Progress: {percent}%");
            });

            await LongRunningOperationWithProgressAsync(progress);
            Console.WriteLine("Operation with progress completed");
        }

        static async Task LongRunningOperationWithProgressAsync(IProgress<int> progress)
        {
            for (int i = 0; i <= 100; i += 20)
            {
                progress?.Report(i);
                await Task.Delay(200);
            }
        }

        static async Task DemonstrateAsyncLazy()
        {
            Console.WriteLine("Demonstrating async lazy initialization...");

            var lazyResource = new AsyncLazy<ExpensiveResource>(async () =>
            {
                Console.WriteLine("Initializing expensive resource...");
                await Task.Delay(500);
                return new ExpensiveResource();
            });

            // First access - will initialize
            Console.WriteLine("First access:");
            var resource1 = await lazyResource.Value;
            resource1.Use();

            // Second access - will reuse
            Console.WriteLine("Second access:");
            var resource2 = await lazyResource.Value;
            resource2.Use();

            Console.WriteLine($"Same instance: {ReferenceEquals(resource1, resource2)}");
        }

        static async Task DemonstrateCommonPitfalls()
        {
            Console.WriteLine("Demonstrating common async pitfalls and solutions...");

            // Pitfall 1: Async void in non-event handlers
            Console.WriteLine("\nPitfall 1: Async void in regular methods");
            try
            {
                // This exception cannot be caught by the caller!
                FireAndForgetWithException();
                await Task.Delay(200); // Give it time to execute
            }
            catch (Exception ex)
            {
                Console.WriteLine($"This won't catch the exception: {ex.Message}");
            }

            // Pitfall 2: Blocking on async code
            Console.WriteLine("\nPitfall 2: Blocking on async code (don't do this!)");
            var stopwatch = Stopwatch.StartNew();
            // BAD: .Result or .Wait() can cause deadlocks
            // var result = AsyncOperation().Result; // DON'T DO THIS
            var result = await AsyncOperation(); // GOOD
            stopwatch.Stop();
            Console.WriteLine($"Async operation took {stopwatch.ElapsedMilliseconds}ms");

            // Pitfall 3: Multiple awaits on the same task
            Console.WriteLine("\nPitfall 3: Multiple awaits on the same task");
            var task = Task.Delay(300);
            await task;
            await task; // This is OK - task is already completed
            Console.WriteLine("Multiple awaits on same task completed");
        }

        static async void FireAndForgetWithException()
        {
            await Task.Delay(100);
            throw new Exception("This exception from async void cannot be caught!");
        }

        static async Task<string> AsyncOperation()
        {
            await Task.Delay(200);
            return "Async result";
        }
    }

    // Helper class for async lazy initialization
    public class AsyncLazy<T>
    {
        private readonly Lazy<Task<T>> _lazy;

        public AsyncLazy(Func<Task<T>> factory)
        {
            _lazy = new Lazy<Task<T>>(factory);
        }

        public Task<T> Value => _lazy.Value;
    }

    // Helper class for expensive resource
    public class ExpensiveResource
    {
        private int _usageCount;

        public void Use()
        {
            _usageCount++;
            Console.WriteLine($"Resource used {_usageCount} time(s)");
        }
    }
}