# Lab 2: Mutations and Data Modification

## Objectives
- Implement GraphQL mutations for Create, Update, Delete operations
- Define input types for complex data structures
- Handle validation and error scenarios
- Return meaningful response types
- Follow GraphQL mutation best practices

## Coming Soon

This lab will cover:

### 1. Creating Data (Add)
- AddBook mutation
- AddAuthor mutation
- Input validation
- Returning created object

### 2. Updating Data
- UpdateBook mutation
- Partial updates with optional fields
- Input types for updates
- Optimistic concurrency

### 3. Deleting Data
- DeleteBook mutation
- Soft vs hard deletes
- Return types for delete operations
- Cascade delete considerations

### 4. Input Types
```csharp
public record AddBookInput(
    string Title,
    string AuthorId,
    int PublishedYear,
    string Genre,
    int Pages
);

public record UpdateBookInput(
    string? Title,
    string? AuthorId,
    int? PublishedYear,
    string? Genre,
    int? Pages
);
```

### 5. Mutation Responses
```csharp
public record AddBookPayload(
    Book? Book,
    string? Error
);
```

### 6. Validation
- Input validation
- Business rule validation
- Error handling and reporting

Stay tuned for the complete implementation!
