using Domain.Constants;
using OrderService.Services.OrderProductService;
using RabbitMqServiceExtension.AsyncMessageService;

namespace OrderService.BackgroundServices;

class PaymentPaidMessage
{
    public int OrderId { get; set; }
    public int PaymentId { get; set; }
}

public class PaymentSubscriber : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<PaymentSubscriber> _logger;
    private readonly IRabbitMqService _rabbitMqService;
    private readonly IOrderProductService _orderService;

    public PaymentSubscriber(
        IServiceProvider serviceProvider,
        ILogger<PaymentSubscriber> logger,
        IRabbitMqService rabbitMqService
    )
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _rabbitMqService = rabbitMqService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();
        try
        {
            _logger.LogInformation("Start subscribe to Rabbit MQ, on Order change");
            await _rabbitMqService.TryToCreateConnection();
            OnPaymentPaid(stoppingToken);
        }
        catch (Exception e)
        {
            _logger.LogWarning("Cannot start to receive Rabbitmq message");
            _logger.LogWarning(e.Message);
        }
    }

    private void OnPaymentPaid(CancellationToken cancellationToken)
    {
        var topic = "payment.paid";
        _rabbitMqService.ReceiveMessageOnTopic(topic,
            (PaymentPaidMessage message) =>
            {
                using IServiceScope scope = _serviceProvider.CreateScope();
                var orderProductService = scope.ServiceProvider.GetRequiredService<IOrderProductService>();
                _logger.LogInformation(
                    $"The order with Id {message.OrderId} is paid with payment Id {message.PaymentId}");
                orderProductService.UpdateOrderStatusAsync(
                    message.OrderId,
                    OrderStatusCode.Paid,
                    cancellationToken).Wait(cancellationToken);
            });
    }
}