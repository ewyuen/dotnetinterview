# gRPC Quick Reference

## Proto File Syntax

### Basic Structure
```protobuf
syntax = "proto3";

option csharp_namespace = "MyApp.Grpc";

package myapp;

service MyService {
  rpc MyMethod (MyRequest) returns (MyResponse);
}

message MyRequest {
  string field1 = 1;
  int32 field2 = 2;
}
```

### Field Types

| Proto Type | C# Type | Notes |
|------------|---------|-------|
| `double` | `double` | |
| `float` | `float` | |
| `int32` | `int` | Variable-length encoding |
| `int64` | `long` | Variable-length encoding |
| `uint32` | `uint` | Variable-length encoding |
| `uint64` | `ulong` | Variable-length encoding |
| `sint32` | `int` | Signed, variable-length |
| `sint64` | `long` | Signed, variable-length |
| `fixed32` | `uint` | Fixed 4 bytes |
| `fixed64` | `ulong` | Fixed 8 bytes |
| `bool` | `bool` | |
| `string` | `string` | UTF-8 or 7-bit ASCII |
| `bytes` | `ByteString` | Arbitrary byte sequence |

### RPC Patterns

```protobuf
// Unary
rpc GetUser (UserRequest) returns (User);

// Server Streaming
rpc ListUsers (ListRequest) returns (stream User);

// Client Streaming
rpc CreateUsers (stream User) returns (CreateResponse);

// Bidirectional Streaming
rpc Chat (stream ChatMessage) returns (stream ChatMessage);
```

### Collections
```protobuf
message UserList {
  repeated User users = 1;
}

message UserMap {
  map<string, User> users_by_id = 1;
}
```

### Enums
```protobuf
enum Status {
  UNKNOWN = 0;  // First value must be 0
  ACTIVE = 1;
  INACTIVE = 2;
}
```

### Oneof
```protobuf
message Response {
  oneof result {
    string error = 1;
    Data data = 2;
  }
}
```

## C# Implementation

### Server

**Service Implementation:**
```csharp
public class MyServiceImpl : MyService.MyServiceBase
{
    public override Task<MyResponse> MyMethod(MyRequest request, ServerCallContext context)
    {
        return Task.FromResult(new MyResponse { /* ... */ });
    }
}
```

**Program.cs:**
```csharp
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddGrpc();

var app = builder.Build();
app.MapGrpcService<MyServiceImpl>();
app.Run();
```

### Client

**Basic Call:**
```csharp
var channel = GrpcChannel.ForAddress("https://localhost:7000");
var client = new MyService.MyServiceClient(channel);

var response = await client.MyMethodAsync(new MyRequest { /* ... */ });
```

**With Options:**
```csharp
var metadata = new Metadata
{
    { "authorization", "Bearer token" }
};

var deadline = DateTime.UtcNow.AddSeconds(5);

var response = await client.MyMethodAsync(
    new MyRequest(),
    headers: metadata,
    deadline: deadline
);
```

## Streaming

### Server Streaming
```csharp
// Server
public override async Task ListItems(
    Request request,
    IServerStreamWriter<Item> responseStream,
    ServerCallContext context)
{
    foreach (var item in GetItems())
    {
        await responseStream.WriteAsync(item);
    }
}

// Client
var call = client.ListItems(new Request());
await foreach (var item in call.ResponseStream.ReadAllAsync())
{
    Console.WriteLine(item);
}
```

### Client Streaming
```csharp
// Server
public override async Task<Summary> UploadItems(
    IAsyncStreamReader<Item> requestStream,
    ServerCallContext context)
{
    await foreach (var item in requestStream.ReadAllAsync())
    {
        ProcessItem(item);
    }
    return new Summary();
}

// Client
var call = client.UploadItems();
foreach (var item in items)
{
    await call.RequestStream.WriteAsync(item);
}
await call.RequestStream.CompleteAsync();
var summary = await call.ResponseAsync;
```

### Bidirectional Streaming
```csharp
// Server
public override async Task Chat(
    IAsyncStreamReader<ChatMessage> requestStream,
    IServerStreamWriter<ChatMessage> responseStream,
    ServerCallContext context)
{
    await foreach (var message in requestStream.ReadAllAsync())
    {
        await responseStream.WriteAsync(ProcessMessage(message));
    }
}

// Client
var call = client.Chat();

// Reading task
var readTask = Task.Run(async () =>
{
    await foreach (var response in call.ResponseStream.ReadAllAsync())
    {
        Console.WriteLine(response);
    }
});

// Writing
foreach (var message in messages)
{
    await call.RequestStream.WriteAsync(message);
}
await call.RequestStream.CompleteAsync();

await readTask;
```

## Error Handling

