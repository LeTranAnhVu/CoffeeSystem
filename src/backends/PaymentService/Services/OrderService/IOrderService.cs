using PaymentService.ExternalModels;

namespace PaymentService.Services.OrderService;

public interface IOrderService
{
    public Task<Order?> GetOrderByIdAsync(int orderId, CancellationToken cancellationToken = default);
}