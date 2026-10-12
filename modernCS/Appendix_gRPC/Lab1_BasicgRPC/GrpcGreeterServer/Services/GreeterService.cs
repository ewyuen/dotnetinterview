using Grpc.Core;
using GrpcGreeter;

namespace GrpcStreamingServer.Services;

public class GreeterService : Greeter.GreeterBase
{
    private readonly ILogger<GreeterService> _logger;
    private static int _greetingCount = 0;

    public GreeterService(ILogger<GreeterService> logger)
    {
        _logger = logger;
    }

    public override Task<HelloReply> SayHelloAgain(HelloRequest request, ServerCallContext context)
    {
        _logger.LogInformation($"Saying hello again to {request.Name}");
        
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Name cannot be empty"));
        }

        Interlocked.Increment(ref _greetingCount);
        
        return Task.FromResult(new HelloReply
        {
            Message = $"Hello again, {request.Name}!"
        });
    }

    public override Task<CountReply> GetGreetingCount(Empty request, ServerCallContext context)
    {
        _logger.LogInformation($"Returning greeting count: {_greetingCount}");
        
        return Task.FromResult(new CountReply
        {
            Count = _greetingCount
        });
    }
}
