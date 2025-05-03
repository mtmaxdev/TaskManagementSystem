using TaskManagementSystem.Api.Models.Requests;
using TaskManagementSystem.Application.Dtos;

namespace TaskManagementSystem.Api.Mapping;

public static class TaskRequestMapping
{
    public static TaskDto ToTaskDto(this AddTaskRequest request)
    {
        return new TaskDto { Name = request.Name, Description = request.Description };
    }

    public static TaskDto ToTaskDto(this UpdateTaskRequest request, int id)
    {
        return new TaskDto { Id = id, Status = request.Status.ToDtoStatus() };
    }
}
