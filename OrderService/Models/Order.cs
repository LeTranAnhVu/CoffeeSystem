namespace OrderService.Models;

public class Order
{
    public int Id { get; set; }
    public int OrderedBy { get; set; }
    public string Status { get; set; }
    public DateTime OrderedAt { get; set; }
    public virtual IReadOnlyList<OrderedProduct> OrderedProducts { get; set; }
}