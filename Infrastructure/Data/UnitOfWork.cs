using Microsoft.EntityFrameworkCore.Storage;
using RestaurantReservation.Application.Interfaces;

namespace RestaurantReservation.Infrastructure.Data
{
    /// <summary>
    /// Unit of Work implementation using EF Core transactions.
    /// Manages a single <see cref="IDbContextTransaction"/> instance for the lifetime of an operation.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Begin a new database transaction if one is not already active.
        /// </summary>
        public async Task BeginTransactionAsync()
        {
            if (_transaction == null)
                _transaction = await _context.Database.BeginTransactionAsync();
        }

        /// <summary>
        /// Commit and dispose the current transaction.
        /// </summary>
        public async Task CommitAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        /// <summary>
        /// Roll back and dispose the current transaction.
        /// </summary>
        public async Task RollbackAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }
}
