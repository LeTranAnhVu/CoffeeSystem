namespace PaymentService.Exceptions;

public class InvalidPaymentWebhookEventException : Exception
{
    public InvalidPaymentWebhookEventException(string message) : base(message)
    {
    }
}