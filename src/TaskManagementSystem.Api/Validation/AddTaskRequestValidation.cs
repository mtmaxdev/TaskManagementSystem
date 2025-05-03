using FluentValidation;
using TaskManagementSystem.Api.Models.Requests;

namespace TaskManagementSystem.Api.Validation;

public class AddTaskRequestValidation : AbstractValidator<AddTaskRequest>
{
    public AddTaskRequestValidation()
    {
        RuleFor(input => input.Name)
            .NotNull()
            .NotEmpty();
    }
}