using AdminDashBoard.Models;
using AutoMapper;
using Talabat.APIs.Helper;
using Talabat.Core.Entities;

namespace AdminDashBoard.Helper
{
    public class MappsProfile:Profile
    {
        public MappsProfile()
        {
            CreateMap<Product, ProductViewModel>().ReverseMap();
              //   .ForMember(d => d.ProductType, o => o.MapFrom(s => s.Type.Name))
              //.ForMember(d => d.ProductBrand, o => o.MapFrom(s => s.Brand.Name));
             
        }
    }
}
