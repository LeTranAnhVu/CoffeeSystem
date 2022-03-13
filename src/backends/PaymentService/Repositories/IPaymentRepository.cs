using PaymentService.Models;

namespace PaymentService.Repositories;

public interface IPaymentRepository
{
    public Task<Payment> CreateAsync(Payment payment, CancellationToken cancellationToken = default);
    public Task<IEnumerable<Payment>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task<Payment?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    public Task<Payment?> GetByOrderIdAsync(int orderId, CancellationToken cancellationToken = default);
    public Task<Payment?> GetByPaymentIntentIdAsync(string paymentIntentId, CancellationToken cancellationToken = default);
    public Task<Payment> UpdateAsync(Payment updatedPayment, CancellationToken cancellationToken = default);
    public Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}