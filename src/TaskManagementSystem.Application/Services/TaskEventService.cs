using TaskManagementSystem.Application.Interfaces;
using TaskManagementSystem.Domain.Events;

namespace TaskManagementSystem.Application.Services;

public class TaskEventService(IEventPublisher eventPublisher) : ITaskEventService
{
    public async Task ProcessEvents(IEnumerable<IDomainEvent> domainEvents)
    {
        foreach (var domainEvent in domainEvents ?? [])
        {
            if (domainEvent is TaskCompletedEvent completedEvent)
            {
                await eventPublisher.PublishAsync(completedEvent);
            }
        }
    }
}
