using TaskManagementSystem.Domain.Enums;

namespace TaskManagementSystem.Application.Dtos;

public class TaskDto
{
    public TaskDto() { }

    public TaskDto(int id, string name, string description, TaskStatusEnum status)
    {
        Id = id;
        Name = name;
        Description = description;
        Status = status;
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public TaskStatusEnum Status { get; set; } = TaskStatusEnum.NotStarted;
}
