using AutoMapper;
using Talabat.APIs.DTOs;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Entities.OrderAggregate;

namespace Talabat.APIs.Helper
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(d=>d.ProductType,o=>o.MapFrom(s=>s.ProductType.Name))
              .ForMember(d => d.ProductBrand, o => o.MapFrom(s => s.ProductBrand.Name))
              .ForMember(d=>d.PictureUrl,o=>o.MapFrom<ProductPicyureResolver>());
           
            CreateMap<address, AddressDto>();
            CreateMap<CustomerBasketDyo, CustomerBasket>();
            CreateMap<BasketItemDto, BasketItem>();
            CreateMap<AddressDto, Address>();
            CreateMap<Order, OrderToReturnDto>()
                .ForMember(k => k.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod.ShortName))
                .ForMember(k => k.DeliveryMethodCost, o => o.MapFrom(s => s.DeliveryMethod.Cost));
            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d => d.ProductId, o => o.MapFrom(n => n.Product.ProductId))
                .ForMember(d => d.ProductName, o => o.MapFrom(n => n.Product.ProductName))
                .ForMember(d => d.PictureUrl, o => o.MapFrom(n => n.Product.PictureUrl))
                .ForMember(c => c.PictureUrl ,o => o.MapFrom<OrdrItemPictureItemUrlResolver>());
        }
    }
}
