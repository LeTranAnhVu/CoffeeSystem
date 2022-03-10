namespace PaymentService.Constants;

public enum PaymentEventType
{
    Unknown = 1,
    PaymentCreated,
    PaymentSucceeded,
    PaymentFailed,
    PaymentCancelled,
}