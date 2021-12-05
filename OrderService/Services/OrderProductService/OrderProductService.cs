using OrderService.AsyncMessageDtos;
using OrderService.Constants;
using OrderService.Exceptions;
using OrderService.Models;
using OrderService.Repositories;
using RabbitMqServiceExtension.AsyncMessageService;

namespace OrderService.Services.OrderProductService;

public class OrderProductService : IOrderProductService
{
    private readonly IOrderRepository _orderRepo;
    private readonly IRabbitMqService _mqService;
    private readonly string _messageTopic = "order.";

    public OrderProductService(IOrderRepository orderRepo, IRabbitMqService mqService)
    {
        _orderRepo = orderRepo;
        _mqService = mqService;
    }

    public async Task<Order> CreateOrderAsync(Order order, IReadOnlyList<int> productIds,
        CancellationToken cancellationToken = default)
    {
         var newOrder =  await _orderRepo.CreateAsync(order, productIds, cancellationToken);
         var contract = new OrderCreatedDto() { OrderId = newOrder.Id, OrderedBy = newOrder.OrderedBy};
         _mqService.SendMessage($"{_messageTopic}created", contract);
         return newOrder;
    }

    public Task<IEnumerable<Order>> GetOrdersAsync(CancellationToken cancellationToken = default)
    {
        return _orderRepo.GetAllAsync(cancellationToken);
    }

    public Task<Order?> GetOrderByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return _orderRepo.GetByIdAsync(id, cancellationToken);
    }

    public async Task<Order> CancelOrderAsync(int id, CancellationToken cancellationToken = default)
    {
        // Validate status condition
        // Cannot be cancel if order is not Ordered anymore.
        var existOrder = await _orderRepo.GetByIdAsync(id, cancellationToken);
        if (existOrder is null)
        {
            throw new OrderNotFoundException(id);
        }

        if (existOrder.StatusCode != OrderStatusCode.Ordered)
        {
            // Cannot cancel
            throw new OrderCannotCancelException(id);
        }

        var updatedOrder = await _orderRepo.CancelOrderAsync(id, cancellationToken);
        var contract = new OrderStatusChangedDto() { OrderId = updatedOrder.Id, OrderedBy = updatedOrder.OrderedBy, StatusCode = updatedOrder.StatusCode,  StatusName = updatedOrder.StatusName};
        _mqService.SendMessage($"{_messageTopic}cancelled", contract);
        return updatedOrder;
    }

    public async Task<Order> UpdateOrderStatusAsync(int id, OrderStatusCode statusCode,
        CancellationToken cancellationToken = default)
    {
        var updatedOrder = await _orderRepo.UpdateStatusOrderAsync(id, statusCode, cancellationToken);
        var contract = new OrderStatusChangedDto() { OrderId = updatedOrder.Id, OrderedBy = updatedOrder.OrderedBy, StatusCode = updatedOrder.StatusCode, StatusName = updatedOrder.StatusName };
        _mqService.SendMessage($"{_messageTopic}updated.status", contract);
        return updatedOrder;

    }
}