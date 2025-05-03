namespace TaskManagementSystem.Domain.Events;

public class TaskCompletedEvent(int taskId, string taskName) : IDomainEvent
{
    public int TaskId { get; } = taskId;
    public string TaskName { get; } = taskName;
}
