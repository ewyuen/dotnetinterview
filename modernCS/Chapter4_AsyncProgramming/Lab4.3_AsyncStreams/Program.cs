using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Lab4_3_AsyncStreams
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== Lab 4.3: Async Streams ===\n");

            // Demo 1: Basic Async Iterator
            Console.WriteLine("1. Basic Async Iterator");
            await DemonstrateBasicAsyncIterator();

            // Demo 2: Async Streams with Cancellation
            Console.WriteLine("\n2. Async Streams with Cancellation");
            await DemonstrateAsyncStreamsWithCancellation();

            // Demo 3: Processing Large Files with Async Streams
            Console.WriteLine("\n3. Processing Large Files with Async Streams");
            await DemonstrateFileProcessing();

            // Demo 4: Async LINQ Operations
            Console.WriteLine("\n4. Async LINQ Operations");
            await DemonstrateAsyncLinq();

            // Demo 5: Infinite Async Streams
            Console.WriteLine("\n5. Infinite Async Streams");
            await DemonstrateInfiniteAsyncStream();

            // Demo 6: Combining Async Streams
            Console.WriteLine("\n6. Combining Async Streams");
            await DemonstrateCombiningStreams();

            // Demo 7: Custom Async Enumerator
            Console.WriteLine("\n7. Custom Async Enumerator");
            await DemonstrateCustomAsyncEnumerator();

            Console.WriteLine("\n=== Lab 4.3 Complete ===");
        }

        static async Task DemonstrateBasicAsyncIterator()
        {
            Console.WriteLine("Generating numbers asynchronously...");

            await foreach (var number in GenerateNumbersAsync(5))
            {
                Console.WriteLine($"Received: {number}");
            }

            Console.WriteLine("Basic async iteration completed");
        }

        static async IAsyncEnumerable<int> GenerateNumbersAsync(int count)
        {
            for (int i = 1; i <= count; i++)
            {
                await Task.Delay(200); // Simulate async work
                yield return i;
            }
        }

        static async Task DemonstrateAsyncStreamsWithCancellation()
        {
            Console.WriteLine("Async streams with cancellation...");

            using var cts = new CancellationTokenSource();

            // Start the async stream
            var enumerationTask = EnumerateWithCancellationAsync(cts.Token);

            // Cancel after 1 second
            await Task.Delay(1000);
            Console.WriteLine("Requesting cancellation...");
            cts.Cancel();

            try
            {
                await foreach (var item in enumerationTask)
                {
                    Console.WriteLine($"Received: {item}");
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Async stream enumeration was cancelled");
            }
        }

        static async IAsyncEnumerable<string> EnumerateWithCancellationAsync(
            [EnumeratorCancellation] CancellationToken token = default)
        {
            for (int i = 1; i <= 10; i++)
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(300, token);
                yield return $"Item {i}";
            }
        }

        static async Task DemonstrateFileProcessing()
        {
            string tempFile = Path.GetTempFileName();

            try
            {
                // Create a large file with multiple lines
                Console.WriteLine($"Creating large file: {tempFile}");
                var lines = new List<string>();
                for (int i = 1; i <= 100; i++)
                {
                    lines.Add($"Line {i}: {Guid.NewGuid()}");
                }
                await File.WriteAllLinesAsync(tempFile, lines);

                // Process file asynchronously line by line
                Console.WriteLine("Processing file asynchronously...");
                int processedCount = 0;

                await foreach (var line in ReadLinesAsync(tempFile))
                {
                    processedCount++;
                    if (processedCount <= 5) // Show first 5 lines
                    {
                        Console.WriteLine($"Processed: {line}");
                    }
                    else if (processedCount == 6)
                    {
                        Console.WriteLine("... (processing remaining lines)");
                    }
                }

                Console.WriteLine($"Total lines processed: {processedCount}");
            }
            finally
            {
                if (File.Exists(tempFile))
                {
                    File.Delete(tempFile);
                    Console.WriteLine("Temporary file cleaned up");
                }
            }
        }

        static async IAsyncEnumerable<string> ReadLinesAsync(string filePath)
        {
            using var reader = new StreamReader(filePath);

            string? line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                yield return line;
            }
        }

        static async Task DemonstrateAsyncLinq()
        {
            Console.WriteLine("Async LINQ operations...");

            var numbers = GenerateNumbersAsync(20);

            // Async Where
            Console.WriteLine("Even numbers:");
            var evenNumbers = numbers.WhereAwait(async x =>
            {
                await Task.Delay(10); // Simulate async condition
                return x % 2 == 0;
            });

            int count = 0;
            await foreach (var num in evenNumbers)
            {
                Console.WriteLine($"  {num}");
                count++;
                if (count >= 5) break;
            }

            // Async Select
            Console.WriteLine("\nSquared numbers:");
            await foreach (var num in GenerateNumbersAsync(5)
                .SelectAwait(async x =>
                {
                    await Task.Delay(50);
                    return x * x;
                }))
            {
                Console.WriteLine($"  {num}");
            }

            // Async aggregation
            var sum = await GenerateNumbersAsync(10)
                .AggregateAsync(0, async (acc, x) =>
                {
                    await Task.Delay(20);
                    return acc + x;
                });

            Console.WriteLine($"\nSum of first 10 numbers: {sum}");
        }

        static async Task DemonstrateInfiniteAsyncStream()
        {
            Console.WriteLine("Infinite async stream (will cancel after 5 items)...");

            using var cts = new CancellationTokenSource(2500); // Auto-cancel after 2.5s

            try
            {
                int count = 0;
                await foreach (var timestamp in GenerateTimestampsAsync(cts.Token))
                {
                    Console.WriteLine($"Timestamp: {timestamp:HH:mm:ss.fff}");
                    count++;

                    if (count >= 5) // Safety limit
                        break;
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Infinite stream cancelled");
            }
        }

        static async IAsyncEnumerable<DateTime> GenerateTimestampsAsync(
            [EnumeratorCancellation] CancellationToken token = default)
        {
            while (!token.IsCancellationRequested)
            {
                await Task.Delay(500, token);
                yield return DateTime.Now;
            }
        }

        static async Task DemonstrateCombiningStreams()
        {
            Console.WriteLine("Combining multiple async streams...");

            var stream1 = GenerateNumbersAsync(3);
            var stream2 = GenerateLettersAsync(3);

            // Interleave two streams
            Console.WriteLine("Interleaved streams:");
            await foreach (var item in InterleaveAsync<string>(
                stream1.SelectAwait(async x => { await Task.Delay(1); return x.ToString(); }), stream2))
            {
                Console.WriteLine($"  {item}");
            }

            // Merge and sort
            Console.WriteLine("\nMerged and sorted:");
            var merged = await MergeAndCollectAsync(
                GenerateNumbersAsync(5),
                GenerateNumbersAsync(5).SelectAwait(async x => { await Task.Delay(1); return x + 10; }));

            foreach (var item in merged.OrderBy(x => x))
            {
                Console.WriteLine($"  {item}");
            }
        }

        static async IAsyncEnumerable<string> GenerateLettersAsync(int count)
        {
            for (int i = 0; i < count; i++)
            {
                await Task.Delay(150);
                yield return ((char)('A' + i)).ToString();
            }
        }

        static async IAsyncEnumerable<T> InterleaveAsync<T>(
            IAsyncEnumerable<T> first,
            IAsyncEnumerable<T> second)
        {
            var firstEnumerator = first.GetAsyncEnumerator();
            var secondEnumerator = second.GetAsyncEnumerator();

            try
            {
                bool firstHasValue = await firstEnumerator.MoveNextAsync();
                bool secondHasValue = await secondEnumerator.MoveNextAsync();

                while (firstHasValue || secondHasValue)
                {
                    if (firstHasValue)
                    {
                        yield return firstEnumerator.Current;
                        firstHasValue = await firstEnumerator.MoveNextAsync();
                    }

                    if (secondHasValue)
                    {
                        yield return secondEnumerator.Current;
                        secondHasValue = await secondEnumerator.MoveNextAsync();
                    }
                }
            }
            finally
            {
                await firstEnumerator.DisposeAsync();
                await secondEnumerator.DisposeAsync();
            }
        }

        static async Task<List<int>> MergeAndCollectAsync(
            IAsyncEnumerable<int> first,
            IAsyncEnumerable<int> second)
        {
            var result = new List<int>();

            await foreach (var item in first)
            {
                result.Add(item);
            }

            await foreach (var item in second)
            {
                result.Add(item);
            }

            return result;
        }

        static async Task DemonstrateCustomAsyncEnumerator()
        {
            Console.WriteLine("Custom async enumerator implementation...");

            var fibonacciSource = new FibonacciAsyncSource();

            Console.WriteLine("Fibonacci sequence:");
            await foreach (var number in fibonacciSource.GenerateAsync(10))
            {
                Console.WriteLine($"  {number}");
            }
        }
    }

    // Custom async enumerable implementation
    public class FibonacciAsyncSource
    {
        public async IAsyncEnumerable<long> GenerateAsync(int count)
        {
            if (count <= 0) yield break;

            yield return 0;
            if (count == 1) yield break;

            yield return 1;
            if (count == 2) yield break;

            long prev = 0, current = 1;
            for (int i = 2; i < count; i++)
            {
                await Task.Delay(100); // Simulate async computation
                long next = prev + current;
                yield return next;
                prev = current;
                current = next;
            }
        }
    }

    // Extension methods for async LINQ operations
    public static class AsyncLinqExtensions
    {
        public static async IAsyncEnumerable<T> WhereAwait<T>(
            this IAsyncEnumerable<T> source,
            Func<T, Task<bool>> predicate)
        {
            await foreach (var item in source)
            {
                if (await predicate(item))
                {
                    yield return item;
                }
            }
        }

        public static async IAsyncEnumerable<TResult> SelectAwait<T, TResult>(
            this IAsyncEnumerable<T> source,
            Func<T, Task<TResult>> selector)
        {
            await foreach (var item in source)
            {
                yield return await selector(item);
            }
        }

        public static async Task<TAccumulate> AggregateAsync<T, TAccumulate>(
            this IAsyncEnumerable<T> source,
            TAccumulate seed,
            Func<TAccumulate, T, Task<TAccumulate>> accumulator)
        {
            TAccumulate result = seed;
            await foreach (var item in source)
            {
                result = await accumulator(result, item);
            }
            return result;
        }
    }
}