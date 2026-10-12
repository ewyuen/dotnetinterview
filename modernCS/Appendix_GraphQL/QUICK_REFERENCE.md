# GraphQL Quick Reference

## Query Syntax

### Basic Query
```graphql
query {
  books {
    id
    title
  }
}
```

### Named Query
```graphql
query GetBooks {
  books {
    id
    title
  }
}
```

### Query with Arguments
```graphql
query {
  book(id: "1") {
    title
    author {
      name
    }
  }
}
```

### Query with Variables
```graphql
query GetBook($bookId: ID!) {
  book(id: $bookId) {
    title
  }
}
```

Variables:
```json
{
  "bookId": "1"
}
```

### Multiple Queries
```graphql
query {
  books {
    title
  }
  authors {
    name
  }
}
```

### Aliases
```graphql
query {
  firstBook: book(id: "1") {
    title
  }
  secondBook: book(id: "2") {
    title
  }
}
```

### Fragments
```graphql
fragment BookDetails on Book {
  id
  title
  publishedYear
}

query {
  book(id: "1") {
    ...BookDetails
    author {
      name
    }
  }
}
```

## Mutations

### Basic Mutation
```graphql
mutation {
  addBook(input: {
    title: "New Book"
    authorId: "1"
  }) {
    id
    title
  }
}
```

### Mutation with Variables
```graphql
mutation AddBook($input: AddBookInput!) {
  addBook(input: $input) {
    id
    title
  }
}
```

Variables:
```json
{
  "input": {
    "title": "New Book",
    "authorId": "1"
  }
}
```

### Multiple Mutations
```graphql
mutation {
  addBook(input: { title: "Book 1" }) {
    id
  }
  addAuthor(input: { name: "Author 1" }) {
    id
  }
}
```

## Subscriptions

### Basic Subscription
```graphql
subscription {
  bookAdded {
    id
    title
  }
}
```

### Subscription with Filter
```graphql
subscription OnBookUpdated($bookId: ID!) {
  bookUpdated(id: $bookId) {
    id
    title
  }
}
```

## Schema Definition (C#)

### Object Type
```csharp
public class Book
{
    public string Id { get; set; }
    public string Title { get; set; }
    public int PublishedYear { get; set; }
    public Author Author { get; set; }
}
```

### Query Type
```csharp
public class Query
{
    public List<Book> GetBooks()
    {
        return _bookRepository.GetAll();
    }

    public Book? GetBook(string id)
    {
        return _bookRepository.GetById(id);
    }
}
```

### Mutation Type
```csharp
public class Mutation
{
    public Book AddBook(AddBookInput input)
    {
        var book = new Book
        {
            Id = Guid.NewGuid().ToString(),
            Title = input.Title
        };
        _bookRepository.Add(book);
        return book;
    }
}
```

### Subscription Type
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

## Input Types

### Basic Input
```csharp
public record AddBookInput(
    string Title,
    string AuthorId,
    int PublishedYear
);
```

### Nested Input
```csharp
public record AddBookWithAuthorInput(
    string Title,
    AuthorInput Author
);

public record AuthorInput(
    string Name,
    int BirthYear
);
```

### Optional Fields
```csharp
public record UpdateBookInput(
    string? Title,           // Optional
    string? AuthorId,        // Optional
    int? PublishedYear       // Optional
);
```

## Resolvers

### Field Resolver
```csharp
public class Book
{
    public string Id { get; set; }
    public string AuthorId { get; set; }
    
    // Resolver method
    public Author GetAuthor([Service] IAuthorRepository repo)
    {
        return repo.GetById(AuthorId);
    }
}
```

### Resolver Attribute
```csharp
public class BookResolvers
{
    public Author GetAuthor(
        [Parent] Book book,
        [Service] IAuthorRepository repo)
    {
        return repo.GetById(book.AuthorId);
    }
}
```

