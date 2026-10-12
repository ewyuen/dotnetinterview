using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace Lab4._2_AsyncWeb;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
}

public class Post
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
}

public class Todo
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool Completed { get; set; }
}

class Program
{
    private static readonly HttpClient httpClient = new()
    {
        Timeout = TimeSpan.FromSeconds(30)
    };

    static async Task Main(string[] args)
    {
        Console.WriteLine("=== Lab 4.2: Async Web Requests ===\n");

        try
        {
            // Demonstrate basic HTTP operations
            await DemonstrateBasicHttpOperationsAsync();

            // Show JSON handling
            await DemonstrateJsonHandlingAsync();

            // Demonstrate retry logic
            await DemonstrateRetryLogicAsync();

            // Show error handling
            await DemonstrateErrorHandlingAsync();

            // Demonstrate concurrent requests
            await DemonstrateConcurrentRequestsAsync();

            // Show cancellation support
            await DemonstrateCancellationAsync();
        }
        finally
        {
            httpClient.Dispose();
        }

        Console.WriteLine("\n=== Lab Complete ===");
    }

    static async Task DemonstrateBasicHttpOperationsAsync()
    {
        Console.WriteLine("1. Basic HTTP Operations");
        Console.WriteLine("------------------------");

        // GET request - fetch user data
        Console.WriteLine("Fetching user data with GET...");
        var userResponse = await httpClient.GetAsync("https://jsonplaceholder.typicode.com/users/1");
        userResponse.EnsureSuccessStatusCode();

        var user = await userResponse.Content.ReadFromJsonAsync<User>();
        Console.WriteLine($"User: {user?.Name} ({user?.Email})");

        // GET request - fetch posts
        Console.WriteLine("\nFetching posts with GET...");
        var postsResponse = await httpClient.GetAsync("https://jsonplaceholder.typicode.com/posts?userId=1");
        postsResponse.EnsureSuccessStatusCode();

        var posts = await postsResponse.Content.ReadFromJsonAsync<List<Post>>();
        Console.WriteLine($"Found {posts?.Count} posts for user 1");

        // POST request - create new post
        Console.WriteLine("\nCreating new post with POST...");
        var newPost = new Post
        {
            UserId = 1,
            Title = "Async Web Requests Lab",
            Body = "This post was created using async HTTP operations in C#"
        };

        var postResponse = await httpClient.PostAsJsonAsync("https://jsonplaceholder.typicode.com/posts", newPost);
        postResponse.EnsureSuccessStatusCode();

        var createdPost = await postResponse.Content.ReadFromJsonAsync<Post>();
        Console.WriteLine($"Created post with ID: {createdPost?.Id}");

        // PUT request - update post
        Console.WriteLine("\nUpdating post with PUT...");
        var updatedPost = new Post
        {
            Id = createdPost!.Id,
            UserId = 1,
            Title = "Updated: Async Web Requests Lab",
            Body = "This post was updated using async HTTP PUT operation"
        };

        var putResponse = await httpClient.PutAsJsonAsync($"https://jsonplaceholder.typicode.com/posts/{createdPost.Id}", updatedPost);
        putResponse.EnsureSuccessStatusCode();
        Console.WriteLine("Post updated successfully");

        // DELETE request - delete post
        Console.WriteLine("\nDeleting post with DELETE...");
        var deleteResponse = await httpClient.DeleteAsync($"https://jsonplaceholder.typicode.com/posts/{createdPost.Id}");
        deleteResponse.EnsureSuccessStatusCode();
        Console.WriteLine("Post deleted successfully");

        Console.WriteLine();
    }

    static async Task DemonstrateJsonHandlingAsync()
    {
        Console.WriteLine("2. JSON Data Handling");
        Console.WriteLine("---------------------");

        // Fetch and parse JSON data
        Console.WriteLine("Fetching and parsing JSON data...");
        var response = await httpClient.GetAsync("https://jsonplaceholder.typicode.com/todos/1");
        response.EnsureSuccessStatusCode();

        // Read as string first
        var jsonString = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Raw JSON: {jsonString}");

        // Deserialize to object
        var todo = await response.Content.ReadFromJsonAsync<Todo>();
        Console.WriteLine($"Deserialized: ID={todo?.Id}, Title='{todo?.Title}', Completed={todo?.Completed}");

        // Serialize object to JSON
        Console.WriteLine("\nSerializing object to JSON...");
        var newTodo = new Todo
        {
            UserId = 1,
            Title = "Learn async web requests",
            Completed = false
        };

        var serializedJson = JsonSerializer.Serialize(newTodo, new JsonSerializerOptions { WriteIndented = true });
        Console.WriteLine($"Serialized JSON:\n{serializedJson}");

        Console.WriteLine();
    }

