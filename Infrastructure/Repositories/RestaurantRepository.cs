using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RestaurantReservation.Application.Interfaces.IRepositories;
using RestaurantReservation.Domain.Entities;
using RestaurantReservation.Infrastructure.Data;

namespace RestaurantReservation.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for <see cref="Restaurant"/> entities.
/// Provides CRUD and search operations for restaurants.
/// </summary>
public class RestaurantRepository : IRestaurantRepository
{
    /// <summary>
    /// EF Core database context used for restaurant operations.
    /// </summary>
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Logger used to record repository diagnostics and errors.
    /// </summary>
    private readonly ILogger<RestaurantRepository> _logger;

    /// <summary>
    /// Construct a new <see cref="RestaurantRepository"/> instance.
    /// </summary>
    /// <param name="context">Application database context.</param>
    /// <param name="logger">Logger for this repository.</param>
    public RestaurantRepository(ApplicationDbContext context, ILogger<RestaurantRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Retrieve a restaurant by identifier.
    /// </summary>
    public async Task<Restaurant?> GetByIdAsync(int id)
        => await _context.RestaurantsTable.FindAsync(id);

    /// <summary>
    /// Retrieve all restaurants.
    /// </summary>
    public async Task<IEnumerable<Restaurant>> GetAllAsync()
        => await _context.RestaurantsTable.ToListAsync();

    /// <summary>
    /// Add a new restaurant to the database.
    /// </summary>
    public async Task<Restaurant> AddAsync(Restaurant restaurant)
    {
        try
        {
            _context.RestaurantsTable.Add(restaurant);
            await _context.SaveChangesAsync();
            return restaurant;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to add restaurant");
            throw;
        }
    }

    /// <summary>
    /// Update an existing restaurant.
    /// </summary>
    public async Task UpdateAsync(Restaurant restaurant)
    {
        try
        {
            _context.RestaurantsTable.Update(restaurant);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update restaurant {RestaurantId}", restaurant.Id);
            throw;
        }
    }

    /// <summary>
    /// Delete a restaurant by identifier.
    /// </summary>
    public async Task DeleteAsync(int id)
    {
        try
        {
            var restaurant = await _context.RestaurantsTable.FindAsync(id);
            if (restaurant != null)
            {
                _context.RestaurantsTable.Remove(restaurant);
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete restaurant {RestaurantId}", id);
            throw;
        }
    }

    /// <summary>
    /// Search restaurants by a partial name match.
    /// </summary>
    public async Task<IEnumerable<Restaurant>> SearchByNameAsync(string name)
        => await _context.RestaurantsTable.Where(r => r.Name.Contains(name)).ToListAsync();
}
