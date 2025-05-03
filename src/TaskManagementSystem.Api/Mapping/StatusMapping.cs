using TaskManagementSystem.Application.Enums;
using TaskManagementSystem.Domain.Enums;

namespace TaskManagementSystem.Api.Mapping;

public static class StatusMapping
{
    public static TaskStatusEnum ToDtoStatus(this ApiTaskStatusEnum request)
    {
        return request switch
        {
            ApiTaskStatusEnum.NotStarted => TaskStatusEnum.NotStarted,
            ApiTaskStatusEnum.InProgress => TaskStatusEnum.InProgress,
            ApiTaskStatusEnum.Completed => TaskStatusEnum.Completed,
            _ => throw new ArgumentOutOfRangeException(nameof(request), request, null),
        };
    }

    public static ApiTaskStatusEnum ToApiStatus(this TaskStatusEnum request)
    {
        return request switch
        {
            TaskStatusEnum.NotStarted => ApiTaskStatusEnum.NotStarted,
            TaskStatusEnum.InProgress => ApiTaskStatusEnum.InProgress,
            TaskStatusEnum.Completed => ApiTaskStatusEnum.Completed,
            _ => throw new ArgumentOutOfRangeException(nameof(request), request, null),
        };
    }
}
