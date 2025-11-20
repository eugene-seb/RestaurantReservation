using FluentValidation;
using RestaurantReservation.Application.DTOs;

namespace RestaurantReservation.Application.Validators;

/// <summary>
/// Validator for CreateAddressDto.
/// </summary>
public class CreateAddressDtoValidator : AbstractValidator<CreateAddressDto>
{
    public CreateAddressDtoValidator()
    {
        RuleFor(x => x.Street)
            .NotEmpty()
            .WithMessage("Street is required.")
            .MaximumLength(200)
            .WithMessage("Street must be at most 200 characters.");
        RuleFor(x => x.ZipCode)
            .NotEmpty()
            .WithMessage("Zip code is required.")
            .InclusiveBetween(10000, 99999)
            .WithMessage("Zip code must be between 10000 and 99999.");
        RuleFor(x => x.City)
            .NotEmpty()
            .WithMessage("City is required.")
            .MaximumLength(100)
            .WithMessage("City must be at most 100 characters.");
        RuleFor(x => x.Country)
            .NotEmpty()
            .WithMessage("Country is required.")
            .MaximumLength(100)
            .WithMessage("Country must be at most 100 characters.");
        RuleFor(x => x.RestaurantId)
            .GreaterThan(0)
            .WithMessage("RestaurantId must be greater than 0.");
    }
}

/// <summary>
/// Validator for nullable CreateAddressDto (for nested validation).
/// </summary>
public class CreateAddressDtoValidatorNullable : AbstractValidator<CreateAddressDto?>
{
    public CreateAddressDtoValidatorNullable()
    {
        RuleFor(x => x!.Street)
            .NotEmpty()
            .MaximumLength(200);
        RuleFor(x => x!.ZipCode)
            .NotEmpty()
            .InclusiveBetween(10000, 99999);
        RuleFor(x => x!.City)
            .NotEmpty()
            .MaximumLength(100);
        RuleFor(x => x!.Country)
            .NotEmpty()
            .MaximumLength(100);
        RuleFor(x => x!.RestaurantId)
            .GreaterThan(0);
    }
}

/// <summary>
/// Validator for UpdateAddressDto.
/// </summary>
public class UpdateAddressDtoValidator : AbstractValidator<UpdateAddressDto>
{
    public UpdateAddressDtoValidator()
    {
        RuleFor(x => x.Street)
            .NotEmpty()
            .WithMessage("Street is required.")
            .MaximumLength(200)
            .WithMessage("Street must be at most 200 characters.");
        RuleFor(x => x.ZipCode)
            .NotEmpty()
            .WithMessage("Zip code is required.")
            .InclusiveBetween(10000, 99999)
            .WithMessage("Zip code must be between 10000 and 99999.");
        RuleFor(x => x.City)
            .NotEmpty()
            .WithMessage("City is required.")
            .MaximumLength(100)
            .WithMessage("City must be at most 100 characters.");
        RuleFor(x => x.Country)
            .NotEmpty()
            .WithMessage("Country is required.")
            .MaximumLength(100)
            .WithMessage("Country must be at most 100 characters.");
        RuleFor(x => x.RestaurantId)
            .GreaterThan(0)
            .WithMessage("RestaurantId must be greater than 0.");
    }
}
