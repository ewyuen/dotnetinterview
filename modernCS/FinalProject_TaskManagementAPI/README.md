# Task Management API

A comprehensive ASP.NET Core Web API demonstrating modern C# features, async programming, concurrent operations, and best practices. This project serves as the final integration of a complete C# learning curriculum covering:

- **Object-Oriented Programming** (Chapter 1)
- **Modern C# Features** (Chapter 2)
- **Multithreading & Concurrency** (Chapter 3)
- **Async Programming** (Chapter 4)
- **ASP.NET Core Development** (Chapter 5)

## Features

### Core Functionality
- **User Management**: Registration, authentication, and profile management
- **Task Management**: CRUD operations with status tracking and assignment
- **Project Organization**: Group tasks under projects with hierarchical structure
- **Dashboard Analytics**: Real-time statistics and productivity metrics
- **Background Processing**: Automated task processing with concurrent operations

### Technical Highlights
- **JWT Authentication**: Secure token-based authentication with role-based authorization
- **Entity Framework Core**: ORM with SQLite database and code-first migrations
- **Async/Await Patterns**: Comprehensive async operations throughout the application
- **Concurrent Processing**: Background services with SemaphoreSlim for resource management
- **Modern C# Features**: Records, pattern matching, nullable reference types, init-only properties
- **Dependency Injection**: Clean architecture with interface-based design
- **Swagger/OpenAPI**: Interactive API documentation

## Architecture

### Layered Architecture
```
Controllers/     # API endpoints with authorization
├── AuthController.cs
├── TasksController.cs
├── DashboardController.cs

Services/        # Business logic layer
├── Interfaces/  # Service contracts
├── AuthService.cs
├── TaskService.cs
├── DashboardService.cs
└── TaskProcessingService.cs

Data/           # Data access layer
├── ApplicationDbContext.cs
└── DbInitializer.cs

Models/         # Domain models and DTOs
├── Domain/     # Entity classes
├── DTOs/       # Data transfer objects
└── Enums/      # Enumeration types

BackgroundServices/  # Hosted services
└── TaskProcessingService.cs
```

### Key Design Patterns
- **Repository Pattern**: Data access abstraction
- **Service Layer**: Business logic separation
- **Dependency Injection**: Loose coupling and testability
- **Background Services**: Long-running operations
- **Records**: Immutable data structures
- **Pattern Matching**: Type-safe conditional logic

## Getting Started

### Prerequisites
- .NET 8.0 SDK
- SQLite (included with .NET)

### Installation

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd FinalProject_TaskManagementAPI
   ```

2. **Restore packages**
   ```bash
   dotnet restore
   ```

3. **Run the application**
   ```bash
   dotnet run
   ```

4. **Access the API**
   - Swagger UI: `https://localhost:5001/swagger`
   - API Base URL: `https://localhost:5001/api`

### Database Setup
The application automatically creates and seeds the SQLite database on first run with:
- Default roles (Admin, Manager, User)
- Demo users and sample data
- Pre-configured projects and tasks

## API Endpoints

### Authentication
```
POST /api/auth/register     # User registration
POST /api/auth/login        # User login (returns JWT token)
GET  /api/auth/profile      # Get current user profile
PUT  /api/auth/profile      # Update user profile
POST /api/auth/change-password # Change password
```

### Tasks
```
GET    /api/tasks           # Get user's tasks with filtering
GET    /api/tasks/{id}      # Get specific task
POST   /api/tasks           # Create new task
PUT    /api/tasks/{id}      # Update task
PATCH  /api/tasks/{id}/status # Update task status
DELETE /api/tasks/{id}      # Delete task (Manager/Admin)
PATCH  /api/tasks/{id}/assign # Assign task (Manager/Admin)
GET    /api/tasks/assigned  # Get assigned tasks
GET    /api/tasks/overdue   # Get overdue tasks
```

### Dashboard
```
GET /api/dashboard/stats     # Get dashboard statistics
GET /api/dashboard/trends    # Get completion trends
GET /api/dashboard/productivity # Get productivity metrics
GET /api/dashboard/activity  # Get recent activity
GET /api/dashboard/system-stats # System stats (Admin only)
```

## Authentication

The API uses JWT (JSON Web Tokens) for authentication. Include the token in the Authorization header:

```
Authorization: Bearer <your-jwt-token>
```

### Default Users
- **Admin**: admin@taskapi.com / Admin123!
- **Manager**: manager@taskapi.com / Manager123!
- **User**: user@taskapi.com / User123!

## Key Concepts Demonstrated

### 1. Object-Oriented Programming
- **Inheritance**: `BaseEntity` base class
- **Encapsulation**: Private fields with public properties
- **Polymorphism**: Interface implementations
- **Abstraction**: Abstract service classes

### 2. Modern C# Features
- **Records**: Immutable DTOs and value objects
- **Pattern Matching**: Switch expressions and property patterns
- **Nullable Reference Types**: Safe null handling
- **Init-only Properties**: Immutable object initialization
- **Top-level Statements**: Simplified Program.cs

### 3. Multithreading & Concurrency
- **SemaphoreSlim**: Resource synchronization
- **Concurrent Collections**: Thread-safe data structures
- **Background Services**: Long-running operations
- **Async Coordination**: Task.WhenAll for parallel processing

### 4. Async Programming
- **Async/Await**: Non-blocking operations
- **Task-based APIs**: Comprehensive async methods
- **Cancellation Tokens**: Operation cancellation
- **Exception Handling**: Async exception propagation

### 5. ASP.NET Core
- **Dependency Injection**: Service registration and lifetime management
- **Middleware Pipeline**: Request processing pipeline
- **Authorization**: Role-based and policy-based access control
- **API Controllers**: RESTful endpoint design
- **Swagger Integration**: API documentation

## Development

### Running Tests
```bash
dotnet test
```

### Database Migrations
```bash
dotnet ef migrations add <MigrationName>
dotnet ef database update
```

### Code Style
The project follows C# coding standards with:
- Nullable reference types enabled
- Implicit usings
- File-scoped namespaces
- Modern C# syntax throughout

## Contributing

This project demonstrates best practices for:
- Clean Architecture
- SOLID Principles
- RESTful API Design
- Security Best Practices
- Performance Optimization
- Code Documentation

## License

This project is part of a C# learning curriculum and is provided for educational purposes.