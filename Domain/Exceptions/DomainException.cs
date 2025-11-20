using System;

namespace RestaurantReservation.Domain.Exceptions;

/// <summary>
/// Base exception for domain-specific errors.
/// </summary>
public class DomainException : Exception
{
    public DomainException() { }
    public DomainException(string message) : base(message) { }
    public DomainException(string message, Exception inner) : base(message, inner) { }
}
