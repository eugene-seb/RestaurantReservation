using FluentValidation;
using RestaurantReservation.Application.DTOs;

namespace RestaurantReservation.Application.Validators;

/// <summary>
/// Validator for CreateTableDto.
/// </summary>
public class CreateTableDtoValidator : AbstractValidator<CreateTableDto>
{
    public CreateTableDtoValidator()
    {
        RuleFor(x => x.TableNumber)
            .NotEmpty()
            .WithMessage("Table number is required.")
            .GreaterThan(0)
            .WithMessage("Table number must be greater than 0.");
        RuleFor(x => x.Capacity)
            .NotEmpty()
            .WithMessage("Capacity is required.")
            .InclusiveBetween(1, 20)
            .WithMessage("Capacity must be between 1 and 20.");
        RuleFor(x => x.RestaurantId)
            .NotEmpty()
            .WithMessage("RestaurantId is required.")
            .GreaterThan(0)
            .WithMessage("RestaurantId must be greater than 0.");
    }
}

/// <summary>
/// Validator for UpdateTableDto.
/// </summary>
public class UpdateTableDtoValidator : AbstractValidator<UpdateTableDto>
{
    public UpdateTableDtoValidator()
    {
        RuleFor(x => x.TableNumber)
            .NotEmpty()
            .WithMessage("Table number is required.")
            .GreaterThan(0)
            .WithMessage("Table number must be greater than 0.");
        RuleFor(x => x.Capacity)
            .NotEmpty()
            .WithMessage("Capacity is required.")
            .InclusiveBetween(1, 100)
            .WithMessage("Capacity must be between 1 and 100.");
        RuleFor(x => x.RestaurantId)
            .NotEmpty()
            .WithMessage("RestaurantId is required.")
            .GreaterThan(0)
            .WithMessage("RestaurantId must be greater than 0.");
    }
}
