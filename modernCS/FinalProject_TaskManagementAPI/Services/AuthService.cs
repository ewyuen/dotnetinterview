using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.Data;
using TaskManagementAPI.Models.Domain;
using TaskManagementAPI.Models.DTOs;
using TaskManagementAPI.Services.Interfaces;

namespace TaskManagementAPI.Services;

/// <summary>
/// Authentication service implementation
/// </summary>
public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IJwtService _jwtService;
    private readonly ApplicationDbContext _context;

    public AuthService(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IJwtService jwtService,
        ApplicationDbContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtService = jwtService;
        _context = context;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        // Check if user already exists
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
        {
            return new AuthResponse(false, "User with this email already exists", null, null, null, null);
        }

        // Create new user
        var user = new User
        {
            UserName = request.Email,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Bio = request.Bio,
            EmailConfirmed = true, // For demo purposes
            IsActive = true
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return new AuthResponse(false, $"Registration failed: {errors}", null, null, null, null);
        }

        // Assign default role
        await _userManager.AddToRoleAsync(user, "User");

        // Generate JWT token
        var token = _jwtService.GenerateToken(user);
        var refreshToken = _jwtService.GenerateRefreshToken();

        // Store refresh token
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
        await _userManager.UpdateAsync(user);

        // Get user roles
        var roles = await _userManager.GetRolesAsync(user);

        var userDto = new UserDto(
            user.Id,
            user.UserName ?? "",
            user.Email ?? "",
            user.FirstName,
            user.LastName,
            user.Bio,
            user.DateOfBirth,
            user.CreatedAt,
            user.LastLoginAt,
            user.IsActive,
            roles.ToList()
        );

        return new AuthResponse(true, "User registered successfully", token, refreshToken, userDto, user.Id);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return new AuthResponse(false, "Invalid email or password", null, null, null, null);
        }

        if (!user.IsActive)
        {
            return new AuthResponse(false, "Account is deactivated", null, null, null, null);
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (!result.Succeeded)
        {
            return new AuthResponse(false, "Invalid email or password", null, null, null, null);
        }

        // Update last login
        user.UpdateLastLogin();
        await _userManager.UpdateAsync(user);

        // Generate new tokens
        var token = _jwtService.GenerateToken(user);
        var refreshToken = _jwtService.GenerateRefreshToken();

        // Update refresh token
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
        await _userManager.UpdateAsync(user);

        // Get user roles
        var roles = await _userManager.GetRolesAsync(user);

        var userDto = new UserDto(
            user.Id,
            user.UserName ?? "",
            user.Email ?? "",
            user.FirstName,
            user.LastName,
            user.Bio,
            user.DateOfBirth,
            user.CreatedAt,
            user.LastLoginAt,
            user.IsActive,
            roles.ToList()
        );

        return new AuthResponse(true, "Login successful", token, refreshToken, userDto, user.Id);
    }

    public async Task<bool> ValidateUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        return user != null && user.IsActive && user.EmailConfirmed;
    }

    public async Task<UserDto?> GetUserByIdAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return null;

        var roles = await _userManager.GetRolesAsync(user);

        return new UserDto(
            user.Id,
            user.UserName ?? "",
            user.Email ?? "",
            user.FirstName,
            user.LastName,
            user.Bio,
            user.DateOfBirth,
            user.CreatedAt,
            user.LastLoginAt,
            user.IsActive,
            roles.ToList()
        );
    }

    public async Task<ServiceResult> ChangePasswordAsync(string userId, ChangePasswordRequest request)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new ServiceResult(false, "User not found");
            }

            var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return new ServiceResult(false, $"Password change failed: {errors}");
            }

            return new ServiceResult(true, "Password changed successfully");
        }
        catch (Exception ex)
        {
            return new ServiceResult(false, $"Failed to change password: {ex.Message}");
        }
    }

    public async Task<ServiceResult> UpdateProfileAsync(string userId, UpdateProfileRequest request)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new ServiceResult(false, "User not found");
            }

            // Update profile fields
            if (!string.IsNullOrEmpty(request.FirstName))
                user.FirstName = request.FirstName;

            if (!string.IsNullOrEmpty(request.LastName))
                user.LastName = request.LastName;

            if (request.Bio != null)
                user.Bio = request.Bio;

            if (request.DateOfBirth.HasValue)
                user.DateOfBirth = request.DateOfBirth;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return new ServiceResult(false, $"Profile update failed: {errors}");
            }

            return new ServiceResult(true, "Profile updated successfully");
        }
        catch (Exception ex)
        {
            return new ServiceResult(false, $"Failed to update profile: {ex.Message}");
        }
    }
}