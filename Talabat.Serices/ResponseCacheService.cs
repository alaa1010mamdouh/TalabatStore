using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Services;

namespace Talabat.Serices
{
    public class ResponseCacheService : IResponseCacheService
    {
        private readonly IDatabase _database;
        public ResponseCacheService(IConnectionMultiplexer Redis )
        {
            _database=Redis.GetDatabase();
        }
        public async Task CacheResponse(string CacheKey, object Response, TimeSpan ExpireTime)
        {
            if (Response is null) return;
            var options=new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            //object toooooooo string
            var SerializeResponse = JsonSerializer.Serialize(Response,options);
            await _database.StringSetAsync(CacheKey, SerializeResponse, ExpireTime);
        }

        public async Task<string?> GetCachedResponse(string CacheKey)
        {
         var CachedResponse=   await _database.StringGetAsync(CacheKey);

            if (CachedResponse.IsNullOrEmpty) return null;
            return CachedResponse;

        }
    }
}
