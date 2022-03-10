namespace PaymentService.Exceptions;

public class InvalidPaymentWebhookOptionException : Exception
{
    public InvalidPaymentWebhookOptionException(string message) : base(message)
    {
    }
}