using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ProductService.Models;

namespace ProductService.Repositories
{
    public interface IProductRepository
    {
        public Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken = default);
        public Task<Product?> GetByIdAsync(int productId, CancellationToken cancellationToken = default);
        public Task<Product> CreateAsync(Product product, CancellationToken cancellationToken = default);
        public Task UpdateOneAsync(Product product, CancellationToken cancellationToken = default);
        public Task DeleteOneAsync(int id, CancellationToken cancellationToken = default);
    }
}