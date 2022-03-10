using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentService.Services.OrderService;
using PaymentService.Services.PaymentService;

namespace PaymentService.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class PaymentController : ControllerBase
{
    private readonly ILogger<PaymentController> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IPaymentService _paymentService;
    private readonly IOrderService _orderService;

    public PaymentController(
        ILogger<PaymentController> logger,
        IHttpContextAccessor httpContextAccessor,
        IPaymentService paymentService,
        IOrderService orderService
    )
    {
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
        _paymentService = paymentService;
        _orderService = orderService;
    }

    [HttpGet("paymentPublicKey")]
    public IActionResult GetPaymentPublicKey()
    {
        return Ok(new {PaymentPublicKey = _paymentService.GetPublicKey()});
    }

    [HttpPost("createCheckoutSession/{orderId}")]
    public async Task<IActionResult> CreateCheckoutSession(
        int orderId, 
        CancellationToken cancellationToken)
    {
        // Get user
        var user = _httpContextAccessor.HttpContext?.User;
        var userEmail = user?.FindFirst(ClaimTypes.Email)?.Value;

        if (string.IsNullOrWhiteSpace(userEmail))
        {
            return Unauthorized();
        }

        // Get order
        var order = await _orderService.GetOrderByIdAsync(orderId, cancellationToken);
        if (order is null)
        {
            return BadRequest("Order is not existed");
        }

        // Validate the order
        if (!_paymentService.CanProcessPaymentAsync(order, userEmail, cancellationToken))
        {
            return BadRequest($"Cannot process payment on the order with Id is {orderId.ToString()}");
        }

        // Create the checkout session
        var session = _paymentService.CreateCheckoutSession(order);
        return Ok(new {CheckoutSessionId = session.SessionId});
    }


    // This API should better be a Serverless service like Azure Function
    // Then we will use the buffer like storage queue or service bus to quickly store the event
    // Then we will need a web job to enqueue the event from payment provider (Stripe) and store the payment to database.
    // The above approach would make the payment never be missed when this payment service is unavailable.
    [AllowAnonymous]
    [HttpPost("StripeWebhook")]
    public async Task<IActionResult> StripeWebhook(CancellationToken cancellationToken)
    {
        try
        {
            await _paymentService.ParseWebhookEventAsync<StripeWebhookParseOptions>(new()
            {
                RequestBody = HttpContext.Request.Body,
                SignatureHeader = Request.Headers["Stripe-Signature"]
            }, cancellationToken);
          
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest();
        }
    }
}