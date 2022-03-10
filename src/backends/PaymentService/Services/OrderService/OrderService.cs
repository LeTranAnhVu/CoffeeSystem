using Microsoft.Extensions.Options;
using PaymentService.ExternalModels;

namespace PaymentService.Services.OrderService;

public class OrderService : IOrderService
{
    private readonly string _url;
    private readonly HttpClient _orderHttpClient;

    public OrderService(IHttpClientFactory httpFactory, IOptions<OrderServiceSettings> orderSettings)
    {
        _url = orderSettings.Value.Url ?? throw new NullReferenceException("Cannot find the Order Service Endpoint");
        _orderHttpClient = httpFactory.CreateClient();
        _orderHttpClient.BaseAddress = new Uri(_url);
    }
    
    public async Task<Order?> GetOrderByIdAsync(int orderId,  CancellationToken cancellationToken)
    {
        var path = $"api/Orders/{orderId}?withPrice=true";
        var order = await _orderHttpClient.GetFromJsonAsync<Order>(path, cancellationToken);
        return order;
    }
}