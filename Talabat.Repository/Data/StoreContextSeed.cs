using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.OrderAggregate;

namespace Talabat.Repository.Data
{
    public static class StoreContextSeed
    {
        //seeding
        public static async Task SeedAsync(StoreContext dbContext)
        {
            if (!dbContext.ProductBrands.Any())
            {


                var brandData = File.ReadAllText("../Talabat.Repository/DataSeed/brands.json");
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandData);
                if (brands?.Count > 0)
                {
                    foreach (var item in brands)
                    
                        dbContext.ProductBrands.Add(item);
                    
                    await dbContext.SaveChangesAsync();
                }
            }



            if (!dbContext.ProductTypes.Any())
            {


                var typeData = File.ReadAllText("../Talabat.Repository/DataSeed/types.json");
                var types = JsonSerializer.Deserialize<List<ProductType>>(typeData);
                if (types?.Count > 0)
                {

                    foreach (var type in types)
                    {

                        dbContext.ProductTypes.Add(type);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }


            if (!dbContext.Products.Any())
            {


                var productData = File.ReadAllText("../Talabat.Repository/DataSeed/products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productData);
                if (products?.Count > 0)
                {
                    foreach (var product in products)
                    
                        dbContext.Products.Add(product);
                    
                    await dbContext.SaveChangesAsync();
                }
            }

            if (!dbContext.DeliveryMethods.Any())
            {
                var DeliveryMethodsData = File.ReadAllText("../Talabat.Repository/DataSeed/delivery.json");
                var DeliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliveryMethodsData);
                if (DeliveryMethods?.Count > 0)
                {
                    foreach (var DeliveryMethod in DeliveryMethods)

                      await  dbContext.Set<DeliveryMethod>().AddAsync(DeliveryMethod);

                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
