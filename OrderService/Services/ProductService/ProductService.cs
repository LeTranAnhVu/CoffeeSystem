using System.Web;
using Microsoft.Extensions.Options;
using OrderService.ExternalModels;

namespace OrderService.Services.ProductService;
public class ProductService : IProductService
{
    private readonly string _url;
    private readonly HttpClient _productHttpClient;

    public ProductService(IHttpClientFactory httpFactory, IOptions<ProductServiceSettings> productServiceSettings)
    {
        _url = productServiceSettings.Value.Url ?? throw new NullReferenceException();
        _productHttpClient = httpFactory.CreateClient();
        _productHttpClient.BaseAddress = new Uri(_url);
    }
    public async Task<IReadOnlyList<Product>> GetProductsByIds(IReadOnlyList<int> productIds, CancellationToken cancellationToken = default)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        foreach (var productId in productIds)
        {
            query.Add("ids", productId.ToString());
        }

        query.Add("findByIds", true.ToString());
        var queryString = query.ToString();
        var path = "api/Products?" + queryString;
        var products = await _productHttpClient.GetFromJsonAsync<List<Product>>(path , cancellationToken);

        return products ?? new List<Product>();
    }
}