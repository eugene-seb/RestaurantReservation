using System;

namespace RestaurantReservation.Domain.Exceptions;

/// <summary>
/// Exception thrown when a reservation conflict occurs (e.g., double booking).
/// </summary>
public class ReservationConflictException : DomainException
{
    public ReservationConflictException(string message) : base(message) { }
    public ReservationConflictException(string message, Exception inner) : base(message, inner) { }
}
