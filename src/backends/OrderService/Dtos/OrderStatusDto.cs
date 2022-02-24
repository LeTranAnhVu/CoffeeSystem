using Domain.Constants;

namespace OrderService.Dtos;

public class OrderStatusDto
{
    public OrderStatusCode Code { get; set; }
    public string Name { get; set; }
}