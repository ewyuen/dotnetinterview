# Lab 4: Production-Ready gRPC

## Objectives
- Integrate gRPC with ASP.NET Core applications
- Implement health checks for service monitoring
- Configure load balancing strategies
- Add monitoring and observability
- Deploy gRPC services in containers
- Implement security best practices

## Concepts

### 1. ASP.NET Core Integration

gRPC services integrate seamlessly with ASP.NET Core, allowing you to:
- Host both gRPC and REST APIs in the same application
- Use existing ASP.NET Core middleware
- Leverage dependency injection
- Apply authentication and authorization

### 2. Health Checks

Health checks allow load balancers and orchestrators to determine if a service is ready to handle requests.

**Health Check Protocol:**
- Standard gRPC health checking protocol
- Per-service health status
- Overall application health

### 3. Load Balancing

gRPC supports various load balancing strategies:
- **Client-side**: Client distributes calls across multiple servers
- **Proxy-based**: External load balancer (nginx, Envoy)
- **Service mesh**: Istio, Linkerd

### 4. Observability

Production services need monitoring, logging, and tracing:
- **Logging**: Structured logs for debugging
- **Metrics**: Request counts, latencies, error rates
- **Tracing**: Distributed tracing across services

## Project Structure

```
Lab4_Production/
├── GrpcProductionService/      # Main gRPC service
│   ├── Services/
│   ├── Health/
│   └── Interceptors/
├── GrpcGateway/                # REST to gRPC gateway
└── docker-compose.yml          # Container orchestration
```

## Implementation

### Health Checks

**Install Package:**
```bash
dotnet add package Grpc.HealthCheck
dotnet add package Grpc.AspNetCore.HealthChecks
```

**Program.cs:**
```csharp
using Grpc.Health.V1;
using Grpc.HealthCheck;

var builder = WebApplication.CreateBuilder(args);

// Add health checks
builder.Services.AddGrpc();
builder.Services.AddGrpcHealthChecks()
    .AddCheck("sample_check", () => HealthCheckResult.Healthy());

var app = builder.Build();

// Map health check service
app.MapGrpcService<HealthServiceImpl>();
app.MapGrpcHealthChecksService();

app.Run();
```

**Custom Health Check:**
```csharp
public class DatabaseHealthCheck : IHealthCheck
{
    private readonly IDbConnection _connection;

    public DatabaseHealthCheck(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await _connection.OpenAsync(cancellationToken);
            return HealthCheckResult.Healthy("Database is accessible");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Database is not accessible", ex);
        }
    }
}
```

### Load Balancing

**Client-Side Load Balancing:**
```csharp
var services = new[]
{
    "https://server1.example.com",
    "https://server2.example.com",
    "https://server3.example.com"
};

var channels = services.Select(s => GrpcChannel.ForAddress(s)).ToArray();
var clients = channels.Select(c => new ProductService.ProductServiceClient(c)).ToArray();

// Round-robin
int index = 0;
var client = clients[index++ % clients.Length];
```

**DNS Load Balancing:**
```csharp
// Configure resolver
var handler = new SocketsHttpHandler
{
    EnableMultipleHttp2Connections = true
};

var channel = GrpcChannel.ForAddress("dns:///myservice.example.com", new GrpcChannelOptions
{
    HttpHandler = handler,
    ServiceConfig = new ServiceConfig
    {
        LoadBalancingConfigs = { new RoundRobinConfig() }
    }
});
```

### Monitoring and Logging

**Structured Logging:**
```csharp
public class MonitoringInterceptor : Interceptor
{
    private readonly ILogger<MonitoringInterceptor> _logger;
    private readonly Counter<long> _requestCounter;
    private readonly Histogram<double> _requestDuration;

    public MonitoringInterceptor(
        ILogger<MonitoringInterceptor> logger,
        IMeterFactory meterFactory)
    {
        _logger = logger;
        var meter = meterFactory.Create("GrpcService");
        
        _requestCounter = meter.CreateCounter<long>("grpc_requests_total");
        _requestDuration = meter.CreateHistogram<double>("grpc_request_duration_seconds");
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        var stopwatch = Stopwatch.StartNew();
        var method = context.Method;
        
        try
        {
            var response = await continuation(request, context);
            
            stopwatch.Stop();
            _requestCounter.Add(1, new KeyValuePair<string, object?>("method", method));
            _requestDuration.Record(stopwatch.Elapsed.TotalSeconds, 
                new KeyValuePair<string, object?>("method", method));
            
            _logger.LogInformation(
                "gRPC call {Method} completed in {Duration}ms",
                method, stopwatch.ElapsedMilliseconds);
            
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in gRPC call {Method}", method);
            throw;
        }
    }
}
```

**OpenTelemetry Integration:**
```csharp
builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing
        .AddAspNetCoreInstrumentation()
        .AddGrpcClientInstrumentation()
        .AddSource("GrpcService")
        .AddJaegerExporter())
    .WithMetrics(metrics => metrics
        .AddAspNetCoreInstrumentation()
        .AddMeter("GrpcService")
        .AddPrometheusExporter());
```

