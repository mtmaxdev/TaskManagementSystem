using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using TaskManagementSystem.Domain.Events;
using TaskManagementSystem.Infrastructure.Messaging;

namespace TaskManagementSystem.EventHandler;

public class TaskEventConsumer(RabbitMqSettings settings, ILogger<TaskEventConsumer> logger)
    : BackgroundService
{
    private IConnection _connection;
    private IChannel _channel;

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var factory = new ConnectionFactory
        {
            HostName = settings.Host,
            UserName = settings.Username,
            Password = settings.Password,
        };

        _connection = await factory.CreateConnectionAsync(cancellationToken);
        _channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);

        await _channel.ExchangeDeclareAsync(
            exchange: settings.TaskExchange,
            type: ExchangeType.Fanout,
            durable: true,
            cancellationToken: cancellationToken
        );

        var queue = await _channel.QueueDeclareAsync(
            queue: settings.TaskQueue,
            durable: true,
            exclusive: true,
            autoDelete: true,
            cancellationToken: cancellationToken
        );

        var queueName = queue.QueueName;

        await _channel.QueueBindAsync(
            queue: queueName,
            exchange: settings.TaskExchange,
            routingKey: "",
            cancellationToken: cancellationToken
        );

        logger.LogInformation("Waiting for messages.");

        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.ReceivedAsync += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            try
            {
                var domainEvent = JsonSerializer.Deserialize<TaskCompletedEvent>(message);
                logger.LogInformation(
                    "TaskCompleted message has been handled. TaskId: {taskId}",
                    domainEvent?.TaskId
                );
            }
            catch (JsonException jsonEx)
            {
                logger.LogError(jsonEx, "Failed to deserialize message: {message}", message);

                ProcessFailedMessage(message, jsonEx);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to process message: {message}", message);

                ProcessFailedMessage(message, ex);
            }

            return Task.CompletedTask;
        };

        await _channel.BasicConsumeAsync(
            queueName,
            autoAck: true,
            consumer: consumer,
            cancellationToken: cancellationToken
        );
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_channel != null)
        {
            await _channel.CloseAsync(cancellationToken: cancellationToken);
            _channel = null;
        }

        if (_connection != null)
        {
            await _connection.CloseAsync(cancellationToken: cancellationToken);
            _connection = null;
        }
    }

    private void ProcessFailedMessage(string message, Exception ex)
    {
        // handle failed messages(add to dead-letter queue or some fallback)
    }
}
