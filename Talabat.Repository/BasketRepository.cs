using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repositores;

namespace Talabat.Repository
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase database;

        //ask clr to inject object from class that implement IConnectionMultiplexer
        public BasketRepository(IConnectionMultiplexer redis )
        {

            database = redis.GetDatabase();
        }
        public async Task<bool> DeleteBasketAsync(string basketId)
        {

            return await database.KeyDeleteAsync(basketId);
        }

        public async Task<CustomerBasket?> GetBasketByIdAsync(string basketId)
        {
          
            var Baket=await database.StringGetAsync(basketId);
            //if (Baket.IsNull)
            //{
            //    return null;
            //}
            //else
            //{
            //    var Returnbasket=JsonSerializer.Deserialize<CustomerBasket>(Baket);

            //    return Returnbasket;
            //}
            return Baket.IsNull ? null : JsonSerializer.Deserialize<CustomerBasket>(Baket);

        }

        public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket)
        {
            //object to json serilizaition
            //json to object deserilization
           
            var JsonBasket = JsonSerializer.Serialize(basket);
         var createdorUpdated=  await database.StringSetAsync(basket.Id,JsonBasket,TimeSpan.FromDays(1));

            if (!createdorUpdated)
            {
                return null;
            }
            return await GetBasketByIdAsync(basket.Id);


        }
    }
}
