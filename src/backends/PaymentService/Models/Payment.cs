using System.Diagnostics.CodeAnalysis;
using Domain.Constants;

namespace PaymentService.Models;

public class Payment
{
    public int Id { get; set; } 
    public string PaymentIntentId { get; set; } 
    public string? SessionPaymentId { get; set; } 
    public int OrderId { get; set; } 
    public string? PaymentProvider { get; set; } 
    public string? ReceiptEmail { get; set; } 
    public IEnumerable<string>? ReceiptUrls { get; set; } 
    public PaymentStatusCode StatusCode  { get; set; } 
    public virtual string StatusName
    {
        get
        {
            return StatusCode switch
            {
                PaymentStatusCode.Created => PaymentStatus.Created,
                PaymentStatusCode.Paid => PaymentStatus.Paid,
                _ => PaymentStatus.Unknown,
            };
        }
    }

    public string? Currency { get; set; }
    public long? Amount { get; set; }
    public long? AmountReceived { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? PaidAt { get; set; }
    public DateTime? CancelledAt { get; set; }
}