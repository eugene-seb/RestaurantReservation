using FluentValidation;
using RestaurantReservation.Application.DTOs;

namespace RestaurantReservation.Application.Validators;

/// <summary>
/// Validator for CreateRestaurantDto.
/// </summary>
public class CreateRestaurantDtoValidator : AbstractValidator<CreateRestaurantDto>
{
    public CreateRestaurantDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Restaurant name is required.")
            .MaximumLength(200)
            .WithMessage("Restaurant name must be at most 200 characters.");
        RuleFor(x => x.OpeningTime)
            .NotNull()
            .WithMessage("Opening time is required.");
        RuleFor(x => x.ClosingTime)
            .NotNull()
            .WithMessage("Closing time is required.");
        // Address validation is handled separately
    }
}

/// <summary>
/// Validator for UpdateRestaurantDto.
/// </summary>
public class UpdateRestaurantDtoValidator : AbstractValidator<UpdateRestaurantDto>
{
    public UpdateRestaurantDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Restaurant name is required.")
            .MaximumLength(200)
            .WithMessage("Restaurant name must be at most 200 characters.");
        RuleFor(x => x.OpeningTime)
            .NotNull()
            .WithMessage("Opening time is required.");
        RuleFor(x => x.ClosingTime)
            .NotNull()
            .WithMessage("Closing time is required.");
        // Address validation is handled separately
    }
}
