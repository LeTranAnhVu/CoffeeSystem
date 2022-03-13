using Domain.Constants;
using PaymentService.Constants;
using PaymentService.Models;
using Order = PaymentService.ExternalModels.Order;

namespace PaymentService.Services.PaymentService;

public interface IPaymentService
{
    public Task<CheckoutSession> CreateCheckoutSessionAsync(Order order, CancellationToken cancellationToken);
    public string GetPublicKey();
    public bool CanProcessPaymentAsync(Order order, string userEmail, CancellationToken cancellationToken)
    {
        /*
         * To allow payment we need:
         * - Order existed, on created state
         * - User owns the order
         */
        if (order.StatusCode == OrderStatusCode.Created &&
            order.OrderedBy == userEmail
           )
        {
            return true;
        }

        return false;
    }
    public Task ParseWebhookEventAsync<T>(T parseOptions, CancellationToken cancellationToken);

    public Task<IEnumerable<Payment>> GetAllAsync(CancellationToken cancellationToken);
    public Task<Payment?> GetById(int id, CancellationToken cancellationToken);
}