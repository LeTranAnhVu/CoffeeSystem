namespace OrderService.Services.ProductService;

public interface IProductService
{
    public Task<IReadOnlyList<ExternalModels.Product>> GetProductsByIds(IReadOnlyList<int> productIds,
        CancellationToken cancellationToken = default);
}