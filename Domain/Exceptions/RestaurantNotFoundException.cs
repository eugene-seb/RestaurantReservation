using System;

namespace RestaurantReservation.Domain.Exceptions;

/// <summary>
/// Exception thrown when a restaurant is not found in the system.
/// </summary>
public class RestaurantNotFoundException : DomainException
{
    public RestaurantNotFoundException(string message) : base(message) { }
    public RestaurantNotFoundException(string message, Exception inner) : base(message, inner) { }
}
