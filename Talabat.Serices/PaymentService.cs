using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.OrderAggregate;
using Talabat.Core.Repositores;
using Talabat.Core.Services;
using Talabat.Core.Specification.Order_Spec;
using Product = Talabat.Core.Entities.Product;

namespace Talabat.Serices
{
    public class PaymentService : IpaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IConfiguration configuration,IBasketRepository basketRepository,IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<CustomerBasket> createOrUpdatepaymentIntent(string BasketId)
        {
            StripeConfiguration.ApiKey = _configuration["StripeSetting:Secretkey"];
            //getbasket
            var Basket=await _basketRepository.GetBasketByIdAsync(BasketId);
            if (Basket is null) return null;
            //amount =subtotal + deliverymethod
            var Shippingprice = 0m;
            if (Basket.DeliveryMethodId.HasValue) 
            { 
                var Delivery=await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(Basket.DeliveryMethodId.Value);
                 Shippingprice=Delivery.Cost;
            }
            if (Basket.Items.Count > 0) {
                foreach (var item in Basket.Items) 
                { 
                var product =await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                    if(item.Price!=product.Price)
                        item.Price = product.Price;
                
                }
            
            }
            var Subtotal=Basket.Items.Sum(item=>item.Price* item.Quantity);

            var Service = new PaymentIntentService();
            PaymentIntent paymentIntent;
            if (string.IsNullOrEmpty(Basket.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions()
                {
                    Amount = (long)Subtotal * 100 + (long)Shippingprice * 100,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" }
                };

                paymentIntent = await Service.CreateAsync(options);
                Basket.PaymentIntentId = paymentIntent.Id;
                Basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else 
            {
                var optionss = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)Subtotal * 100 + (long)Shippingprice * 100
                };
              paymentIntent=await  Service.UpdateAsync(Basket.PaymentIntentId,optionss);
                Basket.PaymentIntentId = paymentIntent.Id;
                Basket.ClientSecret = paymentIntent.ClientSecret;
            }
           await  _basketRepository.UpdateBasketAsync(Basket);
             return Basket;
        }

        public async Task<Order> updatepaymentintent(string paymentintentId, bool flag)
        {
            var sec = new OrderWithPaymentIntentIdSpecification(paymentintentId);
            var order = await _unitOfWork.Repository<Order>().GetByEntityWithSpecification(sec);
            if (flag)
            {
                order.Status=OrderStatus.PaymentReceived;

            }
            else
            {
                order.Status = OrderStatus.PaymentFailed;
            }
            _unitOfWork.Repository<Order>().Update(order);
           await _unitOfWork.CompleteAsync();
            return order;
        }
    }
}
