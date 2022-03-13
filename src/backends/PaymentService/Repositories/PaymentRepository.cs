using Microsoft.EntityFrameworkCore;
using PaymentService.Models;

namespace PaymentService.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly AppDbContext _context;

    public PaymentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Payment> CreateAsync(Payment payment, CancellationToken cancellationToken = default)
    {
        // Update datetime
        payment.CreatedAt = DateTime.Now;
        await _context.Payments.AddAsync(payment, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return payment;
    }

    public async Task<IEnumerable<Payment>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Payments.ToListAsync(cancellationToken);
    }

    public async Task<Payment?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Payments.FirstOrDefaultAsync(p => p.Id == id, cancellationToken: cancellationToken);
    }

    public async Task<Payment?> GetByOrderIdAsync(int orderId, CancellationToken cancellationToken = default)
    {
        return await _context.Payments.FirstOrDefaultAsync(p => p.OrderId == orderId,
            cancellationToken: cancellationToken);
    }

    public async Task<Payment?> GetByPaymentIntentIdAsync(string paymentIntentId, CancellationToken cancellationToken = default)
    {
        return await _context.Payments.FirstOrDefaultAsync(p => p.PaymentIntentId == paymentIntentId,
            cancellationToken: cancellationToken);
    }

    public async Task<Payment> UpdateAsync(Payment updatedPayment, CancellationToken cancellationToken = default)
    {
        updatedPayment.UpdatedAt = DateTime.Now;
        _context.Update(updatedPayment);
        await _context.SaveChangesAsync(cancellationToken);
        return updatedPayment;
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var payment = await GetByIdAsync(id, cancellationToken);
        if (payment is not null)
        {
            _context.Payments.Remove(payment);
        }
    }
}