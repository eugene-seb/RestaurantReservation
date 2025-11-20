using FluentValidation;
using RestaurantReservation.Application.DTOs;

namespace RestaurantReservation.Application.Validators;

/// <summary>
/// Validator for CreateReservationDto.
/// </summary>
public class CreateReservationDtoValidator : AbstractValidator<CreateReservationDto>
{
    public CreateReservationDtoValidator()
    {
        RuleFor(x => x.ReservationDate)
            .NotEmpty().WithMessage("Reservation date is required.")
            .GreaterThan(DateTime.UtcNow.AddHours(1))
            .WithMessage("Reservation must be at least 1 hour in advance.")
            .LessThan(DateTime.UtcNow.AddDays(30))
            .WithMessage("Reservation cannot be more than 30 days in advance.");

        RuleFor(x => x.NumberOfGuests)
            .NotEmpty()
            .WithMessage("Number of guests is required.")
            .InclusiveBetween(1, 20)
            .WithMessage("Party size must be between 1 and 20.");

        RuleFor(x => x.RestaurantId)
            .NotEmpty()
            .WithMessage("RestaurantId is required.")
            .GreaterThan(0)
            .WithMessage("RestaurantId must be greater than 0.");

        RuleFor(x => x.SpecialRequests)
            .MaximumLength(500)
            .WithMessage("Special requests must be at most 500 characters.")
            .When(x => !string.IsNullOrEmpty(x.SpecialRequests));
    }
}