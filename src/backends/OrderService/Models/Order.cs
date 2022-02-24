using System.ComponentModel.DataAnnotations;
using Domain.Constants;
namespace OrderService.Models;

public class Order
{
    public int Id { get; set; }
    [EmailAddress]
    public string OrderedBy { get; set; }
    public virtual string StatusName
    {
        get
        {
            return StatusCode switch
            {
                OrderStatusCode.Ordered => OrderStatus.Ordered,
                OrderStatusCode.Preparing => OrderStatus.Preparing,
                OrderStatusCode.Ready => OrderStatus.Ready,
                _ => OrderStatus.Cancelled,
            };
        }
    }
    public OrderStatusCode StatusCode { get; set; }
    public DateTime OrderedAt { get; set; }
    public virtual IReadOnlyList<OrderedProduct> OrderedProducts { get; set; }
}