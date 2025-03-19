using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specification
{
    public class ProductWithFiltrationForCountAsync: BaseSpcification<Product>
    {
        public ProductWithFiltrationForCountAsync(ProductSpecParam Param):base(x =>
            (string.IsNullOrEmpty(Param.Search) || x.Name.ToLower().Contains(Param.Search)) 
        &&

            (!Param.BrandId.HasValue || x.ProductBrandId == Param.BrandId) &&
            (!Param.TypeId.HasValue || x.ProductTypeId == Param.TypeId)
            )
        {
            
        }
    }
}
