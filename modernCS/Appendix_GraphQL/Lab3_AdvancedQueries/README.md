# Lab 3: Advanced Queries and Optimization

## Objectives
- Implement filtering and sorting
- Add pagination (cursor and offset-based)
- Solve the N+1 query problem with DataLoader
- Optimize database queries with projections
- Handle complex nested queries

## Coming Soon

This lab will cover:

### 1. Filtering
```graphql
query {
  books(where: {
    publishedYear: { gte: 2000 }
    genre: { eq: "Fantasy" }
  }) {
    title
  }
}
```

### 2. Sorting
```graphql
query {
  books(order: { publishedYear: DESC }) {
    title
    publishedYear
  }
}
```

### 3. Pagination
**Offset-based:**
```graphql
query {
  books(skip: 0, take: 10) {
    items { title }
    totalCount
    hasNextPage
  }
}
```

**Cursor-based:**
```graphql
query {
  books(first: 10, after: "cursor") {
    edges {
      node { title }
      cursor
    }
    pageInfo {
      hasNextPage
      endCursor
    }
  }
}
```

### 4. DataLoader Pattern
Solve the N+1 query problem for nested data fetching.

### 5. Field Resolvers
Custom resolution logic for complex fields.

Stay tuned for the complete implementation!
