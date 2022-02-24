namespace Domain.Constants;

public static class OrderStatus
{
    public const string Created = "Created";
    public const string Paid = "Paid";
    public const string Ordered = "Ordered";
    public const string Preparing = "Preparing";
    public const string Ready = "Ready";
    public const string Cancelled = "Cancelled";
}

public enum OrderStatusCode
{
    Created = 1,
    Paid,
    Ordered,
    Preparing,
    Ready,
    Cancelled
}
