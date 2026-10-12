# Getting Started with GraphQL Appendix

Welcome to the GraphQL learning appendix! This guide will help you get started quickly.

## What You'll Learn

This appendix covers GraphQL from basics to production deployment:

1. **Lab 1**: Basic GraphQL server and queries
2. **Lab 2**: Mutations and data modification
3. **Lab 3**: Advanced queries, filtering, and pagination
4. **Lab 4**: Real-time subscriptions
5. **Lab 5**: Production features with Entity Framework Core

## Prerequisites

Before starting, ensure you have:
- ✅ .NET 6.0 SDK or later installed
- ✅ Visual Studio 2022 or VS Code
- ✅ Basic C# and ASP.NET Core knowledge
- ✅ Understanding of HTTP and REST APIs

Check your .NET version:
```powershell
dotnet --version
```

## Quick Start (5 minutes)

### 1. Navigate to Lab 1
```powershell
cd Appendix_GraphQL/Lab1_BasicGraphQL
```

### 2. Run the Server
```powershell
cd GraphQLServer
dotnet run
```

You should see:
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:7001
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5001
```

### 3. Open Banana Cake Pop
Navigate to: **http://localhost:5001/graphql**

This is your GraphQL IDE (like Postman for GraphQL)!

### 4. Run Your First Query
In Banana Cake Pop, paste and execute:

```graphql
query {
  books {
    id
    title
    author {
      name
    }
  }
}
```

You should see:
```json
{
  "data": {
    "books": [
      {
        "id": "1",
        "title": "The Great Gatsby",
        "author": {
          "name": "F. Scott Fitzgerald"
        }
      }
    ]
  }
}
```

🎉 **Success!** You've just run your first GraphQL query!

## Understanding the Basics

### 1. GraphQL Query Language

**Ask for exactly what you need:**

```graphql
# Minimal query - just titles
query {
  books {
    title
  }
}

# Detailed query - multiple fields
query {
  books {
    id
    title
    publishedYear
    author {
      name
      birthYear
    }
  }
}

# Single item with argument
query {
  book(id: "1") {
    title
    author {
      name
    }
  }
}
```

### 2. Schema Definition

The schema defines what's possible:

```csharp
public class Book
{
    public string Id { get; set; }
    public string Title { get; set; }
    public Author Author { get; set; }
    public int PublishedYear { get; set; }
}

public class Query
{
    public List<Book> GetBooks() => /* ... */;
    public Book? GetBook(string id) => /* ... */;
}
```

### 3. The GraphQL Server

**Program.cs** sets up everything:

```csharp
builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>();

app.MapGraphQL();
```

That's it! Hot Chocolate does the rest.

## How GraphQL Works

```
1. Client sends query to /graphql endpoint
   ↓
2. GraphQL server parses and validates query
   ↓
3. Resolvers fetch the requested data
   ↓
4. Server returns JSON with exact fields requested
   ↓
