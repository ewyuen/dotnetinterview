using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Lab3_3_ParallelProgramming
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== Lab 3.3: Parallel Programming ===\n");

            // Demo 1: Parallel.ForEach
            Console.WriteLine("1. Parallel.ForEach");
            await DemonstrateParallelForEach();

            // Demo 2: Parallel LINQ (PLINQ)
            Console.WriteLine("\n2. Parallel LINQ (PLINQ)");
            await DemonstratePLINQ();

            // Demo 3: Parallel.For
            Console.WriteLine("\n3. Parallel.For");
            await DemonstrateParallelFor();

            // Demo 4: Parallel Options and Cancellation
            Console.WriteLine("\n4. Parallel Options and Cancellation");
            await DemonstrateParallelOptions();

            // Demo 5: Exception Handling in Parallel Operations
            Console.WriteLine("\n5. Exception Handling in Parallel Operations");
            await DemonstrateParallelExceptionHandling();

            // Demo 6: Performance Comparison
            Console.WriteLine("\n6. Performance Comparison");
            await DemonstratePerformanceComparison();

            Console.WriteLine("\n=== Lab 3.3 Complete ===");
        }

        static async Task DemonstrateParallelForEach()
        {
            Console.WriteLine("Processing items with Parallel.ForEach...");

            var items = Enumerable.Range(1, 10);
            var results = new List<int>();

            // Parallel.ForEach with thread-safe collection
            Parallel.ForEach(items, item =>
            {
                Console.WriteLine($"Processing {item} on thread {Thread.CurrentThread.ManagedThreadId}");
                Thread.Sleep(100); // Simulate work
                lock (results)
                {
                    results.Add(item * item);
                }
            });

            Console.WriteLine($"Results: {string.Join(", ", results.OrderBy(x => x))}");
        }

        static async Task DemonstratePLINQ()
        {
            Console.WriteLine("Using Parallel LINQ (PLINQ)...");

            var numbers = Enumerable.Range(1, 1000000);

            // Sequential LINQ
            var stopwatch = Stopwatch.StartNew();
            var sequentialResult = numbers.Where(n => n % 2 == 0)
                                         .Select(n => n * n)
                                         .Take(10)
                                         .ToList();
            stopwatch.Stop();
            Console.WriteLine($"Sequential LINQ took: {stopwatch.ElapsedMilliseconds}ms");

            // Parallel LINQ
            stopwatch.Restart();
            var parallelResult = numbers.AsParallel()
                                       .Where(n => n % 2 == 0)
                                       .Select(n => n * n)
                                       .Take(10)
                                       .ToList();
            stopwatch.Stop();
            Console.WriteLine($"Parallel LINQ took: {stopwatch.ElapsedMilliseconds}ms");

            Console.WriteLine($"Sequential results: {string.Join(", ", sequentialResult)}");
            Console.WriteLine($"Parallel results: {string.Join(", ", parallelResult)}");

            // PLINQ with ordering preservation
            Console.WriteLine("\nPLINQ with ordering:");
            var orderedResult = numbers.AsParallel()
                                      .AsOrdered()
                                      .Where(n => n % 100000 == 0)
                                      .Take(5)
                                      .ToList();
            Console.WriteLine($"Ordered results: {string.Join(", ", orderedResult)}");
        }

        static async Task DemonstrateParallelFor()
        {
            Console.WriteLine("Using Parallel.For...");

            const int size = 100;
            var array = new int[size];
            var results = new int[size];

            // Initialize array
            for (int i = 0; i < size; i++)
                array[i] = i + 1;

            // Parallel.For for transformation
            Parallel.For(0, size, i =>
            {
                results[i] = array[i] * array[i];
                Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}: {array[i]} -> {results[i]}");
            });

            Console.WriteLine($"First 10 results: {string.Join(", ", results.Take(10))}");

            // Parallel.For with local state
            Console.WriteLine("\nParallel.For with thread-local state:");
            var threadSums = new System.Collections.Concurrent.ConcurrentBag<long>();

            Parallel.For(0, size,
                () => 0L, // Initialize thread-local state
                (i, state, localSum) => localSum + array[i], // Body
                localSum => threadSums.Add(localSum) // Final action
            );

            Console.WriteLine($"Thread-local sums: {string.Join(", ", threadSums)}");
            Console.WriteLine($"Total sum: {threadSums.Sum()}");
        }

        static async Task DemonstrateParallelOptions()
        {
            Console.WriteLine("Demonstrating Parallel Options...");

            var items = Enumerable.Range(1, 20);

            // Parallel options with degree of parallelism
            var parallelOptions = new ParallelOptions
            {
                MaxDegreeOfParallelism = 4 // Limit to 4 concurrent operations
            };

            Parallel.ForEach(items, parallelOptions, item =>
            {
                Console.WriteLine($"Processing {item} on thread {Thread.CurrentThread.ManagedThreadId}");
                Thread.Sleep(50);
            });

            // Parallel with cancellation
            Console.WriteLine("\nParallel with cancellation:");
            using var cts = new CancellationTokenSource();
            parallelOptions.CancellationToken = cts.Token;

            Task.Run(async () =>
            {
                await Task.Delay(200);
                Console.WriteLine("Cancelling parallel operation...");
                cts.Cancel();
            });

            try
            {
                Parallel.ForEach(items, parallelOptions, item =>
                {
                    parallelOptions.CancellationToken.ThrowIfCancellationRequested();
                    Console.WriteLine($"Processing {item}");
                    Thread.Sleep(30);
                });
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Parallel operation was cancelled");
            }
        }

        static async Task DemonstrateParallelExceptionHandling()
        {
            Console.WriteLine("Demonstrating exception handling in parallel operations...");

            var items = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            try
            {
                Parallel.ForEach(items, item =>
                {
                    if (item == 5)
                        throw new InvalidOperationException($"Error processing item {item}");

                    Console.WriteLine($"Successfully processed {item}");
                    Thread.Sleep(20);
                });
            }
            catch (AggregateException ae)
            {
                Console.WriteLine($"Caught {ae.InnerExceptions.Count} exception(s):");
                foreach (var ex in ae.InnerExceptions)
                {
                    Console.WriteLine($"  {ex.Message}");
                }
            }

            // Handling exceptions in PLINQ
            Console.WriteLine("\nPLINQ with exception handling:");
            try
            {
                var result = items.AsParallel()
                                 .Select(item =>
                                 {
                                     if (item == 3)
                                         throw new ArgumentException($"Invalid item: {item}");
                                     return item * 2;
                                 })
                                 .ToList();
            }
            catch (AggregateException ae)
            {
                Console.WriteLine($"PLINQ caught exception: {ae.InnerException?.Message}");
            }
        }

        static async Task DemonstratePerformanceComparison()
        {
            Console.WriteLine("Performance comparison: Sequential vs Parallel");

            var data = Enumerable.Range(1, 100000).ToArray();

            // Sequential processing
            var stopwatch = Stopwatch.StartNew();
            var sequentialSum = data.Sum(x => (long)x);
            stopwatch.Stop();
            Console.WriteLine($"Sequential sum: {sequentialSum} (took {stopwatch.ElapsedMilliseconds}ms)");

            // Parallel processing
            stopwatch.Restart();
            var parallelSum = data.AsParallel().Sum(x => (long)x);
            stopwatch.Stop();
            Console.WriteLine($"Parallel sum: {parallelSum} (took {stopwatch.ElapsedMilliseconds}ms)");

            // CPU-intensive operation comparison
            Console.WriteLine("\nCPU-intensive operations:");

            // Sequential
            stopwatch.Restart();
            var sequentialResult = data.Select(CalculateComplex).ToArray();
            stopwatch.Stop();
            Console.WriteLine($"Sequential complex calc took: {stopwatch.ElapsedMilliseconds}ms");

            // Parallel
            stopwatch.Restart();
            var parallelResult = data.AsParallel().Select(CalculateComplex).ToArray();
            stopwatch.Stop();
            Console.WriteLine($"Parallel complex calc took: {stopwatch.ElapsedMilliseconds}ms");

            Console.WriteLine($"Results match: {sequentialResult.SequenceEqual(parallelResult)}");
        }

        static double CalculateComplex(int n)
        {
            double result = n;
            for (int i = 0; i < 1000; i++)
            {
                result = Math.Sqrt(result + 1);
            }
            return result;
        }
    }
}