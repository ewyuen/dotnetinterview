# Chapter 5: ASP.NET Core Web Development

## Overview
This chapter introduces web development with ASP.NET Core, covering RESTful APIs, dependency injection, Entity Framework Core, and building scalable web applications.

## Learning Objectives
- Build RESTful web APIs with ASP.NET Core
- Implement dependency injection patterns
- Work with Entity Framework Core for data access
- Design scalable web application architectures
- Handle HTTP requests and responses properly
- Implement proper error handling and logging

## Key Concepts

### 1. ASP.NET Core Fundamentals
- **WebHost and Generic Host**: Application hosting models
- **Middleware Pipeline**: Request processing pipeline
- **Dependency Injection**: Built-in DI container
- **Configuration**: App settings and environment management

### 2. RESTful API Design
- **Controllers**: Handling HTTP requests and responses
- **Routing**: URL pattern matching and parameter binding
- **Model Binding**: Converting HTTP data to .NET objects
- **Action Results**: Returning appropriate HTTP responses

### 3. Dependency Injection
- **Service Registration**: Configuring services in DI container
- **Service Lifetimes**: Transient, Scoped, and Singleton
- **Constructor Injection**: Injecting dependencies into classes
- **Service Locator Pattern**: Alternative to constructor injection

### 4. Entity Framework Core
- **DbContext**: Database session and unit of work
- **Entity Models**: Domain object representations
- **Migrations**: Database schema versioning
- **LINQ Queries**: Type-safe database queries

### 5. Data Access Patterns
- **Repository Pattern**: Abstracting data access logic
- **Unit of Work**: Managing transactions across repositories
- **CQRS**: Command Query Responsibility Segregation
- **Specification Pattern**: Encapsulating query logic

## Important Learning Points

### ASP.NET Core Architecture
- **Request Pipeline**: How requests flow through middleware
- **Service Collection**: Configuring application services
- **Options Pattern**: Strongly-typed configuration
- **Hosting Environment**: Development vs Production differences

### REST API Best Practices
- **HTTP Methods**: GET, POST, PUT, DELETE semantics
- **Status Codes**: Appropriate HTTP status code usage
- **Content Negotiation**: JSON, XML response formats
- **API Versioning**: Managing API evolution

### Dependency Injection Benefits
- **Testability**: Easy mocking and unit testing
- **Maintainability**: Loose coupling between components
- **Flexibility**: Runtime service configuration
- **Lifetime Management**: Automatic resource cleanup

### Entity Framework Core Patterns
- **Code First**: Define model in code, generate database
- **Migrations**: Incremental database schema changes
- **Change Tracking**: Automatic entity state management
- **Lazy vs Eager Loading**: Query optimization strategies

## Labs Overview

### Lab 5.1: Basic REST API
- Create ASP.NET Core Web API project
- Implement CRUD operations for a resource
- Configure routing and model binding
- Return appropriate HTTP responses

### Lab 5.2: Dependency Injection
- Register services with different lifetimes
- Implement constructor injection
- Create custom services and interfaces
- Handle service dependencies properly

### Lab 5.3: Entity Framework Core
- Set up EF Core with SQL Server
- Create entity models and DbContext
- Implement database migrations
- Perform CRUD operations with EF Core

### Lab 5.4: Final Component - Complete API
- Combine all concepts into a full-featured API
- Implement repository and unit of work patterns
- Add proper error handling and validation
- Include API documentation and testing

## References and Further Reading

### Official Microsoft Documentation
- [ASP.NET Core Documentation](https://docs.microsoft.com/en-us/aspnet/core/)
- [Web API Development](https://docs.microsoft.com/en-us/aspnet/core/web-api/)
- [Dependency Injection](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)

### Books and Resources
- "ASP.NET Core 6 and Angular" by Valerio De Sanctis
- "Pro ASP.NET Core 6" by Adam Freeman
- "Entity Framework Core in Action" by Jon P Smith
- [ASP.NET Core Architecture](https://docs.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/)

### Design Patterns
- [Repository Pattern](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/infrastructure-persistence-layer-design)
- [Unit of Work Pattern](https://docs.microsoft.com/en-us/aspnet/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application)
- [CQRS Pattern](https://docs.microsoft.com/en-us/azure/architecture/patterns/cqrs)
- [Mediator Pattern](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/microservice-application-layer-implementation-web-api)

## Best Practices

### API Design
- Use RESTful conventions for resource naming
- Implement proper HTTP status codes
- Include meaningful error messages
- Version your APIs appropriately
- Document APIs with OpenAPI/Swagger

### Dependency Injection
- Register services at application startup
- Use interfaces for service contracts
- Choose appropriate service lifetimes
- Avoid service locator anti-pattern
- Test with dependency injection in mind

### Entity Framework
- Use async methods for database operations
- Implement proper transaction management
- Avoid N+1 query problems
- Use navigation properties appropriately
- Handle concurrency conflicts

### Security Considerations
- Validate all input data
- Implement authentication and authorization
- Use HTTPS for production deployments
- Protect against common web vulnerabilities
- Implement proper CORS policies

## Common Pitfalls to Avoid

### DI Anti-Patterns
- Captive Dependencies: Wrong service lifetime combinations
- Service Locator: Injecting DI container directly
- Circular Dependencies: Services depending on each other
- Over-Injection: Too many constructor parameters

### EF Core Mistakes
- Not using async methods for I/O operations
- Loading too much data with eager loading
- Not handling database connection issues
- Ignoring migration scripts in production
- Poorly designed entity relationships

### API Design Issues
- Inconsistent resource naming
- Missing error handling
- Not using appropriate HTTP methods
- Tight coupling between controllers and data access
- Ignoring content negotiation

### Performance Problems
- Blocking calls in async methods
- Not using connection pooling
- Loading unnecessary data
- Missing database indexes
- Inefficient LINQ queries

## Testing Web APIs
- Unit test controllers with mocked dependencies
- Integration test with in-memory database
- Test API endpoints with HTTP client
- Verify error responses and status codes
- Test authentication and authorization

## Deployment Considerations
- Environment-specific configuration
- Database migration strategies
- Health checks and monitoring
- Logging configuration
- Container deployment with Docker

## Advanced Topics
- **Minimal APIs**: Simplified API creation in .NET 6+
- **gRPC Services**: High-performance RPC framework
- **SignalR**: Real-time web functionality
- **Blazor**: Web UI framework with C#
- **Microservices**: Distributed system architecture

## Architecture Patterns
- **Clean Architecture**: Separation of concerns
- **CQRS**: Separate read and write models
- **Event Sourcing**: Storing state as events
- **API Gateway**: Single entry point for microservices
- **Circuit Breaker**: Fault tolerance pattern

## Monitoring and Observability
- Application Insights for telemetry
- Structured logging with Serilog
- Health checks for service monitoring
- Distributed tracing with OpenTelemetry
- Performance monitoring and alerting