using OrderService.ExternalModels;

namespace OrderService.Models;

public class OrderedProduct
{
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public virtual Product Product { get; set; }
}