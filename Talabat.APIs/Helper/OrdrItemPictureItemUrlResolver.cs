using AutoMapper;
using AutoMapper.Execution;
using Talabat.APIs.DTOs;
using Talabat.Core.Entities.OrderAggregate;

namespace Talabat.APIs.Helper
{
    public class OrdrItemPictureItemUrlResolver : IValueResolver<OrderItem, OrderItemDto, string>
    {
        private readonly IConfiguration _configuration;

        public OrdrItemPictureItemUrlResolver(IConfiguration configuration )
        {
           _configuration = configuration;
        }
        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.Product.PictureUrl)) 
            { 
             return $"{_configuration["BaseUrl"]}{source.Product.PictureUrl}";
            }

            return string.Empty ;
        }
    }
}
