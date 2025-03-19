using Microsoft.JSInterop;
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

namespace Talabat.Serices
{
    public class OrderService : IorderService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IpaymentService _paymentService;

        public OrderService(IBasketRepository basketRepository,IUnitOfWork unitOfWork,IpaymentService paymentService)
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
           _paymentService = paymentService;
        }
        public async Task<Order?> CreateOrderAsync(string BuyerEmail, string BasketId, int DeliveryMethod, Address ShippingAddress)
        {
            //1.Get Basket From Basket Repo
            var Basket=await _basketRepository.GetBasketByIdAsync(BasketId);
            //2.Get Selected Items at Basket From Product Repo
            var OrderItems=new List<OrderItem>();
            if (Basket?.Items.Count > 0)
            {
                foreach (var item in Basket.Items) { 
                var products=await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                var productitemordered=new ProductItemOrdered(products.Id, products.Name, products.PictureUrl);
                    var orderitem=new OrderItem(productitemordered,  item.Quantity,products.Price);
                    OrderItems.Add(orderitem);
                }

            }
            //3.Calculate SubTotal
            var subtotal = OrderItems.Sum(item => item.Price * item.Quantity);
            //4.Get Delivery Method From DeliveryMethod Repo

            var Deliverymethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(DeliveryMethod);
            //5.Create Order
            var Spec = new OrderWithPaymentIntentIdSpecification(Basket.PaymentIntentId);
            var ExOrder = _unitOfWork.Repository<Order>().GetByEntityWithSpecification(Spec);
            if (ExOrder is not null)
            {
                _unitOfWork.Repository<Order>().Delete(await ExOrder);
             await   _paymentService.createOrUpdatepaymentIntent(BasketId);
            }
            var order=new Order(BuyerEmail, ShippingAddress, Deliverymethod,OrderItems,subtotal,Basket.PaymentIntentId);
            //6.Add Order Locally
           await _unitOfWork.Repository<Order>().AddAsync(order);

            //7.Save Order To Database[ToDo]
         var result=  await _unitOfWork.CompleteAsync();
            if (result <= 0) return null;
            return order;

        }

        public Task<Order?> GetOrderByIdForSpecificationAsync(string BuyerEmail, int OrderId)
        {
            var Spec=new OrderSpecification(BuyerEmail, OrderId);
            var order = _unitOfWork.Repository<Order>().GetByEntityWithSpecification(Spec);
            return order;
        }

        public async Task<IReadOnlyList<Order>> GetOrderForSpecificationAsync(string BuyerEmail)
        {
            var Spec = new OrderSpecification(BuyerEmail);
            var Orders =await _unitOfWork.Repository<Order>().GetAllWithSpecification(Spec);
            return Orders;    
        }
    }
}