    static async Task DemonstrateRetryLogicAsync()
    {
        Console.WriteLine("3. Retry Logic with Exponential Backoff");
        Console.WriteLine("----------------------------------------");

        // Simulate unreliable endpoint with retry logic
        Console.WriteLine("Testing retry logic with simulated failures...");

        var retryPolicy = new RetryPolicy(
            maxRetries: 3,
            initialDelay: TimeSpan.FromSeconds(1),
            backoffMultiplier: 2.0
        );

        int attemptCount = 0;
        var result = await retryPolicy.ExecuteAsync(async () =>
        {
            attemptCount++;
            Console.WriteLine($"  Attempt {attemptCount}...");

            // Simulate occasional failures
            if (attemptCount < 3)
            {
                throw new HttpRequestException("Simulated network failure");
            }

            // On success, return some data
            return await httpClient.GetStringAsync("https://httpbin.org/get");
        });

        Console.WriteLine($"Request succeeded after {attemptCount} attempts");
        Console.WriteLine($"Response length: {result.Length} characters");

        Console.WriteLine();
    }

    static async Task DemonstrateErrorHandlingAsync()
    {
        Console.WriteLine("4. Error Handling in Web Requests");
        Console.WriteLine("----------------------------------");

        // Test various error scenarios
        var errorUrls = new[]
        {
            "https://httpbin.org/status/404",  // Not Found
            "https://httpbin.org/status/500",  // Server Error
            "https://nonexistent-domain-12345.com",  // DNS failure
            "https://httpbin.org/delay/35"    // Timeout (our client has 30s timeout)
        };

        foreach (var url in errorUrls)
        {
            Console.WriteLine($"Testing error handling for: {url}");
            try
            {
                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                Console.WriteLine($"  Success: {response.StatusCode}");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"  HttpRequestException: {ex.Message}");
            }
            catch (TaskCanceledException ex)
            {
                Console.WriteLine($"  TaskCanceledException (timeout): {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  Unexpected error: {ex.GetType().Name}: {ex.Message}");
            }
        }

        Console.WriteLine();
    }

    static async Task DemonstrateConcurrentRequestsAsync()
    {
        Console.WriteLine("5. Concurrent Web Requests");
        Console.WriteLine("--------------------------");

        // Make multiple requests concurrently
        Console.WriteLine("Making concurrent requests to fetch user data...");

        var userIds = new[] { 1, 2, 3, 4, 5 };
        var tasks = userIds.Select(id =>
            httpClient.GetFromJsonAsync<User>($"https://jsonplaceholder.typicode.com/users/{id}")
        );

        Console.WriteLine("Waiting for all requests to complete...");
        var users = await Task.WhenAll(tasks);

        Console.WriteLine("Results:");
        foreach (var user in users)
        {
            if (user != null)
            {
                Console.WriteLine($"  {user.Id}: {user.Name} ({user.Username})");
            }
        }

        // Demonstrate handling partial failures
        Console.WriteLine("\nTesting concurrent requests with some failures...");
        var mixedIds = new[] { 1, 2, 999, 4, 999 }; // Some valid, some invalid IDs
        var mixedTasks = mixedIds.Select(async id =>
        {
            try
            {
                return await httpClient.GetFromJsonAsync<User>($"https://jsonplaceholder.typicode.com/users/{id}");
            }
            catch
            {
                return null; // Return null for failed requests
            }
        });

        var mixedResults = await Task.WhenAll(mixedTasks);
        Console.WriteLine("Mixed results (null indicates failed request):");
        for (int i = 0; i < mixedResults.Length; i++)
        {
            var user = mixedResults[i];
            Console.WriteLine($"  ID {mixedIds[i]}: {(user != null ? user.Name : "FAILED")}");
        }

        Console.WriteLine();
    }

    static async Task DemonstrateCancellationAsync()
    {
        Console.WriteLine("6. Cancellation Support");
        Console.WriteLine("-----------------------");

        var cts = new CancellationTokenSource();

        // Start a slow request
        Console.WriteLine("Starting slow request (will be cancelled)...");
        var slowRequestTask = httpClient.GetAsync("https://httpbin.org/delay/10", cts.Token);

        // Cancel after 2 seconds
        await Task.Delay(2000);
        Console.WriteLine("Cancelling request...");
        cts.Cancel();

        try
        {
            var response = await slowRequestTask;
            Console.WriteLine("Request completed (unexpected)");
        }
        catch (TaskCanceledException)
        {
            Console.WriteLine("Request was cancelled successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
        }

        Console.WriteLine();
    }
}

public class RetryPolicy
{
    private readonly int _maxRetries;
    private readonly TimeSpan _initialDelay;
    private readonly double _backoffMultiplier;

    public RetryPolicy(int maxRetries, TimeSpan initialDelay, double backoffMultiplier)
    {
        _maxRetries = maxRetries;
        _initialDelay = initialDelay;
        _backoffMultiplier = backoffMultiplier;
    }

    public async Task<T> ExecuteAsync<T>(Func<Task<T>> operation)
    {
        var delay = _initialDelay;

        for (int attempt = 1; attempt <= _maxRetries + 1; attempt++)
        {
            try
            {
                return await operation();
            }
            catch (Exception ex) when (attempt <= _maxRetries)
            {
                Console.WriteLine($"  Attempt {attempt} failed: {ex.Message}");
                Console.WriteLine($"  Retrying in {delay.TotalSeconds} seconds...");

                await Task.Delay(delay);
                delay = TimeSpan.FromTicks((long)(delay.Ticks * _backoffMultiplier));
            }
        }

        throw new Exception($"Operation failed after {_maxRetries + 1} attempts");
    }
}