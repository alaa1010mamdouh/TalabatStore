using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;
using Talabat.Core.Services;

namespace Talabat.APIs.Helper
{
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _expireTimeInSecond;
       

        public CachedAttribute(int ExpireTimeInSecond )
        {
            _expireTimeInSecond = ExpireTimeInSecond;
         
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
         {
            var cachservice = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();
            var CachKey = GenerateCacheKeyfromRequest(context.HttpContext.Request);
            var cachedResponse = await cachservice.GetCachedResponse(CachKey);

            if (!string.IsNullOrEmpty(cachedResponse))
            {
                var contentResult = new ContentResult()
                {
                    Content = cachedResponse,
                    ContentType = "application/json",
                    StatusCode = 200
                };
                context.Result = contentResult;
                return;

            }

              var ExecutedEndPointContext=    await  next.Invoke();
            if (ExecutedEndPointContext.Result is OkObjectResult result)
            {
           await  cachservice.CacheResponse(CachKey,result.Value,TimeSpan.FromSeconds(_expireTimeInSecond));
                
            }

        }

        private string GenerateCacheKeyfromRequest(HttpRequest request)
        {
            var KeYBuilder=new StringBuilder();
            KeYBuilder.Append(request.Path);
            foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
            {
                KeYBuilder.Append($"|{key}-{value}");
            }
            return KeYBuilder.ToString();
        }
    }
}
