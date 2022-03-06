using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PaymentService.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class PaymentController : ControllerBase
{
    private readonly ILogger<PaymentController> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PaymentController(
        ILogger<PaymentController> logger,
        IHttpContextAccessor httpContextAccessor
        )
    {
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpPost("{orderId}")]
    public async Task<ActionResult> CreatePaymentIntent(int orderId)
    {
        // Get user
        var user = _httpContextAccessor.HttpContext?.User;

        var userEmail = user?.FindFirst(ClaimTypes.Email)?.Value;

        if (string.IsNullOrWhiteSpace(userEmail)) return Unauthorized();

        // Validate the order
        if (!CanProcessPayment(orderId))
        {
            return BadRequest($"Cannot process payment on the order with Id is {orderId}");
        }

        // Create the payment intent
        // StripeService
        var clientSecret = StripeCreatePaymentIntent(orderId);

        return Ok(new { ClientSecret = clientSecret });
    }

    private string StripeCreatePaymentIntent(int orderId)
    {
        return "."
    }

    private bool CanProcessPayment(int orderId)
    {
        throw new NotImplementedException();
    }
}