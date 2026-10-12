using Microsoft.AspNetCore.Http;
using TaskManager.Shared.Services;

namespace TaskManagementMVC.Services;

/// <summary>
/// MVC-specific authentication service using session storage
/// </summary>
public class MvcAuthService : BaseAuthService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private const string TokenKey = "AuthToken";
    private const string RefreshTokenKey = "RefreshToken";

    public MvcAuthService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        : base(httpClient)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public override async Task<string?> GetTokenAsync()
    {
        var session = _httpContextAccessor.HttpContext?.Session;
        return session?.GetString(TokenKey);
    }

    public override async Task SetTokenAsync(string? token)
    {
        var session = _httpContextAccessor.HttpContext?.Session;
        if (session != null)
        {
            if (string.IsNullOrEmpty(token))
            {
                session.Remove(TokenKey);
            }
            else
            {
                session.SetString(TokenKey, token);
            }
        }
    }

    public override async Task<string?> GetRefreshTokenAsync()
    {
        var session = _httpContextAccessor.HttpContext?.Session;
        return session?.GetString(RefreshTokenKey);
    }

    public override async Task SetRefreshTokenAsync(string? token)
    {
        var session = _httpContextAccessor.HttpContext?.Session;
        if (session != null)
        {
            if (string.IsNullOrEmpty(token))
            {
                session.Remove(RefreshTokenKey);
            }
            else
            {
                session.SetString(RefreshTokenKey, token);
            }
        }
    }
}