 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.OrderAggregate;

namespace Talabat.Core.Specification.Order_Spec
{
    public class OrderSpecification:BaseSpcification<Order>
    {

        public OrderSpecification(string email):base(p=>p.BuyerEmail==email)
        {
            Includes.Add(o => o.DeliveryMethod);
            Includes.Add(p => p.Items);
           SetOrderByDescending(o=>o.OrderDate);
      
        }
        public OrderSpecification(string email, int Id):base(p => p.Id == Id && p.BuyerEmail == email)
        {
            Includes.Add(o => o.DeliveryMethod);
            Includes.Add(p => p.Items);
        }
    }
}
