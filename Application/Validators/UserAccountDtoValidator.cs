using FluentValidation;
using RestaurantReservation.Application.DTOs;

namespace RestaurantReservation.Application.Validators;


/// <summary>
/// Validator for UserRegistrationDto.
/// </summary>
public class RegisterDtoValidator : AbstractValidator<UserRegistrationDto>
{
    public RegisterDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("A valid email address is required.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required.")
            .MinimumLength(8)
            .WithMessage("Password must be at least 8 characters long.")
            .Matches("[A-Z]")
            .WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[a-z]")
            .WithMessage("Password must contain at least one lowercase letter.")
            .Matches("[0-9]")
            .WithMessage("Password must contain at least one digit.");

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("First name is required.")
            .MaximumLength(100)
            .WithMessage("First name must be at most 100 characters long.");

        RuleFor(x => x.LastName)
            .MaximumLength(100)
            .WithMessage("Last name must be at most 100 characters long.");

        RuleFor(x => x.PhoneNumber)
            .Matches(@"^\+?[1-9]\d{1,14}$")
            .WithMessage("Phone number must be in valid international format.")
            .When(x => !string.IsNullOrEmpty(x.PhoneNumber));
    }
}