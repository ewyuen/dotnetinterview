# Lab 2: gRPC Streaming Operations

## Objectives
- Understand different types of streaming in gRPC
- Implement server streaming (one request, multiple responses)
- Implement client streaming (multiple requests, one response)
- Implement bidirectional streaming (multiple requests and responses)
- Learn when to use each streaming pattern

## Concepts

### Types of Streaming

#### 1. Server Streaming
Client sends one request, server sends back a stream of responses.
```
Client ----(Request)----> Server
Client <---(Response 1)-- Server
Client <---(Response 2)-- Server
Client <---(Response 3)-- Server
```

**Use Cases:**
- Downloading large files in chunks
- Real-time data feeds (stock prices, sensor data)
- Paginated results
- Long-running computations with progress updates

#### 2. Client Streaming
Client sends a stream of requests, server sends back one response.
```
Client ----(Request 1)----> Server
Client ----(Request 2)----> Server
Client ----(Request 3)----> Server
Client <---(Response)------ Server
```

**Use Cases:**
- Uploading large files
- Batch processing
- Aggregating data from multiple sources
- Collecting telemetry/metrics

#### 3. Bidirectional Streaming
Both client and server send streams of messages independently.
```
Client ----(Request 1)----> Server
Client <---(Response 1)---- Server
Client ----(Request 2)----> Server
Client <---(Response 2)---- Server
```

**Use Cases:**
- Chat applications
- Real-time collaboration
- Live data synchronization
- Interactive gaming

## Project Structure

This lab includes:
1. **GrpcStreamingServer**: Server with all streaming types
2. **GrpcStreamingClient**: Client demonstrating all patterns

## Proto Definitions

```protobuf
syntax = "proto3";

option csharp_namespace = "GrpcStreaming";

// Server Streaming: Get weather updates
service Weather {
  rpc GetWeatherStream (WeatherRequest) returns (stream WeatherUpdate);
}

// Client Streaming: Upload sensor data
service Sensor {
  rpc UploadSensorData (stream SensorReading) returns (UploadSummary);
}

// Bidirectional Streaming: Chat service
service Chat {
  rpc Join (stream ChatMessage) returns (stream ChatMessage);
}
```

## Implementation Examples

### Server Streaming
```csharp
public override async Task GetWeatherStream(
    WeatherRequest request, 
    IServerStreamWriter<WeatherUpdate> responseStream, 
    ServerCallContext context)
{
    for (int i = 0; i < 10; i++)
    {
        // Check if client cancelled
        if (context.CancellationToken.IsCancellationRequested)
            break;
            
        await responseStream.WriteAsync(new WeatherUpdate
        {
            Temperature = Random.Shared.Next(60, 90),
            Timestamp = DateTime.UtcNow.ToString()
        });
        
        await Task.Delay(1000); // Send update every second
    }
}
```

### Client Streaming
```csharp
public override async Task<UploadSummary> UploadSensorData(
    IAsyncStreamReader<SensorReading> requestStream, 
    ServerCallContext context)
{
    int count = 0;
    double sum = 0;
    
    await foreach (var reading in requestStream.ReadAllAsync())
    {
        count++;
        sum += reading.Value;
    }
    
    return new UploadSummary
    {
        TotalReadings = count,
        AverageValue = sum / count
    };
}
```

### Bidirectional Streaming
```csharp
public override async Task Join(
    IAsyncStreamReader<ChatMessage> requestStream,
    IServerStreamWriter<ChatMessage> responseStream,
    ServerCallContext context)
{
    // In a real app, manage multiple client connections
    await foreach (var message in requestStream.ReadAllAsync())
    {
        // Broadcast to all clients (simplified here)
        await responseStream.WriteAsync(new ChatMessage
        {
            User = message.User,
            Text = message.Text,
            Timestamp = DateTime.UtcNow.ToString()
        });
    }
}
```

## Running the Lab

1. **Start the Server**:
   ```bash
   cd GrpcStreamingServer
   dotnet run
   ```

2. **Run the Client**:
   ```bash
   cd GrpcStreamingClient
   dotnet run
   ```

3. **Select a demo**:
   - Server Streaming Demo
   - Client Streaming Demo
   - Bidirectional Streaming Demo

## Key Concepts

### IServerStreamWriter<T>
Used on the server to write responses to the client in server/bidirectional streaming.

### IAsyncStreamReader<T>
Used on the server to read requests from the client in client/bidirectional streaming.

### CallOptions
Configure timeouts, deadlines, and cancellation tokens for streaming calls.

### Cancellation
Always handle cancellation tokens to allow graceful shutdown of long-running streams.

## Exercises

1. **Server Streaming**: Create a file download service that streams file contents in chunks
2. **Client Streaming**: Create an image upload service that receives image chunks
3. **Bidirectional**: Create a real-time calculator where client sends operations and server streams results
4. **Advanced**: Implement proper connection management for multiple chat clients

## Common Patterns

### Heartbeat
Send periodic messages to keep connection alive:
```csharp
while (!context.CancellationToken.IsCancellationRequested)
{
    await responseStream.WriteAsync(new Heartbeat());
    await Task.Delay(TimeSpan.FromSeconds(30));
}
```

### Buffering
Buffer messages before sending to improve performance:
```csharp
var buffer = new List<Message>();
await foreach (var msg in requestStream.ReadAllAsync())
{
    buffer.Add(msg);
    if (buffer.Count >= 100)
    {
        ProcessBatch(buffer);
        buffer.Clear();
    }
}
```

## Expected Output

```
=== Server Streaming Demo ===
Weather Update: 75°F at 2025-10-12 10:30:00
Weather Update: 78°F at 2025-10-12 10:30:01
Weather Update: 72°F at 2025-10-12 10:30:02
...

=== Client Streaming Demo ===
Uploading sensor reading 1: 23.5
Uploading sensor reading 2: 24.1
...
Upload complete! Total: 100 readings, Average: 24.3

=== Bidirectional Streaming Demo ===
[Alice] Hello everyone!
[Bob] Hi Alice!
[Alice] How's it going?
...
```

## Next Steps

Lab 3 covers advanced gRPC features:
- Error handling and custom status codes
- Metadata (request/response headers)
- Interceptors for logging and authentication
- Deadlines and timeouts
