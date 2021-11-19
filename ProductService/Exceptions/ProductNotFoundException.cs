using System;
using ProductService.Models;

namespace ProductService.Exceptions
{
    public class ProductNotFoundException : Exception
    {
        static string _defaultMessage = "Cannot found the product";
        public ProductNotFoundException(): base(_defaultMessage)
        {
        }

        public ProductNotFoundException(int productId): base($"{_defaultMessage} with id is {productId}")
        {
        }

        public ProductNotFoundException(string? message) : base(message)
        {
        }
    }
}