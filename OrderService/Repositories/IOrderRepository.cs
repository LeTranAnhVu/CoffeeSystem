using OrderService.Models;

namespace OrderService.Repositories;

public interface IOrderRepository
{
    public Task<Order> CreateAsync(Order order, IReadOnlyList<int> productIds, CancellationToken cancellationToken = default);
    public Task<IEnumerable<Order>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task<Order?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    public Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}