using FluentValidation;
using FluentValidation.Results;

namespace TaskManagementSystem.Api.Validation;

public static class ValidationExtensions
{
    public static void ThrowExceptionIfNotValid(this ValidationResult result)
    {
        if (!result.IsValid)
        {
            throw new ValidationException(result.Errors);
        }
    }
}
