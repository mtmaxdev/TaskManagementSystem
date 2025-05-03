namespace TaskManagementSystem.Api.Models.Response;

public class ErrorResponse(IEnumerable<string> errors)
{
    public IEnumerable<string> Errors { get; set; } = errors ?? [];
}
