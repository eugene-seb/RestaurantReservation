using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RestaurantReservation.Application.DTOs;

/// <summary>
/// DTO used when registering a new user.
/// </summary>
public class UserRegistrationDto
{
    /// <summary>User email address (required).</summary>
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    /// <summary>User password (required). Minimum length enforced.</summary>
    [Required]
    [StringLength(100, MinimumLength = 8)]
    public string Password { get; set; } = string.Empty;

    /// <summary>User first name (required).</summary>
    [Required]
    public string FirstName { get; set; } = string.Empty;

    /// <summary>User last name (required).</summary>
    [Required]
    public string LastName { get; set; } = string.Empty;

    /// <summary>User phone number (optional).</summary>
    [Phone]
    public string? PhoneNumber { get; set; }
}

/// <summary>
/// DTO used for user login requests.
/// </summary>
public class UserLoginDto
{
    /// <summary>User email (required).</summary>
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    /// <summary>User password (required).</summary>
    [Required]
    [PasswordPropertyText]
    public string Password { get; set; } = string.Empty;
}

/// <summary>
/// DTO representing a user's public profile information.
/// </summary>
public class UserProfileDto
{
    /// <summary>User email address.</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>User first name.</summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>User last name.</summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>User phone number.</summary>
    public string PhoneNumber { get; set; } = string.Empty;
}