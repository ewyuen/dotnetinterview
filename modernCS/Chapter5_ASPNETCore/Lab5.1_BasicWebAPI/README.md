# Lab 5.1: Basic ASP.NET Core Web API

## Overview

This lab introduces the fundamentals of ASP.NET Core Web API development. You'll learn how to create a minimal API using the new minimal hosting model introduced in .NET 6+.

## Key Concepts

- **Minimal APIs**: A simplified way to create HTTP APIs without controllers
- **Dependency Injection**: Built-in DI container for service registration
- **Swagger/OpenAPI**: Automatic API documentation generation
- **Record Types**: Using C# records for immutable data models

## Project Structure

```
Lab5.1_BasicWebAPI/
├── Lab5.1_BasicWebAPI.csproj    # Project file with dependencies
├── Program.cs                   # Main application entry point
└── README.md                    # This file
```

## Code Explanation

### Program.cs

The `Program.cs` file contains all the application setup and API endpoints:

1. **Builder Configuration**: Creates a `WebApplicationBuilder` and configures services
2. **Service Registration**: Adds Swagger/OpenAPI services for API documentation
3. **Middleware Pipeline**: Configures the HTTP request pipeline with Swagger UI
4. **API Endpoints**: Defines a weather forecast endpoint using minimal API syntax
5. **Data Model**: Uses a `record` type for the weather forecast data

### Key Features Demonstrated

- **Minimal Hosting Model**: Everything in a single file
- **Route Mapping**: `MapGet` for HTTP GET endpoints
- **OpenAPI Integration**: Automatic API documentation
- **Record Types**: Immutable data structures
- **Random Data Generation**: Simulating dynamic data

## Running the Application

1. Navigate to the project directory:
   ```bash
   cd Chapter5_ASPNETCore/Lab5.1_BasicWebAPI
   ```

2. Run the application:
   ```bash
   dotnet run
   ```

3. Open your browser to `https://localhost:5001/swagger` to see the Swagger UI

4. Test the `/weatherforecast` endpoint to get sample weather data

## Expected Output

When you run the application and visit the Swagger UI, you should see:
- A weather forecast endpoint
- Sample request/response documentation
- Ability to test the API directly from the browser

The API returns an array of weather forecasts with date, temperature in Celsius and Fahrenheit, and weather summary.

## Next Steps

In the next labs, we'll expand on this foundation by:
- Adding controllers for better organization
- Implementing dependency injection
- Adding Entity Framework Core for data access
- Implementing authentication and authorization