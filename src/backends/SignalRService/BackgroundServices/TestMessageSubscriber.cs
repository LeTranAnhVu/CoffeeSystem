using RabbitMqServiceExtension.AsyncMessageService;

namespace SignalRService.BackgroundServices;

public class TestMessageSubscriber : BackgroundService
{
    private readonly ILogger<TestMessageSubscriber> _logger;
    private readonly IRabbitMqService _rabbitMqService;

    public TestMessageSubscriber(ILogger<TestMessageSubscriber> logger, IRabbitMqService rabbitMqService)
    {
        _logger = logger;
        _rabbitMqService = rabbitMqService;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
       stoppingToken.ThrowIfCancellationRequested();
       var topic = "order.text";
       try
       {
           await _rabbitMqService.TryToCreateConnection();
           _rabbitMqService.ReceiveMessageOnTopic(topic, (string message) =>
           {
               _logger.LogInformation($"Pingo! there is new message from topic: {topic}, message content: {message}");
           });
       }
       catch (Exception e)
       {
           _logger.LogWarning("Cannot start to receive Rabbitmq message");
           _logger.LogWarning(e.Message);
       }
    }
}