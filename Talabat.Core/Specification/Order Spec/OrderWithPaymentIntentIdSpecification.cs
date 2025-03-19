using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.OrderAggregate;

namespace Talabat.Core.Specification.Order_Spec
{
    public class OrderWithPaymentIntentIdSpecification:BaseSpcification<Order>
    {
        public OrderWithPaymentIntentIdSpecification(string PaymentIntentId):base(o=>o.PaymentIntentId==PaymentIntentId) 
        {
            
        }
    }
}
