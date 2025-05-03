namespace TaskManagementSystem.Api.Models.Response;

public class TaskCreatedResponse(int id)
{
    public int Id { get; set; } = id;
}
