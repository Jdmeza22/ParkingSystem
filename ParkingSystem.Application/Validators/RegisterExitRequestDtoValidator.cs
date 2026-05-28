using FluentValidation;
using ParkingSystem.Application.DTOs;

namespace ParkingSystem.Application.Validators
{
    public class RegisterExitRequestDtoValidator : AbstractValidator<RegisterExitRequestDto>
    {
        public RegisterExitRequestDtoValidator()
        {
            RuleFor(x => x.Plate)
                .NotEmpty()
                .WithMessage("Plate is required.")
                .MaximumLength(10)
                .WithMessage("Plate cannot exceed 10 characters.");
        }
    }
}
