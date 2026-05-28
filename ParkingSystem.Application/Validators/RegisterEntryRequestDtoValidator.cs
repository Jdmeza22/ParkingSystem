using FluentValidation;
using ParkingSystem.Application.DTOs;
namespace ParkingSystem.Application.Validators;


public class RegisterEntryRequestDtoValidator : AbstractValidator<RegisterEntryRequestDto>
{
    public RegisterEntryRequestDtoValidator()
    {
        RuleFor(x => x.Plate)
            .NotEmpty()
            .WithMessage("Plate is required.")
            .MaximumLength(10)
            .WithMessage("Plate cannot exceed 10 characters.");

        RuleFor(x => x.VehicleTypeId)
            .GreaterThan(0)
            .WithMessage("Vehicle type is required.");
    }
}
