using RabbitMqServiceExtension.AsyncMessageService;

namespace SignalRService.BackgroundServices;

public class OrderChangeSubscriber : BackgroundService
{
    private readonly ILogger<OrderChangeSubscriber> _logger;
    private readonly IRabbitMqService _rabbitMqService;

    public OrderChangeSubscriber(ILogger<OrderChangeSubscriber> logger, IRabbitMqService rabbitMqService)
    {
        _logger = logger;
        _rabbitMqService = rabbitMqService;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();
        _logger.LogInformation("Start subscribe to Rabbit MQ, on Order change");
        OnOrderCreated();
        OnOrderCancelled();
        OnOrderUpdatedStatus();
        return Task.CompletedTask;
    }

    private void OnOrderCreated()
    {
        var topic = "order.created";
        _rabbitMqService.ReceiveMessageOnTopic(topic,
            (OrderCreatedContract order) =>
            {
                _logger.LogInformation($"There is new order with Id: {order.OrderId} created by {order.OrderedBy}");
            });
    }

    private void OnOrderCancelled()
    {
        var topic = "order.cancelled";
        _rabbitMqService.ReceiveMessageOnTopic(topic,
            (OrderStatusChangedContract order) =>
            {
                _logger.LogInformation($"The order with Id: {order.OrderId} is cancelled");
            });
    }

    private void OnOrderUpdatedStatus()
    {
        var topic = "order.updated.status";
        _rabbitMqService.ReceiveMessageOnTopic(topic,
            (OrderStatusChangedContract order) =>
            {
                _logger.LogInformation($"The order with Id: {order.OrderId} is updated status to {order.StatusCode}");
            });
    }
}

public class OrderCreatedContract
{
    public int OrderId { get; set; }
    public string OrderedBy { get; set; }
}

public class OrderStatusChangedContract
{
    public int OrderId { get; set; }
    public OrderStatusCode StatusCode { get; set; }
    public string OrderedBy { get; set; }
}

public enum OrderStatusCode
{
    Ordered = 1,
    Preparing,
    Ready,
    Cancelled
}