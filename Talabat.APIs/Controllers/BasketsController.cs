using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Repositores;

namespace Talabat.APIs.Controllers
{

    public class BasketsController : ApiBaseController
    {
        private readonly IBasketRepository _basketRepo;
        private readonly IMapper _mapper;

        public BasketsController(IBasketRepository basketRepo,IMapper mapper)
        {
            _basketRepo = basketRepo;
            _mapper = mapper;
        }
        //Get or ReCreate Basket
        [HttpGet]
        public async Task<ActionResult<CustomerBasket>> GetCustomerBasket( string id)
        {
            var basket=await _basketRepo.GetBasketByIdAsync(id);

            if (basket is null) return new CustomerBasket(id);

            return Ok(basket);
            
            
        }


        //Update OR Create New Basket
        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDyo basket)
        {
            var mapped=_mapper.Map<CustomerBasketDyo, CustomerBasket>(basket);
            var CreatedOrUpdated=   await _basketRepo.UpdateBasketAsync(mapped);

            if (CreatedOrUpdated is null)
            {
                return BadRequest(new ApiResponse(400) );
            }
            return Ok(CreatedOrUpdated);

        }


        //Delete Basket
        [HttpDelete]

        public async Task<ActionResult<bool>> DeleteBasket( string Basketid)
        {
          return  await _basketRepo.DeleteBasketAsync(Basketid);

        }


    }
}
