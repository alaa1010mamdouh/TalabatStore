using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specification
{
    public class ProductWithBrandSpecification:BaseSpcification<Product>
    {
        //ctor for all product
        public ProductWithBrandSpecification(ProductSpecParam Params)
            :base(
                 x =>
                    (string.IsNullOrEmpty(Params.Search) || x.Name.ToLower().Contains(Params.Search))
                 &&
                    (! Params.BrandId.HasValue || x.ProductBrandId == Params.BrandId) &&
                    (!Params.TypeId.HasValue || x.ProductTypeId == Params.TypeId)


                 )
        {
           Includes.Add(x => x.ProductBrand);
            Includes.Add(x => x.ProductType);
            if (!string.IsNullOrEmpty(Params.Sort))
            {
                switch (Params.Sort)
                {
                    case "priceAsc":
                        SetOrderBy(x => x.Price);
                        break;
                    case "priceDesc":
                        SetOrderByDescending(x => x.Price);
                        break;
                    default:
                        SetOrderBy(x => x.Name);
                        break;
                }

            }

            //product=100
            //pagesize=10
            //pageindex=5

            //skip 40=10*4
            //take 10


            ApplyPagination(Params.PageSize * (Params.PageIndex - 1), Params.PageSize);
        }
        //ctor for product with id
        public ProductWithBrandSpecification(int id) : base(x => x.Id == id)
        {
            Includes.Add(x => x.ProductBrand);
            Includes.Add(x => x.ProductType);
        }

    }
}
