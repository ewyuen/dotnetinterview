# Lab 3: Advanced gRPC Features

## Objectives
- Master error handling with status codes
- Use metadata for headers and custom information
- Implement interceptors for cross-cutting concerns
- Configure deadlines and timeouts
- Add authentication and authorization

## Concepts

### 1. Error Handling

gRPC uses status codes similar to HTTP but specific to RPC operations.

**Common Status Codes:**
- `OK`: Success
- `CANCELLED`: Operation cancelled by client
- `INVALID_ARGUMENT`: Client specified invalid argument
- `NOT_FOUND`: Requested entity not found
- `ALREADY_EXISTS`: Entity already exists
- `PERMISSION_DENIED`: Access denied
- `UNAUTHENTICATED`: Request not authenticated
- `RESOURCE_EXHAUSTED`: Resource exhausted (rate limiting)
- `FAILED_PRECONDITION`: Operation rejected (invalid state)
- `INTERNAL`: Internal server error
- `UNAVAILABLE`: Service unavailable
- `DEADLINE_EXCEEDED`: Deadline expired before completion

### 2. Metadata

Metadata is similar to HTTP headers - key-value pairs sent with requests/responses.

**Use Cases:**
- Authentication tokens
- Request IDs for tracing
- Client information
- Custom application headers

### 3. Interceptors

Interceptors allow you to intercept gRPC calls to add cross-cutting functionality.

**Use Cases:**
- Logging all requests/responses
- Authentication/authorization
- Performance monitoring
- Error handling
- Retry logic

### 4. Deadlines and Timeouts

Deadlines prevent operations from running indefinitely.

**Best Practices:**
- Always set deadlines for RPC calls
- Propagate deadlines across service boundaries
- Handle deadline exceeded errors gracefully

## Project Structure

This lab includes:
1. **GrpcAdvancedServer**: Server with error handling, metadata, and interceptors
2. **GrpcAdvancedClient**: Client demonstrating advanced features

## Proto Definitions

```protobuf
syntax = "proto3";

option csharp_namespace = "GrpcAdvanced";

service ProductService {
  // Get a product by ID (demonstrates NOT_FOUND)
  rpc GetProduct (ProductRequest) returns (Product);
  
  // Create a product (demonstrates ALREADY_EXISTS)
  rpc CreateProduct (CreateProductRequest) returns (Product);
  
  // Long-running operation (demonstrates DEADLINE_EXCEEDED)
  rpc ProcessOrder (OrderRequest) returns (OrderResult);
  
  // Requires authentication (demonstrates UNAUTHENTICATED)
  rpc GetSecretData (SecretRequest) returns (SecretData);
}

message ProductRequest {
  string id = 1;
}

message Product {
  string id = 1;
  string name = 2;
  double price = 3;
}
```

## Implementation Examples

### Error Handling

**Server:**
```csharp
public override Task<Product> GetProduct(ProductRequest request, ServerCallContext context)
{
    var product = _repository.FindById(request.Id);
    
    if (product == null)
    {
        throw new RpcException(new Status(
            StatusCode.NotFound, 
            $"Product with ID {request.Id} not found"));
    }
    
    return Task.FromResult(product);
}
```

**Client:**
```csharp
try
{
    var product = await client.GetProductAsync(new ProductRequest { Id = "123" });
}
catch (RpcException ex) when (ex.StatusCode == StatusCode.NotFound)
{
    Console.WriteLine($"Product not found: {ex.Status.Detail}");
}
catch (RpcException ex)
{
    Console.WriteLine($"RPC Error: {ex.StatusCode} - {ex.Status.Detail}");
}
```

### Metadata

**Server - Reading Metadata:**
```csharp
public override Task<Product> GetProduct(ProductRequest request, ServerCallContext context)
{
    // Read metadata from client
    var requestId = context.RequestHeaders.GetValue("request-id");
    var userAgent = context.RequestHeaders.GetValue("user-agent");
    
    _logger.LogInformation($"Request ID: {requestId}, User Agent: {userAgent}");
    
    // Set response metadata
    context.ResponseTrailers.Add("server-time", DateTime.UtcNow.ToString());
    
    return Task.FromResult(new Product());
}
```

**Client - Sending Metadata:**
```csharp
var metadata = new Metadata
{
    { "request-id", Guid.NewGuid().ToString() },
    { "user-agent", "GrpcClient/1.0" },
    { "authorization", "Bearer token123" }
};

var call = client.GetProductAsync(new ProductRequest { Id = "123" }, metadata);
var response = await call.ResponseAsync;

// Read response trailers
var trailers = call.GetTrailers();
var serverTime = trailers.GetValue("server-time");
```

### Interceptors

