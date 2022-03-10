using System.Security.Cryptography.X509Certificates;
using Stripe;
using Microsoft.Extensions.Options;
using PaymentService.Constants;
using PaymentService.Exceptions;
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

    public class StripePaymentService : BasePaymentService
    {
        private readonly string _paymentWebhookSecret;
        private readonly string _successUrl;
        private readonly string _cancelUrl;
        private readonly string _publicKey;

        // TODO Currency need to be dynamic in future
        private const string Currency = "usd";

        public StripePaymentService(IOptions<StripeSettings> stripeSettings)
        {
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

        public override CheckoutSession CreateCheckoutSession(ClientOrder order)
        {
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
            };

            var service = new SessionService();
            Session session =
                service.Create(options, new RequestOptions() {IdempotencyKey = CreateIdempotencyKey(order)});
            
            
            // Update payment row in database
            // ...
            Console.WriteLine("create session id {0}", session.Id);
            Console.WriteLine("create payment intent id {0}", session.PaymentIntentId); 
            return new CheckoutSession() { SessionId = session.Id };
        }

        public override string GetPublicKey()
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

        public override async Task ParseWebhookEventAsync<T>(T parseOptions,
            CancellationToken cancellationToken)
        {
            if (parseOptions is StripeWebhookParseOptions stripeOptions)
            {
                await InternalParseWebhookEventAsync(stripeOptions, cancellationToken);
            }

            throw new InvalidPaymentWebhookOptionException("Invalid Stripe webhook parse options");
        }

        private async Task InternalParseWebhookEventAsync(StripeWebhookParseOptions options, CancellationToken cancellationToken)
        {
            var json = await new StreamReader(options.RequestBody).ReadToEndAsync();
            var stripeEvent = EventUtility.ConstructEvent(json, options.SignatureHeader, _paymentWebhookSecret);

            if (stripeEvent.Type == "checkout.session.completed")
            {
                var session = stripeEvent.Data.Object as Stripe.Checkout.Session;
                Console.WriteLine($"Session ID: {session.Id}");
                // Take some action based on session.
            }

            if (stripeEvent.Type == Events.PaymentIntentSucceeded)
            {
                var intent = stripeEvent.Data.Object as Stripe.PaymentIntent;
                Console.WriteLine($"Payment Intent ID: {intent.Id}");
                // Take some action based on session.
            }
           
        }

        private long ConvertPriceToUnitPayment(double price)
        {
            return Convert.ToInt64(price * 100);
        }
    }
}