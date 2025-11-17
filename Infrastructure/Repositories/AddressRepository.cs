using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RestaurantReservation.Application.Interfaces.IRepositories;
using RestaurantReservation.Domain.Entities;
using RestaurantReservation.Infrastructure.Data;

namespace RestaurantReservation.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for <see cref="Address"/> entity operations.
/// Provides basic CRUD operations against the <see cref="ApplicationDbContext"/>.
/// </summary>
public class AddressRepository : IAddressRepository
{
    /// <summary>
    /// EF Core database context used for address operations.
    /// </summary>
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Logger used to record repository-level diagnostics and errors.
    /// </summary>
    private readonly ILogger<AddressRepository> _logger;

    /// <summary>
    /// Construct a new <see cref="AddressRepository"/>.
    /// </summary>
    /// <param name="context">Application database context.</param>
    /// <param name="logger">Logger for this repository.</param>
    public AddressRepository(ApplicationDbContext context, ILogger<AddressRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Retrieve an <see cref="Address"/> by its identifier.
    /// </summary>
    public async Task<Address?> GetByIdAsync(int id)
        => await _context.AddressesTable.FindAsync(id);

    /// <summary>
    /// Retrieve all addresses.
    /// </summary>
    public async Task<IEnumerable<Address>> GetAllAsync()
        => await _context.AddressesTable.ToListAsync();

    /// <summary>
    /// Retrieve addresses for a specific restaurant.
    /// </summary>
    public async Task<IEnumerable<Address>> GetByRestaurantIdAsync(int restaurantId)
        => await _context.AddressesTable.Where(a => a.RestaurantId == restaurantId).ToListAsync();

    /// <summary>
    /// Add a new address to the database.
    /// </summary>
    public async Task<Address> AddAsync(Address address)
    {
        try
        {
            _context.AddressesTable.Add(address);
            await _context.SaveChangesAsync();
            return address;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to add address");
            throw;
        }
    }

    /// <summary>
    /// Update an existing address.
    /// </summary>
    public async Task UpdateAsync(Address address)
    {
        try
        {
            _context.AddressesTable.Update(address);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update address {AddressId}", address.Id);
            throw;
        }
    }

    /// <summary>
    /// Delete an address by identifier.
    /// </summary>
    public async Task DeleteAsync(int id)
    {
        try
        {
            var address = await _context.AddressesTable.FindAsync(id);
            if (address != null)
            {
                _context.AddressesTable.Remove(address);
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete address {AddressId}", id);
            throw;
        }
    }
}
