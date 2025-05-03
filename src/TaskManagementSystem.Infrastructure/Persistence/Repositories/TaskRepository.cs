using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Application.Interfaces;
using TaskManagementSystem.Domain.Entities;

namespace TaskManagementSystem.Infrastructure.Persistence.Repositories;

public class TaskRepository(TaskDbContext dbContext) : ITaskRepository
{
    public async Task<TaskEntity> GetById(int id)
    {
        return await dbContext.Tasks.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<int> AddTask(TaskEntity task)
    {
        dbContext.Tasks.Add(task);

        await dbContext.SaveChangesAsync();

        return task.Id;
    }

    public async Task Update(TaskEntity task)
    {
        dbContext.Attach(task);
        dbContext.Entry(task).State = EntityState.Modified;

        await dbContext.SaveChangesAsync();
    }

    public async Task<List<TaskEntity>> Get(int page, int limit)
    {
        return await dbContext
            .Tasks.OrderBy(t => t.Id)
            .Skip((page - 1) * limit)
            .Take(limit)
            .AsNoTracking()
            .ToListAsync();
    }
}
