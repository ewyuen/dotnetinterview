# Lab 5.2: Dependency Injection & Middleware

## Overview

This lab demonstrates advanced ASP.NET Core concepts including dependency injection (DI) with different service lifetimes and custom middleware implementation. You'll learn how to structure applications with proper separation of concerns and how middleware works in the request pipeline.

## Key Concepts

- **Dependency Injection**: Service registration and injection with different lifetimes (Scoped, Singleton)
- **Custom Middleware**: Creating and configuring middleware in the request pipeline
- **Controllers**: MVC pattern implementation with dependency injection
- **Service Layer**: Separation of business logic from controllers
- **Request/Response Pipeline**: How middleware processes HTTP requests

## Project Structure

```
Lab5.2_DI_Middleware/
├── Lab5.2_DI_Middleware.csproj          # Project file
├── Program.cs                           # Application entry point with DI setup
├── Controllers/
│   └── WeatherController.cs             # MVC controller with DI
├── Services/
│   ├── IWeatherService.cs               # Service interface
│   ├── WeatherService.cs                # Service implementation (Scoped)
│   └── LoggingService.cs                # Logging service (Singleton)
├── Middleware/
│   ├── RequestLoggingMiddleware.cs      # Custom request logging middleware
│   └── ResponseTimeMiddleware.cs        # Response timing middleware
└── README.md                            # This file
```

## Code Explanation

### Dependency Injection Setup (Program.cs)

The `Program.cs` file demonstrates different service lifetimes:

1. **Scoped Services**: `AddScoped<IWeatherService, WeatherService>()`
   - New instance created per HTTP request
   - Good for services that maintain state per request

2. **Singleton Services**: `AddSingleton<ILoggingService, LoggingService>()`
   - Single instance shared across the entire application
   - Good for stateless services or shared state

### Custom Middleware

#### RequestLoggingMiddleware
- Logs incoming requests and outgoing responses
- Demonstrates dependency injection in middleware
- Uses the singleton logging service

#### ResponseTimeMiddleware
- Measures and logs response times
- Adds custom headers to responses
- Shows how to modify response data

### Service Layer

#### IWeatherService & WeatherService
- Interface-based design for testability
- Async operations with simulated delays
- Scoped lifetime ensures fresh data per request

#### LoggingService
- Singleton lifetime for application-wide logging
- Thread-safe operations with locking
- In-memory log storage for demonstration

### Controller Implementation

The `WeatherController` demonstrates:
- Constructor injection of services
- Async controller actions
- Different HTTP methods and routing
- Error handling with appropriate status codes

## API Endpoints

### Weather Controller Endpoints
- **GET** `/api/weather/forecast?days=5` - Get weather forecasts
- **GET** `/api/weather/forecast/{date}` - Get forecast for specific date
- **GET** `/api/weather/logs` - Get application logs

### Minimal API Endpoint (for comparison)
- **GET** `/weatherforecast` - Original minimal API endpoint

## Running the Application

1. Navigate to the project directory:
   ```bash
   cd Chapter5_ASPNETCore/Lab5.2_DI_Middleware
   ```

2. Run the application:
   ```bash
   dotnet run
   ```

3. Open your browser to `https://localhost:5001/swagger` to see the Swagger UI

4. Test the endpoints and observe the console logs

## Expected Behavior

When you run the application and make requests:

1. **Request Logging**: Each request is logged to the console
2. **Response Timing**: Response times are measured and logged
3. **Custom Headers**: `X-Response-Time` header is added to responses
4. **Service Injection**: Controllers receive injected services automatically
5. **Scoped vs Singleton**: Weather service creates new instances per request, logging service persists

## Testing Different Endpoints

1. **Weather Forecast**: `GET /api/weather/forecast`
   - Returns 5 days of weather data
   - Each request gets a new WeatherService instance

2. **Specific Date**: `GET /api/weather/forecast/2025-09-30`
   - Returns forecast for that date (if within 5 days)
   - Returns 404 for dates outside the range

3. **Logs**: `GET /api/weather/logs`
   - Shows all logged requests and responses
   - Demonstrates singleton service persistence

## Key Takeaways

- **Dependency Injection**: Enables loose coupling and testability
- **Service Lifetimes**: Choose appropriate lifetimes based on service needs
- **Middleware Pipeline**: Request/response processing happens in order
- **Separation of Concerns**: Controllers handle HTTP, services handle business logic
- **Async Operations**: Proper async/await usage throughout the stack

## Next Steps

In the next lab, we'll add Entity Framework Core for data persistence and database operations, building on this foundation of dependency injection and middleware.