using System;

namespace RestaurantReservation.Domain.Exceptions;

/// <summary>
/// Exception thrown when a requested table is not available for reservation.
/// </summary>
public class TableNotAvailableException : DomainException
{
    public TableNotAvailableException(string message) : base(message) { }
    public TableNotAvailableException(string message, Exception inner) : base(message, inner) { }
}
