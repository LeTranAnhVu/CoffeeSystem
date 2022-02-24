using Domain.Constants;

namespace SignalRService.Dtos;
public class OrderCreatedDto
{
    public int OrderId { get; set; }
    public string OrderedBy { get; set; }
}

public class OrderStatusChangedDto
{
    public int OrderId { get; set; }
    public OrderStatusCode StatusCode { get; set; }
    public string StatusName { get; set; }
    public string OrderedBy { get; set; }
}