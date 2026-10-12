namespace TaskManager.Shared.Models;

/// <summary>
/// Login request model
/// </summary>
public record LoginRequest(
    string Email,
    string Password
);

/// <summary>
/// Register request model
/// </summary>
public record RegisterRequest(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    string? Bio = null
);

/// <summary>
/// Authentication response model
/// </summary>
public record AuthResponse(
    bool Success,
    string Message,
    string? Token,
    string? RefreshToken,
    UserDto? User
);

/// <summary>
/// User DTO
/// </summary>
public record UserDto(
    string Id,
    string Email,
    string FirstName,
    string LastName,
    string? Bio,
    string Role,
    DateTime CreatedAt,
    DateTime? LastLoginAt
);

/// <summary>
/// Profile update request
/// </summary>
public record UpdateProfileRequest(
    string? FirstName,
    string? LastName,
    string? Bio
);

/// <summary>
/// Change password request
/// </summary>
public record ChangePasswordRequest(
    string CurrentPassword,
    string NewPassword,
    string ConfirmNewPassword
);