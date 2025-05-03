using TaskManagementSystem.Domain.Events;

namespace TaskManagementSystem.Application.Interfaces;

public interface ITaskEventService
{
    Task ProcessEvents(IEnumerable<IDomainEvent> domainEvents);
}
