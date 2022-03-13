using System.ComponentModel.DataAnnotations;
using Domain.Constants;
using OrderService.ExternalModels;

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
                OrderStatusCode.Created => OrderStatus.Created,
                OrderStatusCode.Paid => OrderStatus.Paid,
                OrderStatusCode.Ordered => OrderStatus.Ordered,
                OrderStatusCode.Preparing => OrderStatus.Preparing,
                OrderStatusCode.Ready => OrderStatus.Ready,
                OrderStatusCode.Cancelled => OrderStatus.Cancelled,
                _ => OrderStatus.Unknown,
            };
        }
    }
    public OrderStatusCode StatusCode { get; set; }
    public DateTime OrderedAt { get; set; }
    public virtual IReadOnlyList<OrderedProduct> OrderedProducts { get; set; }

    public virtual IReadOnlyList<Product>? Products { get; set; }
    public virtual double? TotalPrice {
        get
        {
            if (Products is null )
            {
                return null;
            }

            var sum = Products.Select(p => p.Price).Sum();
            return sum;
        }
    }
}