using TaskManagementSystem.Application.Enums;

namespace TaskManagementSystem.Api.Models.Requests;

public class UpdateTaskRequest
{
    public ApiTaskStatusEnum Status { get; set; }
}
