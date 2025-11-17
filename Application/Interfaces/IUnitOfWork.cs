namespace RestaurantReservation.Application.Interfaces
{
    /// <summary>
    /// Unit of Work abstraction to coordinate transactions across multiple repositories.
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Begin a new database transaction.
        /// </summary>
        Task BeginTransactionAsync();

        /// <summary>
        /// Commit the current transaction.
        /// </summary>
        Task CommitAsync();

        /// <summary>
        /// Roll back the current transaction.
        /// </summary>
        Task RollbackAsync();
    }
}