### Async Resolver
```csharp
public async Task<Author> GetAuthor(
    [Parent] Book book,
    [Service] IAuthorRepository repo,
    CancellationToken cancellationToken)
{
    return await repo.GetByIdAsync(book.AuthorId, cancellationToken);
}
```

## DataLoader (N+1 Solution)

### Group DataLoader
```csharp
public class AuthorByIdDataLoader : GroupedDataLoader<string, Author>
{
    private readonly IAuthorRepository _repository;

    public AuthorByIdDataLoader(
        IBatchScheduler batchScheduler,
        IAuthorRepository repository)
        : base(batchScheduler)
    {
        _repository = repository;
    }

    protected override async Task<ILookup<string, Author>> LoadGroupedBatch(
        IReadOnlyList<string> keys,
        CancellationToken cancellationToken)
    {
        var authors = await _repository.GetByIdsAsync(keys, cancellationToken);
        return authors.ToLookup(a => a.Id);
    }
}
```

### Using DataLoader
```csharp
public class Book
{
    public string AuthorId { get; set; }
    
    public async Task<Author> GetAuthor(
        AuthorByIdDataLoader dataLoader)
    {
        return await dataLoader.LoadAsync(AuthorId);
    }
}
```

## Filtering

### Basic Filter
```csharp
[UseFiltering]
public IQueryable<Book> GetBooks([Service] AppDbContext context)
{
    return context.Books;
}
```

Usage:
```graphql
query {
  books(where: { title: { eq: "GraphQL Guide" } }) {
    title
  }
}
```

### Multiple Conditions
```graphql
query {
  books(where: {
    and: [
      { publishedYear: { gte: 2000 } }
      { title: { contains: "GraphQL" } }
    ]
  }) {
    title
    publishedYear
  }
}
```

## Sorting

### Basic Sorting
```csharp
[UseSorting]
public IQueryable<Book> GetBooks([Service] AppDbContext context)
{
    return context.Books;
}
```

Usage:
```graphql
query {
  books(order: { title: ASC }) {
    title
  }
}
```

### Multiple Sort Fields
```graphql
query {
  books(order: [
    { publishedYear: DESC }
    { title: ASC }
  ]) {
    title
    publishedYear
  }
}
```

## Pagination

### Offset Pagination
```csharp
[UsePaging]
public IQueryable<Book> GetBooks([Service] AppDbContext context)
{
    return context.Books;
}
```

Usage:
```graphql
query {
  books(skip: 0, take: 10) {
    items {
      title
    }
    pageInfo {
      hasNextPage
      hasPreviousPage
    }
    totalCount
  }
}
```

### Cursor Pagination
```csharp
[UsePaging]
public IQueryable<Book> GetBooks([Service] AppDbContext context)
{
    return context.Books;
}
```

Usage:
```graphql
query {
  books(first: 10, after: "cursor") {
    edges {
      node {
        title
      }
      cursor
    }
    pageInfo {
      hasNextPage
      endCursor
    }
  }
}
```

## Error Handling

### Custom Error
```csharp
public class BookNotFoundError : Exception
{
    public BookNotFoundError(string bookId)
        : base($"Book with ID '{bookId}' not found")
    {
    }
}
```

### Error Filter
```csharp
public class ErrorFilter : IErrorFilter
{
    public IError OnError(IError error)
    {
        return error.Exception switch
        {
            BookNotFoundError => error.WithMessage("Book not found")
                                     .WithCode("BOOK_NOT_FOUND"),
            _ => error
        };
    }
}
```

### Throw Error
```csharp
public Book GetBook(string id)
{
    var book = _repository.GetById(id);
    if (book == null)
    {
        throw new BookNotFoundError(id);
    }
    return book;
}
```

## Authentication & Authorization

### [Authorize] Attribute
```csharp
public class Query
{
    [Authorize]
    public List<Book> GetBooks()
    {
        return _repository.GetAll();
    }
}
```

### Role-Based
```csharp
[Authorize(Roles = new[] { "Admin" })]
public Book DeleteBook(string id)
{
    return _repository.Delete(id);
}
```

