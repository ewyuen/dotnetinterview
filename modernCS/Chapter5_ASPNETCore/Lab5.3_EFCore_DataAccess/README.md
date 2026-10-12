# Lab 5.3: Entity Framework Core & Data Access

## Overview

This lab demonstrates Entity Framework Core (EF Core) integration with ASP.NET Core, including SQLite database setup, data models, repository pattern, and full CRUD operations. You'll learn how to implement data persistence in a web API using the latest EF Core features.

## Key Concepts

- **Entity Framework Core**: ORM for .NET with LINQ support
- **SQLite Database**: File-based database for development
- **Repository Pattern**: Clean data access abstraction
- **CRUD Operations**: Create, Read, Update, Delete operations
- **Data Seeding**: Initial data population
- **Async Database Operations**: Non-blocking database access

## Project Structure

```
Lab5.3_EFCore_DataAccess/
├── Lab5.3_EFCore_DataAccess.csproj      # Project with EF Core packages
├── Program.cs                           # Application with EF Core setup
├── Controllers/
│   └── ProductsController.cs            # RESTful API controller
├── Models/
│   └── Product.cs                       # Entity model with validation
├── Data/
│   └── ApplicationDbContext.cs          # EF Core DbContext
├── Repositories/
│   ├── IProductRepository.cs            # Repository interface
│   └── ProductRepository.cs             # Repository implementation
├── Services/
│   └── DatabaseInitializer.cs           # Database setup service
└── README.md                            # This file
```

## Code Explanation

### Entity Framework Core Setup (Program.cs)

The application configures EF Core with SQLite:

1. **DbContext Registration**: `AddDbContext` with SQLite connection string
2. **Repository Registration**: Scoped lifetime for data access
3. **Database Initialization**: Automatic database creation and seeding

### Data Model (Product.cs)

The `Product` entity demonstrates:
- **Primary Key**: Auto-generated `Id` property
- **Data Annotations**: Validation attributes (`Required`, `StringLength`, `Range`)
- **Computed Properties**: `IsInStock` derived from `StockQuantity`
- **Audit Fields**: `CreatedAt` and `UpdatedAt` timestamps

### Repository Pattern

#### IProductRepository Interface
- Defines contract for data operations
- Supports async operations throughout

#### ProductRepository Implementation
- Uses EF Core `DbContext` for database operations
- Implements all CRUD operations asynchronously
- Includes proper error handling

### RESTful API Controller (ProductsController.cs)

Implements standard HTTP methods:
- **GET** `/api/products` - Retrieve all products
- **GET** `/api/products/{id}` - Retrieve specific product
- **POST** `/api/products` - Create new product
- **PUT** `/api/products/{id}` - Update existing product
- **DELETE** `/api/products/{id}` - Delete product

### Database Initialization

The `DatabaseInitializer` ensures:
- Database file creation
- Table schema generation
- Seed data insertion

## API Endpoints

### Products API
- **GET** `/api/products` - Get all products (ordered by name)
- **GET** `/api/products/{id}` - Get product by ID
- **POST** `/api/products` - Create new product
- **PUT** `/api/products/{id}` - Update existing product
- **DELETE** `/api/products/{id}` - Delete product

### Weather Forecast (for comparison)
- **GET** `/weatherforecast` - Original minimal API endpoint

## Running the Application

1. Navigate to the project directory:
   ```bash
   cd Chapter5_ASPNETCore/Lab5.3_EFCore_DataAccess
   ```

2. Run the application:
   ```bash
   dotnet run --urls "http://localhost:5002"
   ```

3. Open your browser to `http://localhost:5002/swagger` to see the Swagger UI

4. Test the CRUD operations on the Products API

## Database File

The application creates a `products.db` SQLite file in the project directory containing:
- Products table with seeded data
- Three initial products (Laptop, Mouse, Keyboard)

## Testing Results

✅ **Application builds successfully**  
✅ **SQLite database created automatically**  
✅ **Seed data inserted correctly**  
✅ **CRUD API endpoints functional**  
✅ **EF Core integration working**

### Sample API Response
```
GET /api/products
[
  {
    "id": 1,
    "name": "Laptop",
    "description": "High-performance laptop for developers",
    "price": 1299.99,
    "stockQuantity": 10,
    "createdAt": "2025-01-01T00:00:00",
    "updatedAt": null,
    "isInStock": true
  },
  // ... more products
]
```

## CRUD Operations Testing

### Create a Product
```bash
POST /api/products
Content-Type: application/json

{
  "name": "Monitor",
  "description": "27-inch 4K monitor",
  "price": 399.99,
  "stockQuantity": 8
}
```

### Get All Products
```bash
GET /api/products
```

### Update a Product
```bash
PUT /api/products/1
Content-Type: application/json

{
  "name": "Gaming Laptop",
  "description": "High-performance gaming laptop",
  "price": 1499.99,
  "stockQuantity": 5
}
```

### Delete a Product
```bash
DELETE /api/products/2
```

## Testing CRUD Operations

### Create a Product
```bash
POST /api/products
{
  "name": "Monitor",
  "description": "27-inch 4K monitor",
  "price": 399.99,
  "stockQuantity": 8
}
```

### Get All Products
```bash
GET /api/products
```

### Update a Product
```bash
PUT /api/products/1
{
  "name": "Gaming Laptop",
  "description": "High-performance gaming laptop",
  "price": 1499.99,
  "stockQuantity": 5
}
```

### Delete a Product
```bash
DELETE /api/products/2
```

## Key Features Demonstrated

- **EF Core Integration**: Seamless database integration
- **Async Operations**: Non-blocking database access
- **Data Validation**: Model validation with data annotations
- **Repository Pattern**: Clean separation of data access logic
- **RESTful Design**: Proper HTTP status codes and responses
- **Database Seeding**: Initial data population
- **SQLite**: File-based database for development

## Entity Framework Core Concepts

- **DbContext**: Central class for database operations
- **DbSet**: Typed collection for entity operations
- **LINQ Queries**: Type-safe database queries
- **Change Tracking**: Automatic entity state management
- **Migrations**: Schema evolution (though not used here)
- **Data Seeding**: Initial data population

## Next Steps

In the next lab, we'll add authentication and authorization to secure the API endpoints and implement user management with JWT tokens.

## Additional Resources

- [Entity Framework Core Documentation](https://docs.microsoft.com/en-us/ef/core/)
- [Repository Pattern](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/infrastructure-persistence-layer-design)
- [RESTful API Design](https://docs.microsoft.com/en-us/azure/architecture/best-practices/api-design)