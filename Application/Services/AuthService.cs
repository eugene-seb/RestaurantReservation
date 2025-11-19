using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using RestaurantReservation.Domain.Enums;
using RestaurantReservation.Application.DTOs;
using RestaurantReservation.Application.Interfaces.IServices;
using System.Security.Claims;
using System.Text;

namespace RestaurantReservation.Application.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthService> _logger;

    public AuthService(UserManager<IdentityUser> userManager, IConfiguration configuration, ILogger<AuthService> logger)
    {
        _userManager = userManager;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<IdentityResult> RegisterAsync(UserRegistrationDto registerDto)
    {
        var user = new IdentityUser
        {
            UserName = registerDto.Email,
            Email = registerDto.Email,
            PhoneNumber = registerDto.PhoneNumber
        };

        var result = await _userManager.CreateAsync(user, registerDto.Password);

        if (result.Succeeded)
        {
            // Assign default role
            await _userManager.AddToRoleAsync(user, nameof(UserRole.Customer));
            _logger.LogInformation("User {Email} registered successfully", registerDto.Email);
        }

        return result;
    }

    public async Task<UserAuthenticatedDto?> LoginAsync(UserLoginDto loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
        {
            _logger.LogWarning("Login failed for email: {Email}", loginDto.Email);
            return null;
        }

        var roles = await _userManager.GetRolesAsync(user);
        var token = GenerateJwtToken(user, roles);

        _logger.LogInformation("User {Email} logged in successfully", loginDto.Email);

        return new UserAuthenticatedDto
        {
            Token = token,
            // Compute expiration consistently with token generation (configurable via Jwt:ExpiresInHours)
            Expiration = DateTime.UtcNow.AddHours(int.TryParse(_configuration["Jwt:ExpiresInHours"], out var h) && h > 0 ? h : 2),
            UserId = user.Id,
            Email = user.Email!,
            Role = roles.FirstOrDefault() ?? nameof(UserRole.Customer)
        };
    }

    public async Task<IdentityResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return IdentityResult.Failed(new IdentityError { Description = "User not found" });

        return await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
    }

    private string GenerateJwtToken(IdentityUser user, IList<string> roles)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email!),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not configured")));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Read expiration from configuration (hours). Default to 2 hours when not configured or invalid.
        var expiresHours = 2;
        if (int.TryParse(_configuration["Jwt:ExpiresInHours"], out var configuredHours) && configuredHours > 0)
        {
            expiresHours = configuredHours;
        }

        var expires = DateTime.UtcNow.AddHours(expiresHours);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: creds);

        var handler = new JwtSecurityTokenHandler();
        return handler.WriteToken(token);
    }
}