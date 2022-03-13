using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentService.Models;
using PaymentService.Services.OrderService;
using PaymentService.Services.PaymentService;
using RabbitMqServiceExtension.AsyncMessageService;

namespace PaymentService.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class PaymentsController : ControllerBase
{
    private readonly ILogger<PaymentsController> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IPaymentService _paymentService;
    private readonly IOrderService _orderService;
    private readonly IRabbitMqService _mqService;

    public PaymentsController(
        ILogger<PaymentsController> logger,
        IHttpContextAccessor httpContextAccessor,
        IPaymentService paymentService,
        IOrderService orderService,
        IRabbitMqService mqService)
    {
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
        _paymentService = paymentService;
        _orderService = orderService;
        _mqService = mqService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Payment>>> GetAll(CancellationToken cancellationToken)
    {
        var payments = await _paymentService.GetAllAsync(cancellationToken);
        _mqService.SendMessage("payment.paid", new {OrderId = 1, PaymentId = 0});
        return Ok(payments);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<IEnumerable<Payment>>> GetOne(int id, CancellationToken cancellationToken)
    {
        var payment = await _paymentService.GetById(id, cancellationToken);

        return payment is null ? NotFound() : Ok(payment);
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
        var session = await _paymentService.CreateCheckoutSessionAsync(order, cancellationToken);
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
            _logger.LogError("Strip webhook fail");
            _logger.LogError(e.Message);
            return BadRequest();
        }
    }
}