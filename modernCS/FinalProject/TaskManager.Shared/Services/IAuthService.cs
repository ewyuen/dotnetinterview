using TaskManager.Shared.Models;
using System.Threading.Tasks;

namespace TaskManager.Shared.Services;

/// <summary>
/// Authentication service interface
/// </summary>
public interface IAuthService
{
    System.Threading.Tasks.Task<AuthResponse> LoginAsync(LoginRequest request);
    System.Threading.Tasks.Task<AuthResponse> RegisterAsync(RegisterRequest request);
    System.Threading.Tasks.Task<UserDto?> GetProfileAsync();
    System.Threading.Tasks.Task<bool> UpdateProfileAsync(UpdateProfileRequest request);
    System.Threading.Tasks.Task<bool> ChangePasswordAsync(ChangePasswordRequest request);
    System.Threading.Tasks.Task<bool> LogoutAsync();

    // Token management
    System.Threading.Tasks.Task<string?> GetTokenAsync();
    System.Threading.Tasks.Task SetTokenAsync(string? token);
    System.Threading.Tasks.Task<string?> GetRefreshTokenAsync();
    System.Threading.Tasks.Task SetRefreshTokenAsync(string? token);
    System.Threading.Tasks.Task<bool> IsAuthenticatedAsync();
}