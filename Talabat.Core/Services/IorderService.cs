using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.OrderAggregate;

namespace Talabat.Core.Services
{
    public interface IorderService
    {
        Task<Order?> CreateOrderAsync(string BuyerEmail, string BasketId, int DeliveryMethod, Address ShippingAddress);
        Task<IReadOnlyList<Order>> GetOrderForSpecificationAsync(string BuyerEmail);
        Task<Order?> GetOrderByIdForSpecificationAsync(string BuyerEmail,  int OrderId);
    }
}
