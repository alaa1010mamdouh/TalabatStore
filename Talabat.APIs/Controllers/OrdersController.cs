using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System.Security.Claims;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.Core;
using Talabat.Core.Entities.OrderAggregate;
using Talabat.Core.Services;
using Order = Talabat.Core.Entities.OrderAggregate.Order;

namespace Talabat.APIs.Controllers
{
 

    public class OrdersController : ApiBaseController
    {
        private readonly IorderService _orderService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public OrdersController(IorderService orderService,IMapper mapper,IUnitOfWork unitOfWork)
        {
            _orderService = orderService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        //Create Order
        [ProducesResponseType(typeof(Order),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status400BadRequest)]
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrderAsync(OrderDto orderDto )
        {
            var BuyerEmail=User.FindFirstValue(ClaimTypes.Email);
            var MappedAddress = _mapper.Map<AddressDto, Address>(orderDto.ShippingAddress);
       var Order=   await _orderService.CreateOrderAsync(BuyerEmail,orderDto.BasketId,orderDto.DeliveryMethodId, MappedAddress);

            if (Order is null)
            {
                return BadRequest(new ApiResponse(400, "problen in your order"));
            }
            return Ok(Order);
        }
        [ProducesResponseType(typeof(IReadOnlyList<OrderToReturnDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersForUser()
        {
            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var Orders=await _orderService.GetOrderForSpecificationAsync(BuyerEmail);
            if (Orders is null)
            {
                return NotFound(new ApiResponse(400, "there is no orders for this User"));

            }
            var mapped=_mapper.Map<IReadOnlyList<Order>, OrderToReturnDto>(Orders);
            return Ok(mapped);
                
                
        }
        [ProducesResponseType(typeof(IReadOnlyList<OrderToReturnDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderById(int id)
        {
            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
         var order=  await _orderService.GetOrderByIdForSpecificationAsync(BuyerEmail, id);
            if (order is null)
            {
                return NotFound(new ApiResponse(404, "Order Not Found for this id"));
            }
            var mapped=_mapper.Map<Order,OrderToReturnDto>(order);
                return Ok(mapped);
        }

        [HttpGet("DeliveryMethod")]
        public async Task<ActionResult<DeliveryMethod>> GetDeliveryMethod()
        {
          var Delivery=await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
          return Ok(Delivery);
        }

    }
}
