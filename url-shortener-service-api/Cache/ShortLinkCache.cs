using System.Threading.Tasks;
using UrlShortenerService.Models;
using UrlShortenerService.Services;

namespace UrlShortenerService.Cache
{
    public class ShortLinkCache : IShortLinkCache
    {
        private readonly IRedisCacheService _cache;

        public ShortLinkCache(IRedisCacheService cache)
        {
            _cache = cache;
        }

        public async Task<ShortLink> GetAsync(string shortLinkKey)
        {
            return await _cache.GetAsync<ShortLink>(shortLinkKey);
        }

        public async Task AddAsync(ShortLink shortLink)
        {
            await _cache.SetAsync<ShortLink>(shortLink.LinkKey, shortLink);
        }
    }
}