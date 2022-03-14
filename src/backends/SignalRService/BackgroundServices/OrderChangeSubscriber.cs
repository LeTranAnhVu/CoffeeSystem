using Domain.Constants;
using Microsoft.AspNetCore.SignalR;
using RabbitMqServiceExtension.AsyncMessageService;
using SignalRService.Dtos;
using SignalRService.Hubs;

namespace SignalRService.BackgroundServices;

public class OrderChangeSubscriber : BackgroundService
{
    private readonly ILogger<OrderChangeSubscriber> _logger;
    private readonly IRabbitMqService _rabbitMqService;
    private readonly IHubContext<CommonHub, ICommonHub> _hubContext;

    public OrderChangeSubscriber(
        ILogger<OrderChangeSubscriber> logger,
        IRabbitMqService rabbitMqService,
        IHubContext<CommonHub, ICommonHub> hubContext
        )
    {
        _logger = logger;
        _rabbitMqService = rabbitMqService;
        _hubContext = hubContext;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();
        try
        {
            _logger.LogInformation("Start subscribe to Rabbit MQ, on Order change");
            await _rabbitMqService.TryToCreateConnection();
            OnOrderCreated();
            OnOrderCancelled();
            OnOrderUpdatedStatus();
        }
        catch (Exception e)
        {
            _logger.LogWarning("Cannot start to receive Rabbitmq message");
            _logger.LogWarning(e.Message);
        }
    }

    private void OnOrderCreated()
    {
        var topic = "order.created";
        _rabbitMqService.ReceiveMessageOnTopic(topic,
            (OrderCreatedDto order) =>
            {
                _logger.LogInformation($"There is new order with Id: {order.OrderId} created by {order.OrderedBy}");

                var sendGroup = "orders.create";
                _hubContext.Clients.Group(sendGroup).CreateNewOrder(order);
            });
    }

    private void OnOrderCancelled()
    {
        var topic = "order.cancelled";
        _rabbitMqService.ReceiveMessageOnTopic(topic,
            (OrderStatusChangedDto order) =>
            {
                _logger.LogInformation($"The order with Id: {order.OrderId} is cancelled");

                var sendGroup = "orders.cancel";
                _hubContext.Clients.Group(sendGroup).ChangeOrderStatus(order);
            });
    }

    private void OnOrderUpdatedStatus()
    {
        var topic = "order.updated.status";
        _rabbitMqService.ReceiveMessageOnTopic(topic,
            (OrderStatusChangedDto order) =>
            {
                _logger.LogInformation($"The order of {order.OrderedBy} with Id: {order.OrderId} is updated status to {order.StatusCode}");

                if (order.StatusCode == OrderStatusCode.Paid)
                {
                    var group = "orders.paid";
                    _hubContext.Clients.Group(group).ChangeOrderStatus(order); 
                }
                
                var sendGroup = "orders." + order.OrderedBy;
                _hubContext.Clients.Group(sendGroup).ChangeOrderStatus(order);
            });
    }
}
