using AutoMapper;
using ProductService.Dtos;
using ProductService.Models;

namespace ProductService.Profiles
{
    public class ProductServiceProfile : Profile
    {
        public ProductServiceProfile()
        {
            // Source --> Target
            CreateMap<ProductWriteDto, Product>();
        }
    }
}