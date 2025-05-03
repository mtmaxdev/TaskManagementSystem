using FluentValidation;
using TaskManagementSystem.Api.Models.Requests;

namespace TaskManagementSystem.Api.Validation;

public class UpdateTaskRequestValidation : AbstractValidator<UpdateTaskRequest>
{
    public UpdateTaskRequestValidation()
    {
        RuleFor(input => input.Status).IsInEnum().WithMessage("Invalid status value.");
    }
}
