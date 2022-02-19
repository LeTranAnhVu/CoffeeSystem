using OrderService.ExternalModels;

namespace OrderService.Models;

public class OrderedProduct
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int ProductId { get; set; }
}