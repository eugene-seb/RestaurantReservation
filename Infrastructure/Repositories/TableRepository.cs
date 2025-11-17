using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RestaurantReservation.Application.Interfaces.IRepositories;
using RestaurantReservation.Domain.Entities;
using RestaurantReservation.Infrastructure.Data;

namespace RestaurantReservation.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for <see cref="Table"/> entities.
/// Handles CRUD and query operations against the database.
/// </summary>
public class TableRepository : ITableRepository
{
    /// <summary>
    /// EF Core database context used for table operations.
    /// </summary>
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Logger used to record repository diagnostics and errors.
    /// </summary>
    private readonly ILogger<TableRepository> _logger;

    /// <summary>
    /// Construct a new <see cref="TableRepository"/> instance.
    /// </summary>
    /// <param name="context">Application database context.</param>
    /// <param name="logger">Logger for this repository.</param>
    public TableRepository(ApplicationDbContext context, ILogger<TableRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Get a table by its identifier.
    /// </summary>
    public async Task<Table?> GetByIdAsync(int id)
        => await _context.TablesTable.FindAsync(id);

    /// <summary>
    /// Retrieve all tables.
    /// </summary>
    public async Task<IEnumerable<Table>> GetAllAsync()
        => await _context.TablesTable.ToListAsync();

    /// <summary>
    /// Get tables for a specific restaurant.
    /// </summary>
    public async Task<IEnumerable<Table>> GetByRestaurantIdAsync(int restaurantId)
        => await _context.TablesTable.Where(t => t.RestaurantId == restaurantId).ToListAsync();

    /// <summary>
    /// Add a new table to the database.
    /// </summary>
    public async Task<Table> AddAsync(Table table)
    {
        try
        {
            _context.TablesTable.Add(table);
            await _context.SaveChangesAsync();
            return table;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to add table");
            throw;
        }
    }

    /// <summary>
    /// Update an existing table.
    /// </summary>
    public async Task UpdateAsync(Table table)
    {
        try
        {
            _context.TablesTable.Update(table);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update table {TableId}", table.Id);
            throw;
        }
    }

    /// <summary>
    /// Delete a table by identifier.
    /// </summary>
    public async Task DeleteAsync(int id)
    {
        try
        {
            var table = await _context.TablesTable.FindAsync(id);
            if (table != null)
            {
                _context.TablesTable.Remove(table);
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete table {TableId}", id);
            throw;
        }
    }
}
