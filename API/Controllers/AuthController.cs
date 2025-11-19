using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.Application.DTOs;
using RestaurantReservation.Application.Interfaces.IServices;
using System.Security.Claims;
namespace RestaurantReservation.API.Controllers;

/// <summary>
/// Controller that handles authentication endpoints: registration, login and password management.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    /// <summary>
    /// Register a new user.
    /// </summary>
    /// <param name="registerDto">Registration data.</param>
    [HttpPost("register")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(UserRegistrationDto registerDto)
    {
        var result = await _authService.RegisterAsync(registerDto);

        if (result.Succeeded)
            return Ok(new { message = "User registered successfully" });

        return BadRequest(new { errors = result.Errors.Select(e => e.Description) });
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