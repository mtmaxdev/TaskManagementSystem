using TaskManagementSystem.Domain.Entities;

namespace TaskManagementSystem.Application.Interfaces;

public interface ITaskRepository
{
    Task<TaskEntity> GetById(int id);
    Task<int> AddTask(TaskEntity task);
    Task Update(TaskEntity task);
    Task<List<TaskEntity>> Get(int page, int limit);
}
