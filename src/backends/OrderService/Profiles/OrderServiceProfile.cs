using AutoMapper;
using OrderService.Dtos;
using OrderService.Models;

namespace OrderService.Profiles;

public class OrderServiceProfile : Profile
{
    public OrderServiceProfile()
    {
        // Source -> Target
        CreateMap<OrderWriteDto, Order>();
    }
}