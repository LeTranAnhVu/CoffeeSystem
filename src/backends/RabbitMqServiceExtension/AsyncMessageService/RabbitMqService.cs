using System.Reflection;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMqServiceExtension.Exceptions;

namespace RabbitMqServiceExtension.AsyncMessageService;

public class RabbitMqService : IRabbitMqService, IDisposable
{
    private readonly ILogger<RabbitMqService> _logger;
    private readonly string _hostName;
    private readonly int _port;
    private IConnection? _connection;
    private IModel? _channel;
    private readonly string _exchange = "common_exchange";
    private readonly string _exchangeType = "topic";

    public RabbitMqService()
    {
    }

    public RabbitMqService(ILogger<RabbitMqService> logger, IOptions<RabbitMqSettings> settings)
    {
        _logger = logger;
        _hostName = settings.Value?.HostName ?? throw new NullReferenceException();
        _port = settings.Value?.Port ?? throw new NullReferenceException();
    }

    private void CreateConnection()
    {
        var factory = new ConnectionFactory() { HostName = _hostName, Port = _port };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.ExchangeDeclare(
            _exchange,
            _exchangeType,
            false,
            false
        );
    }

    private void VerifyConnection()
    {
        if (_channel is null)
        {
            // Try to reconnect
            CreateConnection();
        }
    }

    public async Task TryToCreateConnection()
    {
        int retryCount = 0;
        // Wait maximum 60s before try again.
        var policy = Policy
            .Handle<Exception>()
            .WaitAndRetryForeverAsync(retryAttempt =>
            {
                retryCount = retryAttempt;
                return TimeSpan.FromSeconds(Math.Min(30, Math.Pow(2, retryAttempt)));
            });

        await policy.ExecuteAsync(() =>
        {
            if (retryCount > 0)
            {
                Console.WriteLine($"Retry to connect to Rabbitmq ${retryCount}");
            }

            VerifyConnection();
            return Task.CompletedTask;
        });
    }

    public void SendMessage<T>(string topic, T content, bool persistent = true)
    {
            VerifyConnection();
            var contentStr = JsonSerializer.Serialize(content);
            var bytes = Encoding.UTF8.GetBytes(contentStr);
            var properties = _channel.CreateBasicProperties();
            properties.Persistent = persistent;
            _channel.BasicPublish(
                exchange: _exchange,
                routingKey: topic,
                basicProperties: properties,
                body: bytes
            );

            _logger.LogInformation($"[x] Sent message to exchange:{_exchange}, topic:{topic}");

    }

    public void ReceiveMessageOnTopic<T>(string topic, Action<T?> callback, string? queueName = null)
    {
        if (_channel is null) throw new NullReferenceException();

        if (string.IsNullOrWhiteSpace(queueName))
        {
            queueName = _channel.QueueDeclare().QueueName;
        }
        else
        {
            _channel.QueueDeclare(
                queue: queueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);
        }

        _channel.QueueBind(queueName, _exchange, topic);
        _logger.LogInformation("[*] Waiting for async messages.");
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var content = JsonSerializer.Deserialize<T>(message);
            _logger.LogInformation($"[x] Received message from exchange:{_exchange}, topic:{topic}");
            callback(content);
            _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
        };

        _channel.BasicConsume(
            queue: queueName,
            autoAck: false,
            consumer: consumer);
    }
    
    public void ReceiveMessageOnTopicWithCallbackAsync<T>(string topic, Func<T?, Task> callbackAsync, string? queueName = null)
    {
        if (_channel is null) throw new NullReferenceException();

        if (string.IsNullOrWhiteSpace(queueName))
        {
            queueName = _channel.QueueDeclare().QueueName;
        }
        else
        {
            _channel.QueueDeclare(
                queue: queueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);
        }

        _channel.QueueBind(queueName, _exchange, topic);
        _logger.LogInformation("[*] Waiting for async messages.");
        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var content = JsonSerializer.Deserialize<T>(message);
            _logger.LogInformation($"[x] Received message from exchange:{_exchange}, topic:{topic}");
            await callbackAsync(content);
            _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
        };

        _channel.BasicConsume(
            queue: queueName,
            autoAck: false,
            consumer: consumer);
    }


    public void Dispose()
    {
        _logger.LogInformation($"Disposing {GetType().Name}");
        if (_channel?.IsOpen == true)
        {
            _channel.Close();
            _connection?.Close();
        }
    }
}