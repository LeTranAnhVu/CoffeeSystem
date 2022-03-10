using Domain.Constants;
using PaymentService.Constants;
using PaymentService.ExternalModels;

namespace PaymentService.Services.PaymentService;

public abstract class BasePaymentService : IPaymentService
{
    public abstract CheckoutSession CreateCheckoutSession(Order order);
    public abstract string GetPublicKey();
    public abstract Task ParseWebhookEventAsync<T>(T parseOptions, CancellationToken cancellationToken);
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
}