using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.OrderAggregate;

namespace Talabat.Core.Services
{
    public interface IpaymentService
    {
        //create or update paymentId

        Task<CustomerBasket> createOrUpdatepaymentIntent(string BasketId);
        Task<Order> updatepaymentintent(string paymentintentId, bool flag);
    }
}
