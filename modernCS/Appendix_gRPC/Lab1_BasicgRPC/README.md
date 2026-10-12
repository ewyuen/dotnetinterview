# Lab 1: Basic gRPC Service

## Objectives
- Understand Protocol Buffers and .proto files
- Create a simple unary gRPC service
- Implement a gRPC server
- Build a gRPC client
- Test the service end-to-end

## Concepts

### Protocol Buffers (.proto)
Protocol Buffers are Google's language-neutral, platform-neutral mechanism for serializing structured data. They're used to define:
- **Messages**: Data structures
- **Services**: RPC methods

### Unary RPC
The simplest type of RPC where the client sends a single request and receives a single response.

```
Client ----(Request)----> Server
Client <---(Response)---- Server
```

## Project Structure

This lab includes two projects:
1. **GrpcGreeterServer**: The gRPC server
2. **GrpcGreeterClient**: The client application

## The Proto File

```protobuf
syntax = "proto3";

option csharp_namespace = "GrpcGreeter";

service Greeter {
  rpc SayHello (HelloRequest) returns (HelloReply);
  rpc SayHelloAgain (HelloRequest) returns (HelloReply);
}

message HelloRequest {
  string name = 1;
}

message HelloReply {
  string message = 1;
}
```

### Breaking Down the Proto File:
- `syntax = "proto3"`: Uses Protocol Buffers version 3
- `option csharp_namespace`: Defines the C# namespace for generated code
- `service Greeter`: Defines the service with RPC methods
- `message`: Defines the data structures
- Field numbers (1, 2, etc.): Used for serialization (never change these!)

## Implementation Steps

### 1. Create the Server Project
```bash
dotnet new grpc -n GrpcGreeterServer
```

### 2. Define the Proto File
Edit `Protos/greet.proto` with your service definition.

### 3. Implement the Service
Create a service class that inherits from the generated base class.

### 4. Configure the Server
Set up the gRPC service in `Program.cs`.

### 5. Create the Client Project
```bash
dotnet new console -n GrpcGreeterClient
```

### 6. Add gRPC Client Packages
Add necessary NuGet packages to the client.

### 7. Reference the Proto File
Link the same .proto file in the client project.

### 8. Call the Service
Create a channel and make RPC calls.

## Key Concepts

### Channel
A connection to a gRPC server. Channels are expensive to create, so reuse them when possible.

### Stub (Client)
The generated client class used to make RPC calls.

### Service Implementation
The server-side implementation of the service defined in the .proto file.

## Running the Lab

1. **Start the Server**:
   ```bash
   cd GrpcGreeterServer
   dotnet run
   ```
   Server starts on https://localhost:7000 and http://localhost:5000

2. **Run the Client**:
   ```bash
   cd GrpcGreeterClient
   dotnet run
   ```

## Exercises

1. Add a new RPC method `GetGreetingCount` that returns how many times the service has been called
2. Modify the `HelloRequest` to include a `greeting_type` field (formal, casual, etc.)
3. Implement error handling for empty names
4. Add logging to both client and server

## Expected Output

```
Server listening on port 7000...

Client output:
Greeting: Hello, World!
Greeting again: Hello again, World!
```

## Common Issues

### Issue: Cannot connect to server
- Ensure the server is running
- Check firewall settings
- Verify the port number matches

### Issue: Proto file not found
- Check the .csproj file for correct Protobuf references
- Ensure the proto file is in the Protos folder

## Next Steps

In Lab 2, we'll explore streaming operations:
- Server streaming for sending multiple responses
- Client streaming for sending multiple requests
- Bidirectional streaming for real-time communication
