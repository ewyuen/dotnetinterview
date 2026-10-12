# Lab 4: Real-time Subscriptions

## Objectives
- Implement GraphQL subscriptions
- Set up WebSocket connections
- Broadcast real-time updates
- Handle subscription lifecycle
- Build real-time features

## Coming Soon

This lab will cover:

### 1. Basic Subscription
```graphql
subscription {
  bookAdded {
    id
    title
  }
}
```

### 2. Filtered Subscriptions
```graphql
subscription {
  bookUpdated(id: "1") {
    title
    publishedYear
  }
}
```

### 3. Implementation
```csharp
public class Subscription
{
    [Subscribe]
    [Topic]
    public Book BookAdded([EventMessage] Book book)
    {
        return book;
    }
}
```

### 4. Publishing Events
```csharp
public class Mutation
{
    public async Task<Book> AddBook(
        AddBookInput input,
        [Service] ITopicEventSender eventSender)
    {
        var book = CreateBook(input);
        await eventSender.SendAsync("BookAdded", book);
        return book;
    }
}
```

### 5. WebSocket Configuration
- Setting up WebSocket transport
- Connection management
- Authentication for subscriptions

Stay tuned for the complete implementation!
