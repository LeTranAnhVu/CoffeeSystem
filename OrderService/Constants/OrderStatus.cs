namespace OrderService.Constants;

public static class OrderStatus
{
    public const string Ordered = "Ordered";
    public const string Preparing = "Preparing";
    public const string Ready = "Ready";
    public const string Cancelled = "Cancelled";
}

public enum OrderStatusCode
{
    Ordered = 1,
    Preparing,
    Ready,
    Cancelled
}
