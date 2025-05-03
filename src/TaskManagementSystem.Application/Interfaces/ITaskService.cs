using TaskManagementSystem.Application.Dtos;

namespace TaskManagementSystem.Application.Interfaces;

public interface ITaskService
{
    Task<int> Add(TaskDto request);
    Task Update(TaskDto request);
    Task<TaskDto> GetById(int taskId);
    Task<List<TaskDto>> Get(int page, int limit);
}
