using OrderService.Constants;

namespace OrderService.Models;

public class Order
{
    public int Id { get; set; }
    public int OrderedBy { get; set; }
    public virtual string StatusName
    {
        get{
            switch (StatusCode)
            {
                case OrderStatusCode.Ordered:
                    return OrderStatus.Ordered;
                case OrderStatusCode.Preparing:
                    return OrderStatus.Preparing;
                case OrderStatusCode.Ready:
                    return OrderStatus.Ready;
                default:
                    return OrderStatus.Cancelled;
            }
        }
    }
    public OrderStatusCode StatusCode { get; set; }
    public DateTime OrderedAt { get; set; }
    public virtual IReadOnlyList<OrderedProduct> OrderedProducts { get; set; }
}