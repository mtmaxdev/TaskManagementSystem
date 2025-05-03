using System.Net;
using FluentValidation;
using TaskManagementSystem.Api.Models.Response;
using TaskManagementSystem.Application.Common.Exceptions;

namespace TaskManagementSystem.Api.Middlewares;

public class ExceptionHandlingMiddleware(
    RequestDelegate next,
    ILogger<ExceptionHandlingMiddleware> logger
)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException ex)
        {
            await ErrorResponse(
                context,
                HttpStatusCode.BadRequest,
                ex.Errors?.Select(e => e.ErrorMessage) ?? ["validation error"]
            );
        }
        catch (InvalidOperationException ex)
        {
            await ErrorResponse(context, HttpStatusCode.BadRequest, new[] { ex.Message });
        }
        catch (NotFoundException ex)
        {
            await ErrorResponse(context, HttpStatusCode.NotFound, new[] { ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception occurred.");

            await ErrorResponse(
                context,
                HttpStatusCode.InternalServerError,
                ["An unexpected error occurred."]
            );
        }
    }

    private static async Task ErrorResponse(
        HttpContext context,
        HttpStatusCode status,
        IEnumerable<string> errors
    )
    {
        context.Response.StatusCode = (int)status;
        context.Response.ContentType = "application/json";

        var response = new ErrorResponse(errors);

        await context.Response.WriteAsJsonAsync(response);
    }
}
