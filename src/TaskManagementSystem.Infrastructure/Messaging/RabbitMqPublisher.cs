using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using TaskManagementSystem.Application.Interfaces;
using TaskManagementSystem.Domain.Events;

namespace TaskManagementSystem.Infrastructure.Messaging;

public class RabbitMqPublisher(RabbitMqSettings settings) : IEventPublisher
{
    private IConnection _connection;
    private IChannel _channel;

    public async Task PublishAsync<T>(T taskEvent, CancellationToken cancellationToken = default)
        where T : IDomainEvent
    {
        var message = JsonSerializer.Serialize(taskEvent);
        var body = Encoding.UTF8.GetBytes(message);

        var channel = await GetChannel();

        await channel.BasicPublishAsync(
            exchange: settings.TaskExchange,
            routingKey: string.Empty,
            mandatory: false,
            body,
            cancellationToken: cancellationToken
        );
    }

    private async Task<IChannel> GetChannel()
    {
        if (_channel is null)
        {
            await InitAsync();
        }

        return _channel;
    }

    private async Task InitAsync()
    {
        var factory = new ConnectionFactory
        {
            HostName = settings.Host,
            UserName = settings.Username,
            Password = settings.Password,
        };

        _connection = await factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();

        await _channel.ExchangeDeclareAsync(
            exchange: "task_events",
            type: ExchangeType.Fanout,
            durable: true
        );
    }
}
