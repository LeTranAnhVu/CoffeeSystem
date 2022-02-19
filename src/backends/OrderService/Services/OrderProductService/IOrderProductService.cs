using OrderService.Constants;
using OrderService.Models;

namespace OrderService.Services.OrderProductService;

public interface IOrderProductService
{
    public Task<Order> CreateOrderAsync(Order order, IReadOnlyList<int> productIds,
        CancellationToken cancellationToken = default);

    public Task<IEnumerable<Order>> GetOrdersAsync(CancellationToken cancellationToken = default);
    public Task<Order?> GetOrderByIdAsync(int id, CancellationToken cancellationToken = default);
    public Task<Order> CancelOrderAsync(int id, CancellationToken cancellationToken = default);

    public Task<Order> UpdateOrderStatusAsync(int id, OrderStatusCode statusCode,
        CancellationToken cancellationToken = default);
}