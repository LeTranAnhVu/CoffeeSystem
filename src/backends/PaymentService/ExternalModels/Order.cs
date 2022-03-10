using System.ComponentModel.DataAnnotations;
using Domain.Constants;

namespace PaymentService.ExternalModels;

public class Order
{
    public int Id { get; set; }
    public string OrderedBy { get; set; }
    public string StatusName { get; set; }
    public OrderStatusCode StatusCode { get; set; }
    public DateTime OrderedAt { get; set; }
    public IReadOnlyList<Product> Products { get; set; }
    public double? TotalPrice { get; set; }
}