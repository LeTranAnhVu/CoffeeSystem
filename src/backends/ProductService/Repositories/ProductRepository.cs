using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductService.Dtos;
using ProductService.Exceptions;
using ProductService.Models;

namespace ProductService.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ProductRepository> _logger;

        public ProductRepository(AppDbContext context, ILogger<ProductRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Products.ToListAsync(cancellationToken: cancellationToken);
        }

        public async Task<IEnumerable<Product>> GetProductsByIds(IReadOnlyList<int> productIds, CancellationToken cancellationToken = default)
        {
            if (!productIds.Any())
            {
                return new List<Product>{};
            }

            return await _context.Products.Where(product => productIds.Contains(product.Id)).ToListAsync(cancellationToken: cancellationToken);
        }

        public async Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<Product> CreateAsync(Product product, CancellationToken cancellationToken)
        {
            await _context.Products.AddAsync(product, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return product;
        }

        public async Task UpdateOneAsync(Product product, CancellationToken cancellationToken)
        {
            try
            {
                _context.Products.Update(product);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch(Exception e)
            {
               _logger.LogError($"Cannot update product in {nameof(UpdateOneAsync)} : {e.Message}");
               throw;
            }
        }

        public async Task DeleteOneAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                var product = await GetByIdAsync(id, cancellationToken);
                if (product is null)
                {
                    throw new ProductNotFoundException(id);
                }

                _context.Products.Remove(product);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch(Exception e)
            {
                _logger.LogError($"Cannot delete product in {nameof(DeleteOneAsync)} : {e.Message}");
                throw;
            }
        }
    }
}