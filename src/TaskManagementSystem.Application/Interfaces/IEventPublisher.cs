using TaskManagementSystem.Domain.Events;

namespace TaskManagementSystem.Application.Interfaces;

public interface IEventPublisher
{
    Task PublishAsync<T>(T taskEvent, CancellationToken cancellationToken = default)
        where T : IDomainEvent;
}
