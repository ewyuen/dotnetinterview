using System.Net.Http.Json;
using TaskManager.Shared.Models;
using System.Threading.Tasks;

namespace TaskManager.Shared.Services;

/// <summary>
/// Base authentication service implementation
/// </summary>
public abstract class BaseAuthService : IAuthService
{
    protected readonly HttpClient _httpClient;

    protected BaseAuthService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async System.Threading.Tasks.Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", request);

            if (response.IsSuccessStatusCode)
            {
                var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();
                if (authResponse?.Success == true && authResponse.Token != null)
                {
                    await SetTokenAsync(authResponse.Token);
                    await SetRefreshTokenAsync(authResponse.RefreshToken);
                }
                return authResponse ?? new AuthResponse(false, "Invalid response", null, null, null);
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                return new AuthResponse(false, $"Login failed: {error}", null, null, null);
            }
        }
        catch (Exception ex)
        {
            return new AuthResponse(false, $"Login error: {ex.Message}", null, null, null);
        }
    }

    public async System.Threading.Tasks.Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/register", request);

            if (response.IsSuccessStatusCode)
            {
                return new AuthResponse(true, "Registration successful", null, null, null);
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                return new AuthResponse(false, $"Registration failed: {error}", null, null, null);
            }
        }
        catch (Exception ex)
        {
            return new AuthResponse(false, $"Registration error: {ex.Message}", null, null, null);
        }
    }

    public async System.Threading.Tasks.Task<UserDto?> GetProfileAsync()
    {
        try
        {
            var token = await GetTokenAsync();
            if (string.IsNullOrEmpty(token))
                return null;

            // Add token to request headers
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync("api/auth/profile");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<UserDto>();
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                // Token might be expired, clear it
                await SetTokenAsync(null);
                await SetRefreshTokenAsync(null);
                return null;
            }

            return null;
        }
        catch
        {
            return null;
        }
    }

    public async System.Threading.Tasks.Task<bool> UpdateProfileAsync(UpdateProfileRequest request)
    {
        try
        {
            var token = await GetTokenAsync();
            if (string.IsNullOrEmpty(token))
                return false;

            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PutAsJsonAsync("api/auth/profile", request);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async System.Threading.Tasks.Task<bool> ChangePasswordAsync(ChangePasswordRequest request)
    {
        try
        {
            var token = await GetTokenAsync();
            if (string.IsNullOrEmpty(token))
                return false;

            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PostAsJsonAsync("api/auth/change-password", request);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async System.Threading.Tasks.Task<bool> LogoutAsync()
    {
        await SetTokenAsync(null);
        await SetRefreshTokenAsync(null);
        _httpClient.DefaultRequestHeaders.Authorization = null;
        return true;
    }

    public async System.Threading.Tasks.Task<bool> IsAuthenticatedAsync()
    {
        var token = await GetTokenAsync();
        return !string.IsNullOrEmpty(token);
    }

    // Abstract methods for token storage - implemented by platform-specific services
    public abstract System.Threading.Tasks.Task<string?> GetTokenAsync();
    public abstract System.Threading.Tasks.Task SetTokenAsync(string? token);
    public abstract System.Threading.Tasks.Task<string?> GetRefreshTokenAsync();
    public abstract System.Threading.Tasks.Task SetRefreshTokenAsync(string? token);
}