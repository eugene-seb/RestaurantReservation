using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.Application.DTOs;
using RestaurantReservation.Application.Interfaces.IServices;
using System.Security.Claims;

namespace RestaurantReservation.API.Controllers;

/// <summary>
/// Controller that handles authentication endpoints: registration, login, and password management.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    /// <summary>
    /// Service for authentication and user management.
    /// </summary>
    private readonly IAuthService _authService;

    /// <summary>
    /// Logger for authentication events.
    /// </summary>
    private readonly ILogger<AuthController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthController"/> class.
    /// </summary>
    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    /// <summary>
    /// Registers a new user.
    /// </summary>
    /// <param name="registerDto">Registration data.</param>
    /// <returns>200 OK if successful, 400 Bad Request otherwise.</returns>
    [HttpPost("register")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(UserRegistrationDto registerDto)
    {
        // Call the authentication service to register the user
        var result = await _authService.RegisterAsync(registerDto);

        // If registration succeeded, return success message
        if (!string.IsNullOrWhiteSpace(result.UserId))
            return Ok(new { message = "User registered successfully" });

        // Otherwise, return error message
        return BadRequest(new { error = "User registration failed" });
    }

    /// <summary>
    /// Authenticate a user and return a JWT token.
    /// </summary>
    /// <param name="loginDto">Login credentials.</param>
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(UserAuthenticatedDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<UserAuthenticatedDto>> Login(UserLoginDto loginDto)
    {
        var result = await _authService.LoginAsync(loginDto);

        if (result == null)
            return Unauthorized(new { error = "Invalid credentials" });

        return Ok(result);
    }

    /// <summary>
    /// Change the password for the currently authenticated user.
    /// </summary>
    /// <param name="dto">Current and new password values.</param>
    [HttpPost("change-password")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ChangePassword(UserChangePasswordDto dto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var result = await _authService.ChangePasswordAsync(userId, dto.CurrentPassword, dto.NewPassword);
        if (result.Succeeded)
            return NoContent();

        // If user not found, surface NotFound for clarity
        if (result.Errors.Any(e => (e.Description ?? string.Empty).Contains("User not found", StringComparison.OrdinalIgnoreCase)))
            return NotFound(new { errors = result.Errors.Select(e => e.Description) });

        return BadRequest(new { errors = result.Errors.Select(e => e.Description) });
    }
}