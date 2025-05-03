namespace TaskManagementSystem.Infrastructure.Messaging;

public class RabbitMqSettings
{
    public string TaskExchange { get; set; }
    public string TaskQueue { get; set; }
    public string Host { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}
