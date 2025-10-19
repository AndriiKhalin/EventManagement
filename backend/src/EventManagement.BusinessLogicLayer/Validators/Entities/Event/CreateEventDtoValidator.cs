using EventManagement.Domain.DTOs.EntitiesDTOs.Event;
using FluentValidation;

namespace EventManagement.BusinessLogicLayer.Validators.Entities.Event;

public class CreateEventDtoValidator : AbstractValidator<CreateEventDto>
{
    public CreateEventDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .MaximumLength(2000);

        RuleFor(x => x.StartDateTime)
            .NotEmpty().WithMessage("Date and time are required")
            .GreaterThan(DateTime.UtcNow).WithMessage("Event cannot be in the past");

        RuleFor(x => x.Location)
            .NotEmpty().WithMessage("Location is required")
            .MaximumLength(300);

        RuleFor(x => x.Capacity)
            .GreaterThan(0).When(x => x.Capacity.HasValue)
            .WithMessage("Capacity must be greater than 0");
    }
}