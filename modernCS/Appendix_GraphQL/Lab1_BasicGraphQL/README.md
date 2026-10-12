# Lab 1: Basic GraphQL Server

## Objectives
- Set up a Hot Chocolate GraphQL server in ASP.NET Core
- Define GraphQL types using C# classes
- Implement basic queries
- Use Banana Cake Pop (GraphQL IDE)
- Understand schema introspection
- Learn query syntax and structure

## Concepts

### What is GraphQL?

GraphQL is a query language for APIs that lets clients request exactly the data they need. Unlike REST where the server determines response structure, GraphQL puts the client in control.

**Key Benefits:**
- **Precise data fetching** - No over-fetching or under-fetching
- **Single endpoint** - All data through `/graphql`
- **Strong typing** - Schema defines all possibilities
- **Self-documenting** - Schema serves as documentation
- **Efficient** - Fetch related data in one request

### GraphQL vs REST

#### REST Example (Multiple Requests):
```
GET /api/books/1
GET /api/authors/42
GET /api/books/1/reviews
```

3 requests, lots of unnecessary data in responses.

#### GraphQL Example (Single Request):
```graphql
query {
  book(id: "1") {
    title
    author {
      name
    }
    reviews {
      rating
      comment
    }
  }
}
```

1 request, exactly the data needed!

### Hot Chocolate

Hot Chocolate is the most popular GraphQL server for .NET:
- **Code-first approach** - Use C# classes, not SDL
- **Built-in IDE** - Banana Cake Pop included
- **Excellent performance** - Benchmarked against other implementations
- **Great DX** - Intuitive APIs and helpful errors
- **Active development** - Regular updates and new features

## Project Structure

```
Lab1_BasicGraphQL/
├── README.md                    # This file
└── GraphQLServer/               # ASP.NET Core project
    ├── Program.cs               # Server configuration
    ├── Models/                  # Data models
    │   ├── Book.cs
    │   └── Author.cs
    ├── Data/                    # Mock data
    │   └── BookRepository.cs
    └── GraphQL/                 # GraphQL types
        └── Query.cs
```

## The Schema

### Types (Models/Book.cs & Models/Author.cs)

```csharp
public class Book
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string AuthorId { get; set; }
    public int PublishedYear { get; set; }
    public string Genre { get; set; }
    public int Pages { get; set; }
}

public class Author
{
    public string Id { get; set; }
    public string Name { get; set; }
    public int BirthYear { get; set; }
    public string Country { get; set; }
}
```

### Query Type (GraphQL/Query.cs)

```csharp
public class Query
{
    public List<Book> GetBooks([Service] IBookRepository repository)
    {
        return repository.GetAllBooks();
    }

    public Book? GetBook(string id, [Service] IBookRepository repository)
    {
        return repository.GetBookById(id);
    }

    public List<Author> GetAuthors([Service] IBookRepository repository)
    {
        return repository.GetAllAuthors();
    }

    public Author? GetAuthor(string id, [Service] IBookRepository repository)
    {
        return repository.GetAuthorById(id);
    }
}
```

**Key Points:**
- Method names become field names (GetBooks → books)
- `[Service]` injects dependencies from DI container
- Return types define GraphQL types
- Parameters become query arguments

## Server Configuration (Program.cs)

```csharp
var builder = WebApplication.CreateBuilder(args);

// Register repository
builder.Services.AddSingleton<IBookRepository, BookRepository>();

// Add GraphQL server
builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>();

var app = builder.Build();

// Map GraphQL endpoint
app.MapGraphQL();

app.Run();
```

That's it! Hot Chocolate handles the rest.

## Running the Lab

### 1. Navigate to Server Directory
```powershell
cd GraphQLServer
```

### 2. Restore and Build
```powershell
dotnet restore
dotnet build
```

### 3. Run the Server
```powershell
dotnet run
```

