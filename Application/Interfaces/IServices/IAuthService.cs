using System;
using Microsoft.AspNetCore.Identity;
using RestaurantReservation.Application.DTOs;

namespace RestaurantReservation.Application.Interfaces.IServices;

public interface IAuthService
{
    Task<IdentityResult> RegisterAsync(UserRegistrationDto registerDto);
    Task<UserAuthenticatedDto?> LoginAsync(UserLoginDto loginDto);
    Task<IdentityResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
}