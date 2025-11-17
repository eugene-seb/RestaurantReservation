using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RestaurantReservation.Application.Interfaces.IRepositories;
using RestaurantReservation.Domain.Entities;
using RestaurantReservation.Infrastructure.Data;

namespace RestaurantReservation.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for <see cref="User"/> entities.
/// Responsible for user persistence and basic queries.
/// </summary>
public class UserRepository : IUserRepository
{
    /// <summary>
    /// EF Core database context used for user operations.
    /// </summary>
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Logger used to record repository diagnostics and errors.
    /// </summary>
    private readonly ILogger<UserRepository> _logger;

    /// <summary>
    /// Construct a new <see cref="UserRepository"/> instance.
    /// </summary>
    /// <param name="context">Application database context.</param>
    /// <param name="logger">Logger for this repository.</param>
    public UserRepository(ApplicationDbContext context, ILogger<UserRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Retrieve a user by identifier.
    /// </summary>
    public async Task<User?> GetByIdAsync(string id)
        => await _context.UsersTable.FindAsync(id);

    /// <summary>
    /// Retrieve all users.
    /// </summary>
    public async Task<IEnumerable<User>> GetAllAsync()
        => await _context.UsersTable.ToListAsync();

    /// <summary>
    /// Retrieve a user by email address.
    /// </summary>
    public async Task<User?> GetByEmailAsync(string email)
        => await _context.UsersTable.FirstOrDefaultAsync(u => u.Email == email);

    /// <summary>
    /// Add a new user to the database.
    /// </summary>
    public async Task<User> AddAsync(User user)
    {
        try
        {
            _context.UsersTable.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to add user");
            throw;
        }
    }

    /// <summary>
    /// Update an existing user.
    /// </summary>
    public async Task UpdateAsync(User user)
    {
        try
        {
            _context.UsersTable.Update(user);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update user {UserId}", user.Id);
            throw;
        }
    }

    /// <summary>
    /// Delete a user by identifier.
    /// </summary>
    public async Task DeleteAsync(string id)
    {
        try
        {
            var user = await _context.UsersTable.FindAsync(id);
            if (user != null)
            {
                _context.UsersTable.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete user {UserId}", id);
            throw;
        }
    }
}