5. Client receives only the data it asked for
```

## Key Concepts

### 1. Types

**Object Types:**
```graphql
type Book {
  id: ID!          # ! means required
  title: String!
  year: Int        # No ! means optional
  author: Author!
}
```

**Scalar Types:**
- `String` - Text data
- `Int` - 32-bit integer
- `Float` - Floating point number
- `Boolean` - true/false
- `ID` - Unique identifier (serialized as string)

### 2. Queries (Read Operations)

```graphql
type Query {
  books: [Book!]!              # List of books (never null)
  book(id: ID!): Book          # Single book (nullable)
  searchBooks(title: String): [Book!]!
}
```

### 3. Mutations (Write Operations)

```graphql
type Mutation {
  addBook(input: AddBookInput!): Book!
  updateBook(id: ID!, input: UpdateBookInput!): Book!
  deleteBook(id: ID!): Boolean!
}
```

### 4. Subscriptions (Real-time)

```graphql
type Subscription {
  bookAdded: Book!
  bookUpdated(id: ID!): Book!
}
```

## The Power of GraphQL

### Problem: Over-fetching with REST

**REST - Get unnecessary data:**
```
GET /api/books/1
{
  "id": 1,
  "title": "Book",
  "description": "Long text...",  // Don't need
  "isbn": "...",                   // Don't need
  "pages": 500,                    // Don't need
  "author": {
    "id": 1,
    "name": "Author",
    "bio": "Long bio...",          // Don't need
    "birthDate": "...",            // Don't need
  }
}
```

**GraphQL - Get exactly what you need:**
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

Response:
```json
{
  "data": {
    "book": {
      "title": "Book",
      "author": {
        "name": "Author"
      }
    }
  }
}
```

### Problem: Multiple REST Calls

**REST - Multiple requests:**
```
1. GET /api/users/1              → User data
2. GET /api/users/1/posts        → User's posts
3. GET /api/posts/1/comments     → Comments for each post
```

**GraphQL - Single request:**
```graphql
query {
  user(id: "1") {
    name
    posts {
      title
      comments {
        text
        author {
          name
        }
      }
    }
  }
}
```

## Banana Cake Pop Features

The built-in GraphQL IDE includes:

1. **Schema Explorer** - Browse all available types and operations
2. **Auto-completion** - IntelliSense for queries
3. **Documentation** - Built-in from your schema
4. **Query History** - Save and reuse queries
5. **Variables** - Test with different inputs
6. **Multiple Tabs** - Work on multiple queries

### Using Variables

Instead of hardcoding values:

```graphql
query GetBook($bookId: ID!) {
  book(id: $bookId) {
    title
    author {
      name
    }
  }
}
```

Variables panel:
```json
{
  "bookId": "1"
}
```

## Learning Path Recommendation

### Week 1: Basics (Lab 1)
- ✅ Set up GraphQL server
- ✅ Define types and schema
- ✅ Write queries
- ✅ Use Banana Cake Pop
- ✅ Understand resolvers

**Time**: 4-5 hours

### Week 2: Mutations (Lab 2)
- ✅ Create mutations
- ✅ Handle input types
- ✅ Validate data
- ✅ Error handling
- ✅ Best practices

**Time**: 4-5 hours

### Week 3: Advanced Queries (Lab 3)
- ✅ Filtering and sorting
- ✅ Pagination strategies
- ✅ DataLoader (solve N+1)
- ✅ Complex nested queries
- ✅ Performance optimization

**Time**: 6-7 hours

### Week 4: Real-time (Lab 4)
- ✅ Set up subscriptions
- ✅ WebSocket connections
- ✅ Real-time updates
- ✅ Broadcasting events
- ✅ Subscription patterns

**Time**: 5-6 hours

### Week 5: Production (Lab 5)
- ✅ Entity Framework Core integration
- ✅ Authentication & authorization
- ✅ Field-level permissions
- ✅ Error handling strategies
- ✅ Monitoring and logging

**Time**: 7-8 hours

## Common Questions

### Q: GraphQL vs REST - which is better?
**Neither!** They solve different problems.

**Choose GraphQL when:**
- Multiple clients with different needs (web, mobile, etc.)
- Complex, nested data requirements
- Want to minimize network requests
- Rapid frontend iteration
- Real-time features needed

**Choose REST when:**
- Simple CRUD operations
- HTTP caching is critical
- File uploads/downloads primary use
- Team is new to APIs
- Legacy systems

### Q: Is GraphQL harder to learn than REST?
Initially yes, but the investment pays off:
- REST: Easy to start, complexity grows with API
- GraphQL: Steeper learning curve, but stays manageable at scale

### Q: What about performance?
**GraphQL can be faster OR slower:**
- ✅ Faster: Single request vs multiple REST calls
- ✅ Faster: Get only needed data
- ❌ Slower: If poorly implemented (N+1 queries)
- ✅ Fast: DataLoader solves N+1 problem

### Q: Can I use GraphQL with existing databases?
**Absolutely!** Hot Chocolate works great with:
- Entity Framework Core
- Dapper
- Any data source (databases, REST APIs, gRPC, etc.)

### Q: Do I need to replace REST with GraphQL?
**No!** You can:
- Run both side-by-side
- Use GraphQL as a gateway to REST APIs
- Gradually migrate endpoint by endpoint

## Project Structure

```
Appendix_GraphQL/
├── README.md                      # Main overview
├── GETTING_STARTED.md            # This file
├── QUICK_REFERENCE.md            # Cheat sheet
│
├── Lab1_BasicGraphQL/            # Start here!
│   ├── README.md
│   └── GraphQLServer/            # Complete working server
│
├── Lab2_Mutations/               # Data modification
│   └── README.md
│
├── Lab3_AdvancedQueries/         # Filtering, pagination
│   └── README.md
│
├── Lab4_Subscriptions/           # Real-time updates
│   └── README.md
│
└── Lab5_Production/              # EF Core, auth, deployment
    └── README.md
```

## Troubleshooting

### Problem: "Cannot access /graphql"
**Solution:**
1. Ensure server is running (`dotnet run`)
2. Check the port in browser matches console output
3. Use HTTP not HTTPS if certificate issues

### Problem: "Query type not found"
**Solution:**
1. Ensure you've added `.AddQueryType<Query>()`
2. Rebuild the project
3. Check Query class is public

### Problem: Banana Cake Pop not loading
**Solution:**
1. Clear browser cache
2. Try different browser
3. Check console for JavaScript errors
4. Ensure you're using HTTP (not HTTPS) for development

## Next Steps

1. **Run Lab 1** (5 minutes)
2. **Explore Banana Cake Pop** (10 minutes)
3. **Read Lab 1 README** (15 minutes)
4. **Study the code** (30 minutes)
5. **Complete exercises** (2-3 hours)

## Resources for Learning

### Official Docs
- [GraphQL.org](https://graphql.org/learn/)
- [Hot Chocolate Docs](https://chillicream.com/docs/hotchocolate)

### Interactive Tutorials
- [How to GraphQL](https://www.howtographql.com/)
- [GraphQL Playground](https://www.graphqlbin.com/)

### Real-World Examples
- [GitHub GraphQL API](https://docs.github.com/en/graphql)
- [Shopify GraphQL API](https://shopify.dev/api/admin-graphql)

## Happy Learning! 🚀

Remember:
- Start with Lab 1
- Use Banana Cake Pop extensively
- Experiment with queries
- Read the schema documentation
- Build something real!

---

**Ready to begin?** Head to [Lab 1: Basic GraphQL](Lab1_BasicGraphQL/README.md) and start querying!
