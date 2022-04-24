using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace UrlShortenerService.Cache
{
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache _cache;

        public CacheService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var value = await _cache.GetStringAsync(key);

            if (value != null)
                return JsonConvert.DeserializeObject<T>(value);

            return default;
        }

        public async Task<T> SetAsync<T>(string key, T value)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(3),
                SlidingExpiration = TimeSpan.FromMinutes(30)
            };

            await _cache.SetStringAsync(key, JsonConvert.SerializeObject(value), options);

            return value;
        }
    }
}