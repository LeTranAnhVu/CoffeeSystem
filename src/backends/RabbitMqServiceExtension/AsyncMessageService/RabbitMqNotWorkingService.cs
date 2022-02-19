using System.Reflection;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMqServiceExtension.AsyncMessageService;
[Obsolete("RabbitMqNotWorkingService is deprecated")]
public class RabbitMqNotWorkingService : RabbitMqService, IRabbitMqService, IDisposable
{
    private readonly ILogger<RabbitMqNotWorkingService> _logger;

    public RabbitMqNotWorkingService(ILogger<RabbitMqNotWorkingService> logger) : base()
    {
        _logger = logger;
        _logger.LogWarning("Rabbit MQ is NOT working");
    }

    private void CreateConnection()
    {
    }

    public void SendMessage<T>(string topic, T content, bool persistent = true)
    {
        _logger.LogWarning("Rabbit MQ is NOT working");
    }

    public void ReceiveMessageOnTopic<T>(string topic, Action<T?> callback, string? queueName = null)
    {
        _logger.LogWarning("Rabbit MQ is NOT working");
    }

    public void Dispose()
    {
        _logger.LogWarning("Rabbit MQ is NOT working");
    }
}