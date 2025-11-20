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
using RestaurantReservation.Application.Interfaces.IRepositories;
using RestaurantReservation.Application.Interfaces;
using RestaurantReservation.Domain.Entities;

namespace RestaurantReservation.Application.Services;

/// <summary>
/// Service for authentication and user management.
/// Handles registration, login, password changes, and JWT token generation.
/// </summary>
public class AuthService : IAuthService
{
    /// <summary>
    /// Repository for accessing user account domain data.
    /// </summary>
    private readonly IUserAccountRepository _userAccountRepository;

    /// <summary>
    /// Unit of work for transaction management.
    /// </summary>
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// ASP.NET Identity user manager for authentication operations.
    /// </summary>
    private readonly UserManager<IdentityUser> _userManager;

    /// <summary>
    /// Application configuration for JWT and other settings.
    /// </summary>
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Logger for authentication events.
    /// </summary>
    private readonly ILogger<AuthService> _logger;

    /// <summary>
    /// Default token validity in hours.
    /// </summary>
    private const int DEFAULT_TOKEN_VALIDITY = 2;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthService"/> class.
    /// </summary>
    public AuthService(
        UserManager<IdentityUser> userManager,
        IUserAccountRepository userAccountRepository,
        IUnitOfWork unitOfWork,
        IConfiguration configuration,
        ILogger<AuthService> logger)
    {
        _userAccountRepository = userAccountRepository;
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _configuration = configuration;
        _logger = logger;
    }

    /// <summary>
    /// Registers a new user and returns authentication info.
    /// </summary>
    /// <param name="registerDto">Registration data.</param>
    /// <returns>User authentication DTO with JWT token.</returns>
    public async Task<UserAuthenticatedDto> RegisterAsync(UserRegistrationDto registerDto)
    {
        // Create Identity user object from registration data
        var user = new IdentityUser
        {
            UserName = registerDto.Email,
            Email = registerDto.Email,
            PhoneNumber = registerDto.PhoneNumber
        };

        // Attempt to create user in Identity system
        var result = await _userManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded)
            throw new InvalidOperationException("User registration failed: " + string.Join(", ", result.Errors.Select(e => e.Description)));

        // Assign default role to new user
        await _userManager.AddToRoleAsync(user, nameof(UserRole.Customer));
        _logger.LogInformation("User {Email} registered successfully", registerDto.Email);

        // Begin transaction for domain user account creation
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            // Create domain user account entity
            var userAccount = new UserAccount
            {
                UserId = user.Id,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                CreatedAt = DateTime.UtcNow
            };
            await _userAccountRepository.AddAsync(userAccount);
            await _unitOfWork.CommitAsync();

            // Retrieve roles and generate JWT token
            var roles = await _userManager.GetRolesAsync(user);
            var token = GenerateJwtToken(user, roles);

            // Map to authentication DTO
            return GetUserAuthenticatedDto(user, userAccount, token, roles);
        }
        catch
        {
            // Rollback transaction on error
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    /// <summary>
    /// Authenticates a user and returns authentication info.
    /// </summary>
    /// <param name="loginDto">Login credentials.</param>
    /// <returns>User authentication DTO with JWT token, or null if authentication fails.</returns>
    public async Task<UserAuthenticatedDto?> LoginAsync(UserLoginDto loginDto)
    {
        // Find user by email
        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        // Validate password
        if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
        {
            _logger.LogWarning("Login failed for email: {Email}", loginDto.Email);
            return null;
        }

        // Retrieve roles and generate JWT token
        var roles = await _userManager.GetRolesAsync(user);
        var token = GenerateJwtToken(user, roles);

        // Get domain user account info
        var userAccount = await _userAccountRepository.GetByIdAsync(user.Id);

        // Map to authentication DTO
        return GetUserAuthenticatedDto(user, userAccount, token, roles);
    }

    /// <summary>
    /// Changes the password for a user.
    /// </summary>
    /// <param name="userId">User identifier.</param>
    /// <param name="currentPassword">Current password.</param>
    /// <param name="newPassword">New password.</param>
    /// <returns>Identity result indicating success or failure.</returns>
    public async Task<IdentityResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
    {
        // Find user by ID
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return IdentityResult.Failed(new IdentityError { Description = "User not found" });

        // Attempt password change
        return await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
    }

    /// <summary>
    /// Generates a JWT token for the specified user and roles.
    /// </summary>
    /// <param name="user">Identity user.</param>
    /// <param name="roles">List of user roles.</param>
    /// <returns>JWT token string.</returns>
    private string GenerateJwtToken(IdentityUser user, IList<string> roles)
    {
        // Build claims for JWT
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email!),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        // Add role claims
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        // Get signing key from configuration
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not configured")));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Determine token expiration
        var expiresHours = DEFAULT_TOKEN_VALIDITY;
        if (int.TryParse(_configuration["Jwt:ExpiresInHours"], out var configuredHours) && configuredHours > 0)
        {
            expiresHours = configuredHours;
        }

        var expires = DateTime.UtcNow.AddHours(expiresHours);

        // Create JWT token
        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: creds);

        var handler = new JwtSecurityTokenHandler();
        return handler.WriteToken(token);
    }

    /// <summary>
    /// Maps IdentityUser and UserAccount to UserAuthenticatedDto.
    /// </summary>
    /// <param name="identityUser">Identity user.</param>
    /// <param name="userAccount">Domain user account.</param>
    /// <param name="token">JWT token string.</param>
    /// <param name="roles">List of user roles.</param>
    /// <returns>User authentication DTO.</returns>
    private UserAuthenticatedDto GetUserAuthenticatedDto(
        IdentityUser identityUser,
        UserAccount? userAccount,
        string token,
        IList<string> roles)
    {
        // Map all relevant user and account info to DTO
        return new UserAuthenticatedDto
        {
            Token = token,
            Expiration = DateTime.UtcNow.AddHours(int.TryParse(_configuration["Jwt:ExpiresInHours"], out var h) && h > 0 ? h : DEFAULT_TOKEN_VALIDITY),
            UserId = identityUser.Id,
            Email = identityUser.Email ?? string.Empty,
            Role = roles.FirstOrDefault() ?? nameof(UserRole.Customer),
            FirstName = userAccount?.FirstName ?? string.Empty,
            LastName = userAccount?.LastName ?? string.Empty,
            PhoneNumber = identityUser.PhoneNumber ?? string.Empty
        };
    }
}