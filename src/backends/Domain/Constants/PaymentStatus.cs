namespace Domain.Constants;

public static class PaymentStatus
{
    public const string Unknown = "Unknown";
    public const string Created = "Created";
    public const string Paid = "Paid";
    public const string Cancelled = "Cancelled";
}

public enum PaymentStatusCode
{
    Created = 1,
    Paid,
    Cancelled
}