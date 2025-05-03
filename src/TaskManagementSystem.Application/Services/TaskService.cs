using TaskManagementSystem.Application.Common.Exceptions;
using TaskManagementSystem.Application.Dtos;
using TaskManagementSystem.Application.Interfaces;
using TaskManagementSystem.Application.Mapping;

namespace TaskManagementSystem.Application.Services;

public class TaskService(ITaskRepository taskRepository, ITaskEventService taskEventService)
    : ITaskService
{
    public async Task<int> Add(TaskDto request)
    {
        var taskId = await taskRepository.AddTask(request.ToNewTaskEntity());

        return taskId;
    }

    public async Task Update(TaskDto request)
    {
        var taskEntity = request.ToTaskEntity();

        var task = await taskRepository.GetById(taskEntity.Id);

        if (task is null)
        {
            throw new NotFoundException(request.Id);
        }

        task.UpdateStatus(request.Status);

        await taskRepository.Update(task);

        await taskEventService.ProcessEvents(task.DomainEvents);

        task.ClearDomainEvents();
    }

    public async Task<TaskDto> GetById(int taskId)
    {
        var task = await taskRepository.GetById(taskId);

        if (task is null)
        {
            throw new NotFoundException(taskId);
        }

        return task.ToTaskDto();
    }

    public async Task<List<TaskDto>> Get(int page, int limit)
    {
        var tasks = await taskRepository.Get(page, limit);

        return tasks.ToTaskDtos();
    }
}
