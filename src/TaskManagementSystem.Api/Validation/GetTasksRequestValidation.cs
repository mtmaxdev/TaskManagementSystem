using FluentValidation;
using TaskManagementSystem.Api.Models.Requests;

namespace TaskManagementSystem.Api.Validation;

public class GetTasksRequestValidation : AbstractValidator<GetTasksRequest>
{
    public GetTasksRequestValidation()
    {
        RuleFor(input => input.Page).NotEmpty().GreaterThan(0);

        RuleFor(input => input.Size).NotEmpty().GreaterThan(0).LessThan(50);
    }
}
