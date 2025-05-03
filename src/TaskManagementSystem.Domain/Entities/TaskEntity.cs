using TaskManagementSystem.Domain.Enums;
using TaskManagementSystem.Domain.Events;

namespace TaskManagementSystem.Domain.Entities;

public class TaskEntity
{
    private readonly List<IDomainEvent> _domainEvents = [];

    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public TaskStatusEnum Status { get; set; } = TaskStatusEnum.NotStarted;

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void UpdateStatus(TaskStatusEnum newStatus)
    {
        if (!IsValidTransition(Status, newStatus))
        {
            throw new InvalidOperationException(
                $"Cannot change status from {Status} to {newStatus}."
            );
        }

        if (Status == TaskStatusEnum.InProgress && newStatus == TaskStatusEnum.Completed)
        {
            _domainEvents.Add(new TaskCompletedEvent(Id, Name));
        }

        Status = newStatus;
    }

    private static bool IsValidTransition(TaskStatusEnum current, TaskStatusEnum next)
    {
        if (current == next)
        {
            return true;
        }

        return (current, next) switch
        {
            (TaskStatusEnum.NotStarted, TaskStatusEnum.InProgress) => true,
            (TaskStatusEnum.InProgress, TaskStatusEnum.Completed) => true,
            _ => false,
        };
    }

    public void ClearDomainEvents() => _domainEvents.Clear();
}
