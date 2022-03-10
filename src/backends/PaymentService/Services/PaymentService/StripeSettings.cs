namespace PaymentService.Services.PaymentService;

public class StripeSettings
{
    public string SecretKey { get; set; }
    public string WebHookSecretKey { get; set; }
    public string PublicKey { get; set; }
    public string SuccessUrl { get; set; }
    public string CancelUrl { get; set; }
}