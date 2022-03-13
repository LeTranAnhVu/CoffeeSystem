namespace PaymentService.Exceptions;

public class DuplicatedCheckoutSessionException : Exception
{
    public DuplicatedCheckoutSessionException(string message) : base(message)
    {
    }
}