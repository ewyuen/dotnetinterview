# Lab 5: Production Features

## Objectives
- Integrate Entity Framework Core
- Implement authentication and authorization
- Add field-level permissions
- Error handling strategies
- Performance monitoring
- Deployment considerations

## Coming Soon

This lab will cover:

### 1. Entity Framework Core Integration
```csharp
builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .RegisterDbContext<AppDbContext>(DbContextKind.Pooled);
```

### 2. Authentication
```csharp
[Authorize]
public class Mutation
{
    public Book UpdateBook(string id, UpdateBookInput input)
    {
        // Only authenticated users can update
    }
}
```

### 3. Authorization
```csharp
[Authorize(Roles = new[] { "Admin" })]
public Book DeleteBook(string id)
{
    // Only admins can delete
}
```

### 4. Field-Level Security
```csharp
public class Book
{
    public string Title { get; set; }
    
    [Authorize(Policy = "CanViewRevenue")]
    public decimal Revenue { get; set; }
}
```

### 5. Error Handling
- Custom error filters
- Error codes and messages
- Client-friendly error responses

### 6. Performance
- Query complexity analysis
- Persisted queries
- Response caching
- APM integration

### 7. Deployment
- Docker containerization
- Environment configuration
- Health checks
- Monitoring and logging

Stay tuned for the complete implementation!
