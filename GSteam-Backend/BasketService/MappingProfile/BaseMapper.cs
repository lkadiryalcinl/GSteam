using AutoMapper;
using BasketService.Models;
using Contracts;

namespace BasketService.MappingProfile
{
    public class BaseMapper : Profile
    {
        public BaseMapper()
        {
            CreateMap<CheckoutModel, CheckoutBasketModel>().ReverseMap();
            CreateMap<CheckoutModel, BasketModel>().ReverseMap();
        }
    }
}
