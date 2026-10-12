# Getting Started with gRPC Appendix

Welcome to the gRPC learning appendix! This guide will help you get started quickly.

## What You'll Learn

This appendix covers gRPC from basics to production deployment:

1. **Lab 1**: Basic gRPC service (unary RPC)
2. **Lab 2**: Streaming operations (server, client, bidirectional)
3. **Lab 3**: Advanced features (error handling, metadata, interceptors)
4. **Lab 4**: Production deployment (health checks, monitoring, Docker)

## Prerequisites

Before starting, ensure you have:
- ✅ .NET 6.0 SDK or later installed
- ✅ Visual Studio 2022 or VS Code
- ✅ Basic C# knowledge
- ✅ Understanding of async/await

Check your .NET version:
```bash
dotnet --version
```

## Quick Start (5 minutes)

### 1. Navigate to Lab 1
```bash
cd Appendix_gRPC/Lab1_BasicgRPC
```

### 2. Start the Server
```bash
cd GrpcGreeterServer
dotnet run
```

You should see:
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:7000
```

### 3. Run the Client (in a new terminal)
```bash
cd GrpcGreeterClient
dotnet run
```

You should see:
```
gRPC Client Starting...

=== Example 1: Basic SayHello ===
Greeting: Hello World

=== Example 2: SayHelloAgain ===
Greeting Again: Hello again, World!

=== Example 3: Get Greeting Count ===
Total greetings sent: 2
```

🎉 **Success!** You've just run your first gRPC service!

## Understanding the Code

### The Proto File (greet.proto)
This defines the contract between client and server:

```protobuf
service Greeter {
  rpc SayHello (HelloRequest) returns (HelloReply);
}

message HelloRequest {
  string name = 1;
}

message HelloReply {
  string message = 1;
}
```

**Think of it as:**
- `service` = interface
- `rpc` = method
- `message` = class/struct

### The Server (GreeterService.cs)
Implements the service defined in the proto file:

```csharp
public class GreeterService : Greeter.GreeterBase
{
    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        return Task.FromResult(new HelloReply
        {
            Message = "Hello " + request.Name
        });
    }
}
```

### The Client (Program.cs)
Calls the server:

```csharp
var channel = GrpcChannel.ForAddress("https://localhost:7000");
var client = new Greeter.GreeterClient(channel);

var response = await client.SayHelloAsync(new HelloRequest { Name = "World" });
Console.WriteLine($"Greeting: {response.Message}");
```

## How gRPC Works

```
1. Define service in .proto file
   ↓
2. Build project → Code is generated
   ↓
3. Server implements generated base class
   ↓
4. Client uses generated client class
   ↓
5. Communication happens over HTTP/2
```

## Key Concepts

### 1. Protocol Buffers
- Binary serialization format
- Much faster than JSON
- Strongly typed
- Backward compatible

### 2. HTTP/2
- Multiplexing: Multiple requests over one connection
- Binary protocol: More efficient than HTTP/1.1
- Server push: Server can send data without request
- Header compression: Reduces overhead

### 3. Generated Code
When you build, the .proto file generates:
- **Server**: Base class to implement
- **Client**: Client class to call methods
- **Messages**: C# classes for data

## Learning Path Recommendation

### Week 1: Basics
- ✅ Complete Lab 1
- ✅ Read Lab 1 README thoroughly
- ✅ Experiment with adding new RPC methods
- ✅ Try error handling exercises

### Week 2: Streaming
- ✅ Complete Lab 2
- ✅ Understand when to use each streaming type
- ✅ Build a simple chat application
- ✅ Try file upload/download exercises

### Week 3: Advanced
- ✅ Complete Lab 3
- ✅ Implement logging interceptor
- ✅ Add authentication
- ✅ Practice with deadlines and timeouts

### Week 4: Production
- ✅ Complete Lab 4
- ✅ Set up Docker deployment
- ✅ Implement health checks
- ✅ Add monitoring with OpenTelemetry

## Common Questions

### Q: When should I use gRPC instead of REST?
**Use gRPC when:**
- Performance is critical
- You need streaming
- Working in microservices (internal communication)
- Strong typing is important

**Use REST when:**
- Browser clients (without gRPC-Web)
- Public APIs
- Simple CRUD operations
- Legacy system integration

### Q: Can I use both REST and gRPC in the same app?
**Yes!** ASP.NET Core supports both:

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();           // gRPC
builder.Services.AddControllers();     // REST

var app = builder.Build();

app.MapGrpcService<MyGrpcService>();  // gRPC endpoint
app.MapControllers();                  // REST endpoints
```

### Q: Is gRPC secure?
**Yes, when configured properly:**
- Use TLS/HTTPS
- Implement authentication (JWT, mTLS)
- Add authorization
- Validate all inputs

### Q: What about browser support?
**Options:**
1. **gRPC-Web**: Proxy between browser and gRPC server
2. **SignalR**: Microsoft's real-time framework
3. **REST gateway**: Expose REST API that calls gRPC internally

## Project Structure Overview

```
Appendix_gRPC/
├── README.md                    # Main overview
├── GETTING_STARTED.md          # This file
├── QUICK_REFERENCE.md          # Cheat sheet
│
├── Lab1_BasicgRPC/             # Start here!
│   ├── README.md
│   ├── GrpcGreeterServer/      # Server implementation
│   └── GrpcGreeterClient/      # Client implementation
│
├── Lab2_Streaming/             # Streaming patterns
│   └── README.md
│
├── Lab3_AdvancedFeatures/      # Error handling, interceptors
│   └── README.md
│
└── Lab4_Production/            # Production deployment
    └── README.md
```

## Troubleshooting

### Problem: "Could not connect to server"
**Solution:**
1. Ensure server is running
2. Check port number (should be 7000 for HTTPS)
3. Verify firewall isn't blocking

### Problem: "Proto file not found"
**Solution:**
1. Rebuild project: `dotnet build`
2. Check .csproj has `<Protobuf Include="...">` entry
3. Verify proto file path

### Problem: "SSL certificate error"
**Solution:**
For development:
```csharp
var handler = new HttpClientHandler();
handler.ServerCertificateCustomValidationCallback = 
    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

var channel = GrpcChannel.ForAddress("https://localhost:7000", 
    new GrpcChannelOptions { HttpHandler = handler });
```

**Note:** Never use this in production!

## Next Steps

1. **Start with Lab 1**: Get hands-on experience
2. **Read the READMEs**: Each lab has detailed explanations
3. **Do the exercises**: Learning by doing is key
4. **Check QUICK_REFERENCE.md**: For syntax and patterns
5. **Build something**: Small project to apply knowledge

## Resources

### Official Documentation
- [gRPC.io](https://grpc.io/)
- [gRPC for .NET](https://docs.microsoft.com/en-us/aspnet/core/grpc/)
- [Protocol Buffers Guide](https://developers.google.com/protocol-buffers/docs/proto3)

### Video Tutorials
- Search YouTube for "gRPC .NET tutorial"
- Microsoft Learn modules on gRPC

### Books
- "gRPC: Up and Running" by Kasun Indrasiri
- Look for online articles and blog posts

## Getting Help

If you get stuck:
1. Check the lab's README
2. Review the QUICK_REFERENCE.md
3. Look at the working code in Lab 1
4. Search for the error message online
5. Check gRPC GitHub issues

## Happy Learning! 🚀

Remember:
- Start simple (Lab 1)
- Progress gradually
- Practice regularly
- Build projects
- Have fun!

---

**Ready to begin?** Head to [Lab 1: Basic gRPC](Lab1_BasicgRPC/README.md) and start coding!