### Policy-Based
```csharp
[Authorize(Policy = "CanManageBooks")]
public Book UpdateBook(string id, UpdateBookInput input)
{
    return _repository.Update(id, input);
}
```

### Field-Level Authorization
```csharp
public class Book
{
    public string Title { get; set; }
    
    [Authorize(Roles = new[] { "Admin" })]
    public decimal Revenue { get; set; }
}
```

## Server Configuration

### Basic Setup
```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>();

var app = builder.Build();

app.MapGraphQL();
app.Run();
```

### With All Features
```csharp
builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddSubscriptionType<Subscription>()
    .AddFiltering()
    .AddSorting()
    .AddProjections()
    .AddAuthorization()
    .AddErrorFilter<ErrorFilter>()
    .RegisterDbContext<AppDbContext>(DbContextKind.Pooled);
```

### With DataLoader
```csharp
builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddDataLoader<AuthorByIdDataLoader>();
```

## Introspection Queries

### Get All Types
```graphql
query {
  __schema {
    types {
      name
      kind
      description
    }
  }
}
```

### Get Type Details
```graphql
query {
  __type(name: "Book") {
    name
    fields {
      name
      type {
        name
        kind
      }
    }
  }
}
```

## Common Patterns

### Repository Pattern
```csharp
public interface IBookRepository
{
    List<Book> GetAll();
    Book? GetById(string id);
    Book Add(Book book);
    Book Update(string id, Book book);
    bool Delete(string id);
}

public class Query
{
    public List<Book> GetBooks([Service] IBookRepository repo)
    {
        return repo.GetAll();
    }
}
```

### Service Injection
```csharp
public class Query
{
    public List<Book> GetBooks(
        [Service] IBookService service,
        [Service] ILogger<Query> logger)
    {
        logger.LogInformation("Fetching books");
        return service.GetBooks();
    }
}
```

### Context Injection
```csharp
public class Query
{
    public List<Book> GetBooks(
        [Service] AppDbContext context)
    {
        return context.Books.ToList();
    }
}
```

## Useful Commands

```powershell
# Create new web project
dotnet new web -n GraphQLServer

# Add Hot Chocolate
dotnet add package HotChocolate.AspNetCore

# Add filtering and sorting
dotnet add package HotChocolate.Data

# Add Entity Framework
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package HotChocolate.Data.EntityFramework

# Run server
dotnet run

# Build
dotnet build

# Watch mode (auto-reload)
dotnet watch run
```

## Testing Queries with curl

```powershell
# Basic query
curl -X POST http://localhost:5001/graphql `
  -H "Content-Type: application/json" `
  -d '{"query":"{ books { title } }"}'

# Query with variables
curl -X POST http://localhost:5001/graphql `
  -H "Content-Type: application/json" `
  -d '{"query":"query($id:ID!){book(id:$id){title}}","variables":{"id":"1"}}'
```

## Performance Tips

1. **Use DataLoader** - Prevents N+1 queries
2. **Add Projections** - Only select needed fields from database
3. **Use Filtering** - Push filtering to database
4. **Enable Compression** - Reduce payload size
5. **Connection Pooling** - Reuse database connections
6. **Query Complexity** - Limit query depth/complexity
7. **Persisted Queries** - Cache common queries

## Common Issues

### Issue: Null reference on nested field
**Solution:** Use DataLoader or ensure data is loaded

### Issue: N+1 Query Problem
**Solution:** Implement DataLoader for batch loading

### Issue: Circular references
**Solution:** Use `[GraphQLIgnore]` or configure serialization

### Issue: Field not showing in schema
**Solution:** Ensure property is public and has getter

## Resources

- [Hot Chocolate Docs](https://chillicream.com/docs/hotchocolate)
- [GraphQL Spec](https://spec.graphql.org/)
- [GraphQL Best Practices](https://graphql.org/learn/best-practices/)