### Status Codes
```csharp
// Server
throw new RpcException(new Status(
    StatusCode.NotFound,
    "Resource not found"
));

// Client
try
{
    var response = await client.MyMethodAsync(request);
}
catch (RpcException ex) when (ex.StatusCode == StatusCode.NotFound)
{
    // Handle not found
}
catch (RpcException ex) when (ex.StatusCode == StatusCode.DeadlineExceeded)
{
    // Handle timeout
}
```

### Custom Error Details
```csharp
// Server
var status = new Status(StatusCode.InvalidArgument, "Invalid data");
var trailers = new Metadata
{
    { "error-code", "INVALID_EMAIL" },
    { "field", "email" }
};
throw new RpcException(status, trailers);

// Client
catch (RpcException ex)
{
    var errorCode = ex.Trailers.GetValue("error-code");
    var field = ex.Trailers.GetValue("field");
}
```

## Interceptors

### Server Interceptor
```csharp
public class LoggingInterceptor : Interceptor
{
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        Console.WriteLine($"Starting {context.Method}");
        var response = await continuation(request, context);
        Console.WriteLine($"Completed {context.Method}");
        return response;
    }
}

// Register
builder.Services.AddGrpc(options =>
{
    options.Interceptors.Add<LoggingInterceptor>();
});
```

### Client Interceptor
```csharp
public class AuthInterceptor : Interceptor
{
    public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
        TRequest request,
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
    {
        var metadata = context.Options.Headers ?? new Metadata();
        metadata.Add("authorization", "Bearer token");
        
        var options = context.Options.WithHeaders(metadata);
        var newContext = new ClientInterceptorContext<TRequest, TResponse>(
            context.Method, context.Host, options);
        
        return continuation(request, newContext);
    }
}

// Use
var invoker = channel.Intercept(new AuthInterceptor());
var client = new MyService.MyServiceClient(invoker);
```

## Common Patterns

### Retry Logic
```csharp
builder.Services.AddGrpcClient<MyService.MyServiceClient>(o =>
{
    o.Address = new Uri("https://service");
})
.AddPolicyHandler(Policy
    .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
    .WaitAndRetryAsync(3, retryAttempt => 
        TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));
```

### Circuit Breaker
```csharp
.AddPolicyHandler(Policy<HttpResponseMessage>
    .Handle<HttpRequestException>()
    .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));
```

### Timeout
```csharp
.AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(
    TimeSpan.FromSeconds(10)));
```

## Project Configuration

### .csproj for Server
```xml
<ItemGroup>
  <PackageReference Include="Grpc.AspNetCore" Version="2.71.0" />
</ItemGroup>

<ItemGroup>
  <Protobuf Include="Protos\*.proto" GrpcServices="Server" />
</ItemGroup>
```

### .csproj for Client
```xml
<ItemGroup>
  <PackageReference Include="Grpc.Net.Client" Version="2.71.0" />
  <PackageReference Include="Google.Protobuf" Version="3.32.1" />
  <PackageReference Include="Grpc.Tools" Version="2.72.0" PrivateAssets="All" />
</ItemGroup>

<ItemGroup>
  <Protobuf Include="Protos\*.proto" GrpcServices="Client" />
</ItemGroup>
```

## Useful Commands

```bash
# Create gRPC server project
dotnet new grpc -n MyGrpcService

# Create console client
dotnet new console -n MyGrpcClient

# Add gRPC client packages
dotnet add package Grpc.Net.Client
dotnet add package Google.Protobuf
dotnet add package Grpc.Tools

# Build (generates code from .proto files)
dotnet build

# Run
dotnet run
```

## Performance Tips

1. **Reuse channels** - Channels are expensive to create
2. **Use streaming** for large data transfers
3. **Enable HTTP/2 connection multiplexing**
4. **Use appropriate message sizes**
5. **Implement connection pooling**
6. **Use binary serialization** (default with protobuf)
7. **Consider compression** for large messages

## Security Checklist

- [ ] Use TLS/HTTPS in production
- [ ] Implement authentication (JWT, mTLS)
- [ ] Add authorization checks
- [ ] Validate all input
- [ ] Set appropriate timeouts
- [ ] Rate limit requests
- [ ] Log security events
- [ ] Keep dependencies updated

## Common Issues

### Issue: Could not connect to server
- Check if server is running
- Verify port number
- Check firewall settings
- Ensure TLS certificates are valid

### Issue: Deadline exceeded
- Increase timeout
- Optimize server processing
- Check network latency

### Issue: Message too large
- Increase `MaxReceiveMessageSize`
- Use streaming instead of large messages
- Compress data

## Resources

- [gRPC.io](https://grpc.io/)
- [gRPC for .NET](https://docs.microsoft.com/en-us/aspnet/core/grpc/)
- [Protocol Buffers](https://developers.google.com/protocol-buffers)
- [gRPC Status Codes](https://grpc.github.io/grpc/core/md_doc_statuscodes.html)
