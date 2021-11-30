using OrderService.Constants;

namespace OrderService.AsyncMessageContracts;

public class OrderCreatedContract
{
    public int OrderId { get; set; }
    public string OrderedBy { get; set; }
}

public class OrderStatusChangedContract
{
    public int OrderId { get; set; }
    public OrderStatusCode StatusCode { get; set; }
    public string OrderedBy { get; set; }
}

