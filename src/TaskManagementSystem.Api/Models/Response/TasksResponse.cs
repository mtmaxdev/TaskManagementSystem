namespace TaskManagementSystem.Api.Models.Response;

public class TasksResponse
{
    public IReadOnlyCollection<TaskResponse> Tasks { get; set; }
}
