using Microsoft.JSInterop;
using TaskManager.Shared.Services;

namespace TaskManagementApp.Services;

/// <summary>
/// Blazor-specific authentication service using local storage
/// Safely handles JS interop during prerendering by catching InvalidOperationException.
/// </summary>
public class BlazorAuthService : BaseAuthService
{
    private readonly IJSRuntime _jsRuntime;
    private const string TokenKey = "authToken";
    private const string RefreshTokenKey = "refreshToken";

    public BlazorAuthService(HttpClient httpClient, IJSRuntime jsRuntime)
        : base(httpClient)
    {
        _jsRuntime = jsRuntime;
    }

    public override async Task<string?> GetTokenAsync()
    {
        try
        {
            return await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", TokenKey);
        }
        catch (InvalidOperationException)
        {
            // JS interop not available during prerender/static rendering
            return null;
        }
        catch
        {
            return null;
        }
    }

    public override async Task SetTokenAsync(string? token)
    {
        try
        {
            if (string.IsNullOrEmpty(token))
            {
                await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", TokenKey);
            }
            else
            {
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", TokenKey, token);
            }
        }
        catch (InvalidOperationException)
        {
            // Ignore when JS interop not available (prerender).
        }
        catch
        {
            // swallow storage errors in UI layer
        }
    }

    public override async Task<string?> GetRefreshTokenAsync()
    {
        try
        {
            return await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", RefreshTokenKey);
        }
        catch (InvalidOperationException)
        {
            return null;
        }
        catch
        {
            return null;
        }
    }

    public override async Task SetRefreshTokenAsync(string? token)
    {
        try
        {
            if (string.IsNullOrEmpty(token))
            {
                await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", RefreshTokenKey);
            }
            else
            {
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", RefreshTokenKey, token);
            }
        }
        catch (InvalidOperationException)
        {
            // Ignore during prerender
        }
        catch
        {
            // swallow
        }
    }
}