### Security

**TLS Configuration:**
```csharp
var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5000);  // HTTP
    options.ListenAnyIP(5001, listenOptions =>
    {
        listenOptions.UseHttps();  // HTTPS
    });
});
```

**JWT Authentication:**
```csharp
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = "your-issuer",
            ValidAudience = "your-audience",
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("your-secret-key"))
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
```

**Service Authorization:**
```csharp
[Authorize(Roles = "Admin")]
public class AdminService : Admin.AdminBase
{
    public override Task<Response> DeleteUser(DeleteRequest request, ServerCallContext context)
    {
        // Only accessible to Admin role
        return Task.FromResult(new Response());
    }
}
```

### Docker Deployment

**Dockerfile:**
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["GrpcProductionService.csproj", "./"]
RUN dotnet restore "GrpcProductionService.csproj"
COPY . .
RUN dotnet build "GrpcProductionService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GrpcProductionService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GrpcProductionService.dll"]
```

**docker-compose.yml:**
```yaml
version: '3.8'

services:
  grpc-service:
    build: ./GrpcProductionService
    ports:
      - "5000:80"
      - "5001:443"
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - ASPNETCORE_URLS=https://+:443;http://+:80
    healthcheck:
      test: ["CMD", "grpc_health_probe", "-addr=:80"]
      interval: 30s
      timeout: 10s
      retries: 3
    networks:
      - grpc-network

networks:
  grpc-network:
    driver: bridge
```

## Configuration

**appsettings.Production.json:**
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Grpc": "Debug"
    }
  },
  "Kestrel": {
    "Endpoints": {
      "Http": {
        "Url": "http://+:80"
      },
      "Https": {
        "Url": "https://+:443"
      },
      "gRPC": {
        "Url": "http://+:5000",
        "Protocols": "Http2"
      }
    }
  },
  "AllowedHosts": "*"
}
```

## Running the Lab

1. **Local Development**:
   ```bash
   cd GrpcProductionService
   dotnet run
   ```

2. **Docker**:
   ```bash
   docker-compose up -d
   ```

3. **Health Check**:
   ```bash
   grpc_health_probe -addr=localhost:5000
   ```

## Exercises

1. **Health Checks**: Implement custom health checks for database and external services
2. **Load Balancing**: Set up client-side load balancing across multiple service instances
3. **Monitoring**: Integrate with Prometheus and Grafana for metrics visualization
4. **Security**: Implement certificate-based authentication
5. **Gateway**: Create a REST API gateway that translates HTTP to gRPC calls

## Best Practices

### 1. Always Use TLS in Production
```csharp
// Never do this in production:
var channel = GrpcChannel.ForAddress("http://...", new GrpcChannelOptions
{
    HttpHandler = new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = 
            HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    }
});

// Do this instead:
var channel = GrpcChannel.ForAddress("https://...");
```

### 2. Configure Proper Timeouts
```csharp
builder.Services.AddGrpc(options =>
{
    options.MaxReceiveMessageSize = 4 * 1024 * 1024; // 4 MB
    options.MaxSendMessageSize = 4 * 1024 * 1024; // 4 MB
    options.EnableDetailedErrors = false; // Don't leak details in production
});
```

### 3. Implement Circuit Breakers
```csharp
builder.Services.AddGrpcClient<ProductService.ProductServiceClient>(o =>
{
    o.Address = new Uri("https://backend-service");
})
.AddPolicyHandler(Policy<HttpResponseMessage>
    .Handle<HttpRequestException>()
    .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));
```

## Monitoring Dashboard

Key metrics to monitor:
- **Request rate**: Requests per second
- **Error rate**: Failed requests percentage
- **Latency**: P50, P95, P99 percentiles
- **Active connections**: Current open connections
- **Resource usage**: CPU, memory, network

## Expected Output

```
=== Health Check ===
Service: grpc.health.v1.Health
Status: SERVING

=== Load Test Results ===
Total Requests: 10000
Successful: 9995
Failed: 5
Average Latency: 45ms
P95 Latency: 120ms
P99 Latency: 250ms

=== Metrics ===
grpc_requests_total{method="/ProductService/GetProduct"} 1543
grpc_request_duration_seconds{method="/ProductService/GetProduct",quantile="0.95"} 0.120
```

## Additional Resources

- [gRPC Performance Best Practices](https://grpc.io/docs/guides/performance/)
- [ASP.NET Core gRPC](https://docs.microsoft.com/en-us/aspnet/core/grpc/)
- [OpenTelemetry](https://opentelemetry.io/)
- [Envoy Proxy](https://www.envoyproxy.io/)

## Conclusion

You now have the knowledge to build production-ready gRPC services with:
- Proper error handling and resilience
- Health monitoring
- Security
- Observability
- Scalability

Consider exploring gRPC-Web for browser clients and gRPC reflection for dynamic service discovery!
