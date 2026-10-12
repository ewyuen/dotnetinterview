# Lab 5.4: Authentication & Authorization

     This lab demonstrates a complete JWT-based authentication and authorization system using ASP.NET Core Identity, Entity Framework Core, and role-based access control.

     ## Features Implemented

     ### 🔐 Authentication
     - **JWT Token Generation**: Secure token creation with user claims and roles
     - **User Registration**: New user signup with password hashing
     - **User Login**: Authentication with email/password validation
     - **Token Validation**: JWT token verification and claims extraction
     - **Refresh Tokens**: Token refresh mechanism (basic implementation)

     ### 👥 User Management
     - **Identity Framework**: ASP.NET Core Identity for user management
     - **Role-Based Authorization**: Admin and User roles with different permissions
     - **Password Security**: BCrypt password hashing with validation rules
     - **User Profiles**: Extended user model with first/last name and additional fields

     ### 🛡️ Authorization
     - **Role-Based Access**: Different endpoints for Admin vs User roles
     - **JWT Bearer Authentication**: Secure API access with tokens
     - **Protected Endpoints**: Controllers with authorization requirements
     - **Claims-Based Security**: User information embedded in JWT tokens

     ### 🗄️ Data Layer
     - **Entity Framework Core**: ORM with SQLite database
     - **Identity Integration**: User and role tables with relationships
     - **Database Seeding**: Automatic creation of roles and admin user
     - **Migration Support**: EF Core migrations for schema management

     ## API Endpoints

     ### Authentication Endpoints
     ```
     POST /api/auth/register
     POST /api/auth/login
     POST /api/auth/refresh
     GET  /api/auth/me
     POST /api/auth/change-password
     ```

     ### Protected User Endpoints
     ```
     GET /api/user/profile
     GET /api/user/admin-only      (Admin role required)
     GET /api/user/user-or-admin   (User or Admin role required)
     GET /api/user/claims          (Shows all user claims)
     ```

     ## Project Structure

     ```
     Lab5.4_Auth_AuthZ/
     ├── Controllers/
     │   ├── AuthController.cs     # Authentication endpoints
     │   └── UserController.cs     # Protected user endpoints
     ├── Data/
     │   ├── ApplicationDbContext.cs  # EF Core context with Identity
     │   └── DbInitializer.cs         # Database seeding
     ├── Models/
     │   ├── User.cs                  # Extended IdentityUser
     │   ├── JwtSettings.cs           # JWT configuration
     │   ├── LoginRequest.cs          # Login DTO
     │   ├── RegisterRequest.cs       # Registration DTO
     │   ├── AuthResponse.cs          # Authentication response
     │   └── UserDto.cs               # User data transfer object
     ├── Services/
     │   ├── IJwtService.cs           # JWT service interface
     │   ├── JwtService.cs            # JWT token management
     │   ├── IAuthService.cs          # Auth service interface
     │   └── AuthService.cs           # User authentication logic
     ├── Program.cs                   # Application entry point
     ├── appsettings.json             # Configuration
     └── Lab5.4_Auth_AuthZ.csproj     # Project file
     ```

     ## Configuration

     ### JWT Settings (appsettings.json)
     ```json
     {
     "JwtSettings": {
     "SecretKey": "your-secret-key-here",
     "Issuer": "AuthAuthZ",
     "Audience": "AuthAuthZ",
     "ExpiryMinutes": "60"
     }
     }
     ```

     ### Database
     - Uses SQLite for simplicity
     - Auto-creates database and tables on first run
     - Seeds Admin and User roles
     - Creates default admin user: `admin@authauthz.com` / `Admin123!`

     ## Testing the API

     ### 1. Start the Application
     ```bash
     dotnet run
     ```

     ### 2. Register a New User
     ```bash
     curl -X POST "http://localhost:5000/api/auth/register" \
          -H "Content-Type: application/json" \
          -d "{\"email\":\"test@example.com\",\"password\":\"Test123!\",\"firstName\":\"John\",\"lastName\":\"Doe\"}"
     ```

     ### 3. Login with the User
     ```bash
     curl -X POST "http://localhost:5000/api/auth/login" \
          -H "Content-Type: application/json" \
          -d "{\"email\":\"test@example.com\",\"password\":\"Test123!\"}"
     ```

     ### 4. Access Protected Endpoints
     Use the token from login response:
     ```bash
     curl -X GET "http://localhost:5000/api/user/profile" \
          -H "Authorization: Bearer YOUR_JWT_TOKEN"
     ```

     ### 5. Test Admin-Only Endpoint
     Login with admin credentials:
     ```bash
     curl -X POST "http://localhost:5000/api/auth/login" \
          -H "Content-Type: application/json" \
          -d "{\"email\":\"admin@authauthz.com\",\"password\":\"Admin123!\"}"
     ```

     Then access admin endpoint:
     ```bash
     curl -X GET "http://localhost:5000/api/user/admin-only" \
          -H "Authorization: Bearer ADMIN_JWT_TOKEN"
     ```

     ## Security Features

     - **Password Hashing**: Uses ASP.NET Core Identity's secure password hashing
     - **JWT Tokens**: Stateless authentication with configurable expiration
     - **Role-Based Access**: Granular permissions based on user roles
     - **Input Validation**: Model validation and sanitization
     - **HTTPS Enforcement**: Automatic HTTPS redirection
     - **CORS Support**: Configurable cross-origin resource sharing

     ## Swagger Documentation

     Access the API documentation at: `http://localhost:5000/swagger`

     The Swagger UI provides:
     - Interactive API testing
     - JWT Bearer token authentication
     - Request/response examples
     - Schema documentation

     ## Key Concepts Demonstrated

     1. **JWT Authentication Flow**
     2. **ASP.NET Core Identity Integration**
     3. **Role-Based Authorization**
     4. **Dependency Injection Patterns**
     5. **Entity Framework Core with Identity**
     6. **Secure Password Management**
     7. **Claims-Based Security**
     8. **API Security Best Practices**

     ## Next Steps

     This authentication system can be extended with:
     - Email confirmation and password reset
     - Two-factor authentication (2FA)
     - Social login providers
     - Refresh token rotation
     - Rate limiting and DDoS protection
     - Audit logging
     - API versioning
     - Caching strategies

     ## Running Tests

     Use the Swagger UI or curl commands to test all endpoints. The system includes comprehensive error handling and returns appropriate HTTP status codes with descriptive messages.