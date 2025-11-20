using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RestaurantReservation.Application.Interfaces.IRepositories;
using RestaurantReservation.Domain.Entities;
using RestaurantReservation.Infrastructure.Data;

namespace RestaurantReservation.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for <see cref="UserAccount"/> entities.
/// Responsible for user persistence and basic queries.
/// </summary>
public class UserAccountRepository: IUserAccountRepository
{
    /// <summary>
    /// EF Core database context used for user operations.
    /// </summary>
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Logger used to record repository diagnostics and errors.
    /// </summary>
    private readonly ILogger<UserAccountRepository> _logger;

    /// <summary>
    /// Construct a new <see cref="UserAccountRepository"/> instance.
    /// </summary>
    /// <param name="context">Application database context.</param>
    /// <param name="logger">Logger for this repository.</param>
    public UserAccountRepository(ApplicationDbContext context, ILogger<UserAccountRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Retrieve a user by identifier.
    /// </summary>
    public async Task<UserAccount?> GetByIdAsync(string id)
        => await _context.UsersTable.FindAsync(id);

    /// <summary>
    /// Retrieve all users.
    /// </summary>
    public async Task<IEnumerable<UserAccount>> GetAllAsync()
        => await _context.UsersTable.ToListAsync();

    /// <summary>
    /// Add a new user to the database.
    /// </summary>
    public async Task<UserAccount> AddAsync(UserAccount user)
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
    public async Task UpdateAsync(UserAccount user)
    {
        try
        {
            _context.UsersTable.Update(user);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update user {UserId}", user.UserId);
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
