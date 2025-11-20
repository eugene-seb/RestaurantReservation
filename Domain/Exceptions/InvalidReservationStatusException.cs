using System;

namespace RestaurantReservation.Domain.Exceptions;

/// <summary>
/// Exception thrown when an invalid reservation status is encountered.
/// </summary>
public class InvalidReservationStatusException : DomainException
{
    public InvalidReservationStatusException(string message) : base(message) { }
    public InvalidReservationStatusException(string message, Exception inner) : base(message, inner) { }
}
