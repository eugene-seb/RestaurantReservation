using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RestaurantReservation.Application.Interfaces.IRepositories;
using RestaurantReservation.Domain.Entities;
using RestaurantReservation.Infrastructure.Data;

namespace RestaurantReservation.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for <see cref="Reservation"/> entities.
/// Handles persistence and queries related to reservations.
/// </summary>
public class ReservationRepository : IReservationRepository
{
    /// <summary>
    /// EF Core database context used for reservation operations.
    /// </summary>
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Logger used to record repository diagnostics and errors.
    /// </summary>
    private readonly ILogger<ReservationRepository> _logger;

    /// <summary>
    /// Construct a new <see cref="ReservationRepository"/>.
    /// </summary>
    /// <param name="context">Application database context.</param>
    /// <param name="logger">Logger for this repository.</param>
    public ReservationRepository(ApplicationDbContext context, ILogger<ReservationRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Get a reservation by its id, including related table and user.
    /// </summary>
    public async Task<Reservation?> GetByIdAsync(int id)
    {
        return await _context.ReservationsTable
            .Include(r => r.Table)
            .ThenInclude(t => t.Restaurant)
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    /// <summary>
    /// Retrieve all reservations with related table and user information.
    /// </summary>
    public async Task<IEnumerable<Reservation>> GetAllAsync()
    {
        return await _context.ReservationsTable
            .Include(r => r.Table)
            .ThenInclude(t => t.Restaurant)
            .Include(r => r.User)
            .ToListAsync();
    }

    /// <summary>
    /// Retrieve reservations for a specific user, ordered by reservation date.
    /// </summary>
    public async Task<IEnumerable<Reservation>> GetByUserIdAsync(string userId)
    {
        return await _context.ReservationsTable
            .Where(r => r.UserId == userId)
            .Include(r => r.Table)
            .ThenInclude(t => t.Restaurant)
            .OrderByDescending(r => r.ReservationDate)
            .ToListAsync();
    }

    /// <summary>
    /// Retrieve reservations for a specific restaurant.
    /// </summary>
    public async Task<IEnumerable<Reservation>> GetByRestaurantIdAsync(int restaurantId)
    {
        return await _context.ReservationsTable
            .Include(r => r.Table)
            .Where(r => r.Table.RestaurantId == restaurantId)
            .ToListAsync();
    }

    /// <summary>
    /// Add a new reservation to the database.
    /// </summary>
    public async Task<Reservation> AddAsync(Reservation reservation)
    {
        try
        {
            _context.ReservationsTable.Add(reservation);
            await _context.SaveChangesAsync();
            return reservation;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to add reservation");
            throw;
        }
    }

    /// <summary>
    /// Update an existing reservation.
    /// </summary>
    public async Task UpdateAsync(Reservation reservation)
    {
        try
        {
            _context.ReservationsTable.Update(reservation);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update reservation {ReservationId}", reservation.Id);
            throw;
        }
    }

    /// <summary>
    /// Delete a reservation by its id.
    /// </summary>
    public async Task DeleteAsync(int id)
    {
        try
        {
            var reservation = await _context.ReservationsTable.FindAsync(id);
            if (reservation != null)
            {
                _context.ReservationsTable.Remove(reservation);
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete reservation {ReservationId}", id);
            throw;
        }
    }
}
