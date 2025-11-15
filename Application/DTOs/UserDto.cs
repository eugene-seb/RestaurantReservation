using System;

namespace RestaurantReservation.Application.DTOs;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Used to create a user.
/// </summary>
public class UserRegistrationDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 8)]
    public string Password { get; set; } = string.Empty;

    [Required]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    public string LastName { get; set; } = string.Empty;

    [Phone]
    public string? PhoneNumber { get; set; }
}

/// <summary>
/// Used to log into the App.
/// </summary>
public class UserLoginDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [PasswordPropertyText]
    public string Password { get; set; } = string.Empty;
}

/// <summary>
/// Used to get the infos of a user.
/// </summary>
public class UserProfileDto
{
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
}