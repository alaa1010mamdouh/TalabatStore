using Stripe;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.Core.Services;



namespace Talabat.APIs.Controllers
{
    public class PaymentController : ApiBaseController
    {
        private readonly IpaymentService _paymentService;
        private const string endpointSecret= "whsec_464044a1f190685452c079c3b91c76ec598572a047084bf2e71926add014e387";
        public PaymentController(IpaymentService paymentService)
        {
            _paymentService = paymentService;
        }
        [HttpPost("{basketId}")]
        public async Task<ActionResult<CustomerBasketDyo>> CreateOrUpdateIntent(string basketId)
        {
            var Basket = await _paymentService.createOrUpdatepaymentIntent(basketId);
            if (Basket is null) return BadRequest(new ApiResponse(400, "Problem with your basket"));
            return Ok(Basket);
        }

        [HttpPost("webhook")]//post =>base/api/payment/webhook
        public async Task<IActionResult> Stripeweb()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {

                var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], endpointSecret);
                var paymentIntent = (PaymentIntent)stripeEvent.Data.Object;

                //if (stripeEvent.Type == Stripe.Events.PaymentIntentSucceeded)
                {
                    await _paymentService.updatepaymentintent(paymentIntent.Id, true);
                }
               // else if (stripeEvent.Type == Stripe.Events.PaymentIntentPaymentFailed)
                {
                    await _paymentService.updatepaymentintent(paymentIntent.Id, false);
                }
              
                return Ok();
               
            }
            catch (StripeException e)
            {
                return BadRequest();
            }



        }
    }
}
