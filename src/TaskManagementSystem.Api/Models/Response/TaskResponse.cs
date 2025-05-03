using TaskManagementSystem.Application.Enums;

namespace TaskManagementSystem.Api.Models.Response;

public class TaskResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public ApiTaskStatusEnum Status { get; set; }
}
