using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;

namespace Lab4_1_BasicAsyncAwait
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== Lab 4.1: Basic Async/Await ===\n");

            // Demo 1: Basic async method
            Console.WriteLine("1. Basic Async Method");
            await DemonstrateBasicAsync();

            // Demo 2: Async with return value
            Console.WriteLine("\n2. Async Method with Return Value");
            await DemonstrateAsyncWithResult();

            // Demo 3: Multiple concurrent async operations
            Console.WriteLine("\n3. Multiple Concurrent Async Operations");
            await DemonstrateConcurrentAsync();

            // Demo 4: Async void methods (fire-and-forget)
            Console.WriteLine("\n4. Async Void Methods (Fire-and-Forget)");
            await DemonstrateAsyncVoid();

            // Demo 5: Async file operations
            Console.WriteLine("\n5. Async File Operations");
            await DemonstrateAsyncFileOperations();

            // Demo 6: Async HTTP operations
            Console.WriteLine("\n6. Async HTTP Operations");
            await DemonstrateAsyncHttp();

            Console.WriteLine("\n=== Lab 4.1 Complete ===");
        }

        static async Task DemonstrateBasicAsync()
        {
            Console.WriteLine("Starting basic async operation...");
            await Task.Delay(1000);
            Console.WriteLine("Basic async operation completed");
        }

        static async Task DemonstrateAsyncWithResult()
        {
            Console.WriteLine("Getting async result...");
            int result = await GetNumberAsync();
            Console.WriteLine($"Async result: {result}");

            string message = await GetMessageAsync();
            Console.WriteLine($"Async message: {message}");
        }

        static async Task<int> GetNumberAsync()
        {
            await Task.Delay(500);
            return 42;
        }

        static async Task<string> GetMessageAsync()
        {
            await Task.Delay(300);
            return "Hello from async method!";
        }

        static async Task DemonstrateConcurrentAsync()
        {
            Console.WriteLine("Starting multiple concurrent operations...");

            // Start multiple async operations concurrently
            Task<int> task1 = SlowOperationAsync("Task 1", 1000);
            Task<int> task2 = SlowOperationAsync("Task 2", 800);
            Task<int> task3 = SlowOperationAsync("Task 3", 1200);

            // Wait for all to complete
            int[] results = await Task.WhenAll(task1, task2, task3);

            Console.WriteLine("All concurrent operations completed:");
            for (int i = 0; i < results.Length; i++)
            {
                Console.WriteLine($"  Result {i + 1}: {results[i]}");
            }

            // Demonstrate Task.WhenAny
            Console.WriteLine("\nWaiting for first completion...");
            Task<int> firstCompleted = await Task.WhenAny(task1, task2, task3);
            int firstResult = await firstCompleted;
            Console.WriteLine($"First completed result: {firstResult}");
        }

        static async Task<int> SlowOperationAsync(string name, int delayMs)
        {
            Console.WriteLine($"{name} starting...");
            await Task.Delay(delayMs);
            Console.WriteLine($"{name} completed");
            return delayMs;
        }

        static async Task DemonstrateAsyncVoid()
        {
            Console.WriteLine("Starting fire-and-forget operations...");

            // Fire-and-forget async operations
            FireAndForgetAsync("Operation A");
            FireAndForgetAsync("Operation B");
            FireAndForgetAsync("Operation C");

            // Give them time to complete
            await Task.Delay(1500);
            Console.WriteLine("Main thread continuing after fire-and-forget operations");
        }

        static async void FireAndForgetAsync(string name)
        {
            Console.WriteLine($"{name} started (fire-and-forget)");
            await Task.Delay(500);
            Console.WriteLine($"{name} completed (fire-and-forget)");
        }

        static async Task DemonstrateAsyncFileOperations()
        {
            string tempFile = Path.GetTempFileName();
            string content = "This is test content for async file operations.\nLine 2\nLine 3";

            try
            {
                Console.WriteLine($"Writing to file asynchronously: {tempFile}");

                // Async write
                await File.WriteAllTextAsync(tempFile, content);
                Console.WriteLine("File written successfully");

                // Async read
                string readContent = await File.ReadAllTextAsync(tempFile);
                Console.WriteLine("File read successfully:");
                Console.WriteLine(readContent);

                // Async read lines
                string[] lines = await File.ReadAllLinesAsync(tempFile);
                Console.WriteLine($"File has {lines.Length} lines");
            }
            finally
            {
                // Cleanup
                if (File.Exists(tempFile))
                {
                    File.Delete(tempFile);
                    Console.WriteLine("Temporary file cleaned up");
                }
            }
        }

        static async Task DemonstrateAsyncHttp()
        {
            using var httpClient = new HttpClient();

            try
            {
                Console.WriteLine("Making async HTTP request...");

                // Async HTTP GET request
                HttpResponseMessage response = await httpClient.GetAsync("https://httpbin.org/get");

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("HTTP request successful");
                    Console.WriteLine($"Response length: {content.Length} characters");

                    // Parse JSON response (simplified)
                    if (content.Contains("\"url\""))
                    {
                        Console.WriteLine("Response contains expected JSON structure");
                    }
                }
                else
                {
                    Console.WriteLine($"HTTP request failed: {response.StatusCode}");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP request error: {ex.Message}");
                Console.WriteLine("Note: This demo requires internet connection");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}