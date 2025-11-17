using System;
using RestaurantReservation.Application.DTOs;
using RestaurantReservation.Domain.Entities;

namespace RestaurantReservation.Application.Interfaces;

public interface IRestaurantService
{
    Task<RestaurantDto?> GetRestaurantByIdAsync(int id);
    Task<IEnumerable<RestaurantDto>> GetRestaurantsAsync();
    Task<IEnumerable<RestaurantDto>> GetRestaurantsByNameAsync(string restaurantName);
    Task<Restaurant> CreateRestaurantAsync(CreateRestaurantDto restaurantDto);
    Task UpdateRestaurantAsync(int restaurantId, UpdateRestaurantDto restaurantDto);
    Task DeleteRestaurantAsync(int restaurantId);
}