**Server Interceptor:**
```csharp
public class LoggingInterceptor : Interceptor
{
    private readonly ILogger<LoggingInterceptor> _logger;

    public LoggingInterceptor(ILogger<LoggingInterceptor> logger)
    {
        _logger = logger;
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        var stopwatch = Stopwatch.StartNew();
        _logger.LogInformation($"Starting {context.Method}");
        
        try
        {
            var response = await continuation(request, context);
            _logger.LogInformation($"Completed {context.Method} in {stopwatch.ElapsedMilliseconds}ms");
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in {context.Method}");
            throw;
        }
    }
}

// Register in Program.cs
builder.Services.AddGrpc(options =>
{
    options.Interceptors.Add<LoggingInterceptor>();
});
```

**Client Interceptor:**
```csharp
public class AuthInterceptor : Interceptor
{
    private readonly string _token;

    public AuthInterceptor(string token)
    {
        _token = token;
    }

    public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
        TRequest request,
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
    {
        // Add authorization header
        var metadata = context.Options.Headers ?? new Metadata();
        metadata.Add("authorization", $"Bearer {_token}");
        
        var options = context.Options.WithHeaders(metadata);
        var newContext = new ClientInterceptorContext<TRequest, TResponse>(
            context.Method, context.Host, options);
        
        return continuation(request, newContext);
    }
}

// Use with client
var channel = GrpcChannel.ForAddress("https://localhost:7000");
var invoker = channel.Intercept(new AuthInterceptor("your-token"));
var client = new ProductService.ProductServiceClient(invoker);
```

### Deadlines and Timeouts

**Client with Deadline:**
```csharp
var deadline = DateTime.UtcNow.AddSeconds(5);
var call = client.ProcessOrderAsync(
    new OrderRequest(), 
    deadline: deadline);

try
{
    var response = await call;
}
catch (RpcException ex) when (ex.StatusCode == StatusCode.DeadlineExceeded)
{
    Console.WriteLine("Operation timed out");
}
```

**Server Checking Deadline:**
```csharp
public override async Task<OrderResult> ProcessOrder(
    OrderRequest request, 
    ServerCallContext context)
{
    // Long-running operation
    for (int i = 0; i < 10; i++)
    {
        // Check if deadline exceeded or client cancelled
        if (context.CancellationToken.IsCancellationRequested)
        {
            _logger.LogWarning("Client cancelled or deadline exceeded");
            throw new RpcException(new Status(StatusCode.Cancelled, "Operation cancelled"));
        }
        
        await Task.Delay(1000);
        // Process work...
    }
    
    return new OrderResult { Success = true };
}
```

## Running the Lab

1. **Start the Server**:
   ```bash
   cd GrpcAdvancedServer
   dotnet run
   ```

2. **Run the Client Examples**:
   ```bash
   cd GrpcAdvancedClient
   dotnet run
   ```

## Exercises

1. **Error Handling**: Create a service that validates complex business rules and returns appropriate error codes
2. **Metadata**: Implement request tracing by propagating a correlation ID through metadata
3. **Interceptors**: Create a rate-limiting interceptor that tracks requests per client
4. **Deadlines**: Implement a retry mechanism that respects deadlines
5. **Authentication**: Implement JWT token validation in a server interceptor

## Best Practices

### 1. Always Handle Errors Gracefully
```csharp
try
{
    return await CallService();
}
catch (RpcException ex) when (ex.StatusCode == StatusCode.Unavailable)
{
    // Maybe retry
    await Task.Delay(1000);
    return await CallService();
}
```

### 2. Use Structured Metadata
```csharp
// Good: Consistent naming
metadata.Add("x-request-id", id);
metadata.Add("x-client-version", version);

// Avoid: Inconsistent naming
metadata.Add("RequestId", id);
metadata.Add("client_v", version);
```

### 3. Set Appropriate Timeouts
```csharp
// Short operations
var quickCall = client.GetDataAsync(request, deadline: DateTime.UtcNow.AddSeconds(2));

// Long operations
var longCall = client.ProcessBatchAsync(request, deadline: DateTime.UtcNow.AddMinutes(5));
```

## Expected Output

```
=== Error Handling Demo ===
Attempting to get non-existent product...
Error: NOT_FOUND - Product with ID 999 not found

=== Metadata Demo ===
Request ID: 123e4567-e89b-12d3-a456-426614174000
Server processed at: 2025-10-12 10:30:00

=== Interceptor Demo ===
[Interceptor] Starting /ProductService/GetProduct
[Interceptor] Completed /ProductService/GetProduct in 45ms

=== Deadline Demo ===
Starting long operation with 3s deadline...
Error: DEADLINE_EXCEEDED - Operation timed out
```

## Next Steps

Lab 4 covers production deployment:
- Integration with ASP.NET Core
- Health checks
- Load balancing
- Monitoring and observability
- Docker deployment
