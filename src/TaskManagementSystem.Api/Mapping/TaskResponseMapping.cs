using TaskManagementSystem.Api.Models.Response;
using TaskManagementSystem.Application.Dtos;

namespace TaskManagementSystem.Api.Mapping;

public static class TaskResponseMapping
{
    public static TaskResponse ToTaskResponse(this TaskDto task)
    {
        return new TaskResponse
        {
            Id = task.Id,
            Name = task.Name,
            Description = task.Description,
            Status = task.Status.ToApiStatus(),
        };
    }

    public static TasksResponse ToTasksResponse(this IEnumerable<TaskDto> tasks)
    {
        return new TasksResponse { Tasks = tasks?.Select(t => t.ToTaskResponse()).ToList() ?? [] };
    }
}