You should see:
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5001
```

### 4. Open Banana Cake Pop
Navigate to: **http://localhost:5001/graphql**

## Example Queries

### Query 1: Get All Books (Just Titles)
```graphql
query {
  books {
    title
  }
}
```

**Result:**
```json
{
  "data": {
    "books": [
      { "title": "The Great Gatsby" },
      { "title": "To Kill a Mockingbird" },
      { "title": "1984" }
    ]
  }
}
```

### Query 2: Get All Books with Details
```graphql
query {
  books {
    id
    title
    publishedYear
    genre
    pages
  }
}
```

### Query 3: Get Single Book
```graphql
query {
  book(id: "1") {
    title
    publishedYear
  }
}
```

### Query 4: Get Authors
```graphql
query {
  authors {
    name
    country
  }
}
```

### Query 5: Multiple Queries in One Request
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

### Query 6: Using Aliases
```graphql
query {
  classics: books {
    title
  }
  allAuthors: authors {
    name
  }
}
```

### Query 7: Named Query with Variables
```graphql
query GetSpecificBook($bookId: ID!) {
  book(id: $bookId) {
    title
    publishedYear
  }
}
```

Variables:
```json
{
  "bookId": "1"
}
```

## Exploring the Schema

### View All Types
In Banana Cake Pop, click "Schema" tab to see:
- All available types
- All queries
- Field descriptions
- Argument types

### Introspection Query
```graphql
query {
  __schema {
    types {
      name
    }
  }
}
```

### Query Type Details
```graphql
query {
  __type(name: "Book") {
    name
    fields {
      name
      type {
        name
      }
    }
  }
}
```

## Understanding the Response

Every GraphQL response has this structure:

```json
{
  "data": {
    // Your requested data here
  },
  "errors": [
    // Any errors (if present)
  ]
}
```

### Successful Response
```json
{
  "data": {
    "books": [
      { "title": "Book 1" }
    ]
  }
}
```

### Response with Error
```json
{
  "errors": [
    {
      "message": "Book not found",
      "path": ["book"]
    }
  ],
  "data": {
    "book": null
  }
}
```

## Key Concepts Explained

### 1. Resolvers

Resolvers are functions that return data for fields:

```csharp
// This method resolves the "books" field
public List<Book> GetBooks([Service] IBookRepository repository)
{
    return repository.GetAllBooks();
}
```

### 2. Arguments

Arguments let you parameterize queries:

```csharp
// "id" becomes a required argument
public Book? GetBook(string id, [Service] IBookRepository repository)
{
    return repository.GetBookById(id);
}
```

### 3. Nullable vs Non-Nullable

```csharp
public Book? GetBook(string id)   // Can return null (Book in schema)
public Book GetBook(string id)     // Never null (Book! in schema)
public List<Book> GetBooks()       // Never null list ([Book!]! in schema)
```

### 4. Service Injection

Use `[Service]` to inject dependencies:

```csharp
public List<Book> GetBooks(
    [Service] IBookRepository repository,
    [Service] ILogger<Query> logger)
{
    logger.LogInformation("Fetching books");
    return repository.GetAllBooks();
}
```

## Exercises

### Exercise 1: Add a Search Query
Add a query to search books by title:

```csharp
public List<Book> SearchBooks(
    string title,
    [Service] IBookRepository repository)
{
    return repository.GetAllBooks()
        .Where(b => b.Title.Contains(title, StringComparison.OrdinalIgnoreCase))
        .ToList();
}
```

Test it:
```graphql
query {
  searchBooks(title: "great") {
    title
  }
}
```

### Exercise 2: Filter by Genre
Add a query to get books by genre:

```csharp
public List<Book> BooksByGenre(
    string genre,
    [Service] IBookRepository repository)
{
    return repository.GetAllBooks()
        .Where(b => b.Genre.Equals(genre, StringComparison.OrdinalIgnoreCase))
        .ToList();
}
```

### Exercise 3: Get Books by Year Range
```csharp
public List<Book> BooksByYearRange(
    int startYear,
    int endYear,
    [Service] IBookRepository repository)
{
    return repository.GetAllBooks()
        .Where(b => b.PublishedYear >= startYear && b.PublishedYear <= endYear)
        .ToList();
}
```

### Exercise 4: Add Book Count
```csharp
public int BookCount([Service] IBookRepository repository)
{
    return repository.GetAllBooks().Count;
}
```

### Exercise 5: Get Random Book
```csharp
public Book? RandomBook([Service] IBookRepository repository)
{
    var books = repository.GetAllBooks();
    if (books.Count == 0) return null;
    
    var random = new Random();
    return books[random.Next(books.Count)];
}
```

## Best Practices

### 1. Use Descriptive Names
```csharp
// ✅ Good
public List<Book> GetBooksByAuthor(string authorId)

// ❌ Bad
public List<Book> Get(string id)
```

### 2. Handle Null Cases
```csharp
public Book? GetBook(string id, [Service] IBookRepository repository)
{
    return repository.GetBookById(id); // Returns null if not found
}
```

### 3. Use Async When Possible
```csharp
public async Task<List<Book>> GetBooks(
    [Service] IBookRepository repository,
    CancellationToken cancellationToken)
{
    return await repository.GetAllBooksAsync(cancellationToken);
}
```

### 4. Add Descriptions
```csharp
/// <summary>
/// Gets a book by its unique identifier.
/// </summary>
/// <param name="id">The book's ID</param>
[GraphQLDescription("Retrieves a single book by ID")]
public Book? GetBook(string id)
{
    // ...
}
```

## Common Patterns

### Optional Parameters
```csharp
public List<Book> GetBooks(
    string? genre = null,
    int? minYear = null,
    [Service] IBookRepository repository)
{
    var books = repository.GetAllBooks();
    
    if (genre != null)
        books = books.Where(b => b.Genre == genre).ToList();
    
    if (minYear != null)
        books = books.Where(b => b.PublishedYear >= minYear).ToList();
    
    return books;
}
```

### Default Values
```csharp
public List<Book> GetRecentBooks(
    int count = 10,
    [Service] IBookRepository repository)
{
    return repository.GetAllBooks()
        .OrderByDescending(b => b.PublishedYear)
        .Take(count)
        .ToList();
}
```

## Troubleshooting

### Problem: "Query type not registered"
**Solution:** Add `.AddQueryType<Query>()` in Program.cs

### Problem: "Cannot access /graphql"
**Solution:** Ensure `app.MapGraphQL()` is called

### Problem: "Field not appearing in schema"
**Solution:** Ensure method is public and returns a value

### Problem: "Service not resolved"
**Solution:** Register service with `builder.Services.AddSingleton<IService, Service>()`

## What's Next?

In **Lab 2: Mutations**, you'll learn:
- How to modify data (Create, Update, Delete)
- Input types and validation
- Returning meaningful responses
- Error handling strategies

## Key Takeaways

- ✅ GraphQL uses a single endpoint (`/graphql`)
- ✅ Clients specify exactly what data they need
- ✅ Schema is defined by C# classes and methods
- ✅ Hot Chocolate generates schema automatically
- ✅ Banana Cake Pop provides excellent developer experience
- ✅ Queries are flexible and composable

## Resources

- [Hot Chocolate Documentation](https://chillicream.com/docs/hotchocolate)
- [GraphQL Learn](https://graphql.org/learn/)
- [Banana Cake Pop Guide](https://chillicream.com/docs/bananacakepop)

---

**Ready to code?** Start the server and experiment with queries in Banana Cake Pop!
