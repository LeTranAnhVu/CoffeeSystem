using Domain.Constants;
using Stripe;
using Microsoft.Extensions.Options;
using PaymentService.Exceptions;
using PaymentService.Models;
using PaymentService.Repositories;
using PaymentService.Services.OrderService;
using RabbitMqServiceExtension.AsyncMessageService;
using Stripe.Checkout;
using ClientOrder = PaymentService.ExternalModels.Order;
using Product = PaymentService.ExternalModels.Product;

namespace PaymentService.Services.PaymentService
{
    public class StripeWebhookParseOptions
    {
        public Stream RequestBody { get; set; }
        public string SignatureHeader { get; set; }
    }

    public class StripePaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepo;
        private readonly IRabbitMqService _mqService;
        private readonly ILogger<StripePaymentService> _logger;
        private readonly string _paymentWebhookSecret;
        private readonly string _successUrl;
        private readonly string _cancelUrl;
        private readonly string _publicKey;

        // TODO Currency need to be dynamic in future
        private const string Currency = "usd";

        public StripePaymentService(
            IOptions<StripeSettings> stripeSettings,
            IPaymentRepository paymentRepo,
            IRabbitMqService mqService,
            ILogger<StripePaymentService> logger)
        {
            _paymentRepo = paymentRepo;
            _mqService = mqService;
            _logger = logger;

            StripeConfiguration.ApiKey = stripeSettings.Value.SecretKey ??
                                         throw new NullReferenceException("Cannot find the api key for stripe");

            _paymentWebhookSecret = stripeSettings.Value.WebHookSecretKey ??
                                    throw new NullReferenceException("Cannot find the Webhook key for stripe");

            _publicKey = stripeSettings.Value.PublicKey ??
                         throw new NullReferenceException("Cannot find the public key for stripe");

            _successUrl = stripeSettings.Value.SuccessUrl ??
                          throw new NullReferenceException("Cannot find the SuccessUrl for stripe");

            _cancelUrl = stripeSettings.Value.CancelUrl ??
                         throw new NullReferenceException("Cannot find the CancelUrl for stripe");
        }

        public async Task<CheckoutSession> CreateCheckoutSessionAsync(ClientOrder order,
            CancellationToken cancellationToken)
        {
            // Validate the order in payment database first
            // - Check order is already in database or not
            var existedPayment = await _paymentRepo.GetByOrderIdAsync(order.Id, cancellationToken);

            if (existedPayment is not null)
            {
                // Just return the in-process session
                if (existedPayment.StatusCode == PaymentStatusCode.Created)
                {
                    return new CheckoutSession() {SessionId = existedPayment.SessionPaymentId};
                }

                // Duplicated payment
                throw new DuplicatedCheckoutSessionException(
                    "Cannot create the payment session for processed order Id");
            }

            // Create checkout session
            // TODO quantity should be place in order service in future
            int quantity = 1;
            List<SessionLineItemOptions> lineItems =
                order.Products.Select(p => CreateItemOptionFromOrderProduct(p, quantity)).ToList();

            var options = new SessionCreateOptions
            {
                LineItems = lineItems,
                Mode = "payment",
                SuccessUrl = _successUrl,
                CancelUrl = _cancelUrl,
                PaymentIntentData = new SessionPaymentIntentDataOptions {Metadata = new Dictionary<string, string> {{"OrderId", order.Id.ToString()}}}
            };

            var idempotencyKey = CreateIdempotencyKey(order);

            var service = new SessionService();
            var session = await service.CreateAsync(options, new RequestOptions() {IdempotencyKey = idempotencyKey},
                cancellationToken);

            // Create new payment in database
            var payment = new Payment
            {
                PaymentProvider = PaymentProviders.Stripe,
                OrderId = order.Id,
                StatusCode = PaymentStatusCode.Created,
                SessionPaymentId = session.Id,
                PaymentIntentId = session.PaymentIntentId,
                Amount = session.AmountTotal ?? 0
            };

            await _paymentRepo.CreateAsync(payment, cancellationToken);
            return new CheckoutSession() {SessionId = session.Id};
        }

        public string GetPublicKey()
        {
            return _publicKey;
        }

        public SessionLineItemOptions CreateItemOptionFromOrderProduct(Product product, int quantity)
        {
            return new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = ConvertPriceToUnitPayment(product.Price),
                    Currency = Currency,
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = product.Name,
                        // Images = new List<string>() {}
                    },
                },
                Quantity = quantity,
            };
        }

        private string CreateIdempotencyKey(ClientOrder order)
        {
            return $"orderId_{order.Id}";
        }

        public async Task ParseWebhookEventAsync<T>(T parseOptions,
            CancellationToken cancellationToken)
        {
            if (parseOptions is StripeWebhookParseOptions stripeOptions)
            {
                await InternalParseWebhookEventAsync(stripeOptions, cancellationToken);
            }
            else
            {
                throw new InvalidPaymentWebhookOptionException("Invalid Stripe webhook parse options");
            }
        }

        public async Task<IEnumerable<Payment>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _paymentRepo.GetAllAsync(cancellationToken);
        }

        public async Task<Payment?> GetById(int id, CancellationToken cancellationToken)
        {
            return await _paymentRepo.GetByIdAsync(id, cancellationToken);
        }

        private async Task InternalParseWebhookEventAsync(
            StripeWebhookParseOptions options,
            CancellationToken cancellationToken)
        {
            var json = await new StreamReader(options.RequestBody).ReadToEndAsync();
            var stripeEvent = EventUtility.ConstructEvent(json, options.SignatureHeader, _paymentWebhookSecret);
            var intent = stripeEvent.Data.Object as Stripe.PaymentIntent;

            // When payment is paid
            if (stripeEvent.Type == Events.PaymentIntentSucceeded)
            {
                // Get payment and validate
                var payment = await _paymentRepo.GetByPaymentIntentIdAsync(intent?.Id, cancellationToken);
                var orderId = Convert.ToInt32(intent.Metadata["OrderId"]);
                if (payment is null)
                {
                    // Create new payment if the payment is not existed.
                    payment = new Payment()
                    {
                        PaymentProvider = PaymentProviders.Stripe,
                        OrderId = orderId,
                        StatusCode = PaymentStatusCode.Created,
                        PaymentIntentId = intent.Id,
                        Amount = intent.Amount,
                        CreatedAt = DateTime.Now
                    };
                }
                
                // Update the payment when it is paid
                payment.Currency = intent.Currency;
                payment.AmountReceived = intent.AmountReceived;
                payment.ReceiptEmail = intent.ReceiptEmail;
                payment.ReceiptUrls = intent.Charges.Select(c => c.ReceiptUrl).ToList();
                payment.PaidAt = DateTime.Now;
                payment.StatusCode = PaymentStatusCode.Paid;

                await _paymentRepo.UpdateAsync(payment, cancellationToken);
                _mqService.SendMessage("payment.paid", new {OrderId = orderId, PaymentId = payment.Id});
            }
            
            // When payment is cancelled
            if (stripeEvent.Type == Events.PaymentIntentCanceled)
            {
                var payment = await _paymentRepo.GetByPaymentIntentIdAsync(intent?.Id, cancellationToken);
                if (payment is not null)
                {
                    // Can delete payment when it is unpaid
                    if (payment.StatusCode == PaymentStatusCode.Created)
                    {
                        await _paymentRepo.DeleteAsync(payment.Id, cancellationToken);
                    }
                }
            }
        }


        private long ConvertPriceToUnitPayment(double price)
        {
            return Convert.ToInt64(price * 100);
        }
    }
}