using PaymentService.Constants;
using Order = PaymentService.ExternalModels.Order;

namespace PaymentService.Services.PaymentService;

public interface IPaymentService
{
    public CheckoutSession CreateCheckoutSession(Order order);
    public string GetPublicKey();
    public bool CanProcessPaymentAsync(Order order, string userEmail, CancellationToken cancellationToken);
    public Task ParseWebhookEventAsync<T>(T parseOptions, CancellationToken cancellationToken);
}