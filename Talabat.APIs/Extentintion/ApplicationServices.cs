using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using Talabat.APIs.Errors;
using Talabat.APIs.Helper;
using Talabat.Core;
using Talabat.Core.Repositores;
using Talabat.Core.Services;
using Talabat.Repository;
using Talabat.Serices;

namespace Talabat.APIs.Extentintion
{
    public static class ApplicationServices
    {
        public static IServiceCollection AddApplicationServices(this  IServiceCollection Services)
        {
            Services.AddSingleton<IResponseCacheService, ResponseCacheService>();
            Services.AddScoped<IpaymentService, PaymentService>();
            Services.AddScoped<IorderService, OrderService>();
            Services.AddScoped<IUnitOfWork, UnitOfWork>();
            Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            Services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));  
            Services.AddAutoMapper(typeof(MappingProfile));
            Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                           .Where(e => e.Value.Errors.Count > 0)
                           .SelectMany(x => x.Value.Errors)
                           .Select(x => x.ErrorMessage).ToArray();
                    var errorResponse = new ApiValidtionERrorResponse
                    {
                        Errors = errors

                    };
                    return new BadRequestObjectResult(errorResponse);
                };
            });
         
            return Services;

        }
    }
}
