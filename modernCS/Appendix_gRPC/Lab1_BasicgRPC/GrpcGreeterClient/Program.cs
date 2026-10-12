using Grpc.Core;
using Grpc.Net.Client;
using GrpcGreeter;

namespace GrpcStreamingClient;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("gRPC Client Starting...\n");

        // Create a gRPC channel to connect to the server
        // Note: In production, use HTTPS and proper certificate validation
        using var channel = GrpcChannel.ForAddress("http://localhost:7000");
        
        // Create the gRPC client from the generated code
        var client = new Greeter.GreeterClient(channel);

        try
        {
            // Example 1: Basic unary call
            Console.WriteLine("=== Example 1: Basic SayHello ===");
            await CallSayHello(client, "World");

            // Example 2: SayHelloAgain
            Console.WriteLine("\n=== Example 2: SayHelloAgain ===");
            await CallSayHelloAgain(client, "World");

            // Example 3: Get greeting count
            Console.WriteLine("\n=== Example 3: Get Greeting Count ===");
            await GetGreetingCount(client);

            // Example 4: Multiple calls
            Console.WriteLine("\n=== Example 4: Multiple Calls ===");
            await CallSayHello(client, "Alice");
            await CallSayHello(client, "Bob");
            await CallSayHelloAgain(client, "Charlie");
            await GetGreetingCount(client);

            // Example 5: Error handling - empty name
            Console.WriteLine("\n=== Example 5: Error Handling ===");
            await CallSayHello(client, "");
        }
        catch (RpcException ex)
        {
            Console.WriteLine($"RPC Error: {ex.Status.Detail}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }

    static async Task CallSayHello(Greeter.GreeterClient client, string name)
    {
        try
        {
            var request = new HelloRequest { Name = name };
            var reply = await client.SayHelloAsync(request);
            Console.WriteLine($"Greeting: {reply.Message}");
        }
        catch (RpcException ex)
        {
            Console.WriteLine($"Error calling SayHello: {ex.Status.Detail}");
        }
    }

    static async Task CallSayHelloAgain(Greeter.GreeterClient client, string name)
    {
        try
        {
            var request = new HelloRequest { Name = name };
            var reply = await client.SayHelloAgainAsync(request);
            Console.WriteLine($"Greeting Again: {reply.Message}");
        }
        catch (RpcException ex)
        {
            Console.WriteLine($"Error calling SayHelloAgain: {ex.Status.Detail}");
        }
    }

    static async Task GetGreetingCount(Greeter.GreeterClient client)
    {
        try
        {
            var request = new Empty();
            var reply = await client.GetGreetingCountAsync(request);
            Console.WriteLine($"Total greetings sent: {reply.Count}");
        }
        catch (RpcException ex)
        {
            Console.WriteLine($"Error calling GetGreetingCount: {ex.Status.Detail}");
        }
    }
}

