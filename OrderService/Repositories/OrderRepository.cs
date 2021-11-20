using Microsoft.EntityFrameworkCore;
using OrderService.Constants;
using OrderService.Exceptions;
using OrderService.Models;

namespace OrderService.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<OrderRepository> _logger;

    public OrderRepository(AppDbContext context, ILogger<OrderRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Order> CreateAsync(Order order, IReadOnlyList<int> productIds,
        CancellationToken cancellationToken = default)
    {
        if (productIds is null || !productIds.Any())
        {
            throw new ArgumentNullException($"Parameter {nameof(productIds)} should contains at least one products");
        }

        order.StatusCode = OrderStatusCode.Ordered;
        order.OrderedAt = DateTime.UtcNow;
        await _context.Orders.AddAsync(order, cancellationToken);

        var orderedProducts = productIds.Select(productId => new OrderedProduct
        {
            OrderId = order.Id,
            ProductId = productId
        });

        await _context.OrderedProducts.AddRangeAsync(orderedProducts, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return order;
    }

    public async Task<IEnumerable<Order>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Orders.Include("OrderedProducts").ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<Order?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Orders.Include("OrderedProducts")
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken: cancellationToken);
    }

    public async Task<Order> UpdateStatusOrderAsync(int id, OrderStatusCode statusCode,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var order = await GetByIdAsync(id, cancellationToken);
            if (order is null)
            {
                throw new OrderNotFoundException(id);
            }

            order.StatusCode = statusCode;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync(cancellationToken);

            return order;
        }
        catch (Exception e)
        {
            _logger.LogError($"Cannot update status order with id {id} in {nameof(CancelOrderAsync)} : {e.Message}");
            throw;
        }
    }

    public async Task<Order> CancelOrderAsync(int id, CancellationToken cancellationToken = default)
    {
        return await UpdateStatusOrderAsync(id, OrderStatusCode.Cancelled, cancellationToken);
    }
}