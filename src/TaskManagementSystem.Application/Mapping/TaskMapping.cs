using TaskManagementSystem.Application.Dtos;
using TaskManagementSystem.Domain.Entities;
using TaskManagementSystem.Domain.Enums;

namespace TaskManagementSystem.Application.Mapping;

public static class TaskMapping
{
    public static TaskEntity ToNewTaskEntity(this TaskDto task)
    {
        return new TaskEntity
        {
            Name = task.Name,
            Description = task.Description,
            Status = TaskStatusEnum.NotStarted,
        };
    }

    public static TaskEntity ToTaskEntity(this TaskDto task)
    {
        return new TaskEntity
        {
            Id = task.Id,
            Name = task.Name,
            Description = task.Description,
            Status = task.Status,
        };
    }

    public static TaskDto ToTaskDto(this TaskEntity entity)
    {
        return new TaskDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            Status = entity.Status,
        };
    }

    public static List<TaskDto> ToTaskDtos(this IEnumerable<TaskEntity> entities)
    {
        return entities?.Select(e => e.ToTaskDto()).ToList() ?? [];
    }
}
