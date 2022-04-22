using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UrlShortenerService.Cache;
using UrlShortenerService.Models;

namespace UrlShortenerService.Data
{
    public class ShortUrlRepo : IShortUrlRepo
    {
        private readonly AppDbContext _context;
        private readonly ICacheService _cache;

        public ShortUrlRepo(AppDbContext context, ICacheService cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task AddShortUrlAsync(ShortUrl shortUrl)
        {
            if (shortUrl == null)
                throw new ArgumentNullException(nameof(shortUrl));

            await _context.ShortUrls.AddAsync(shortUrl);
            await _cache.SetAsync<ShortUrl>(shortUrl.UrlKey, shortUrl);
        }

        public async Task<ShortUrl> ResolveShortUrlAsync(string shortUrlKey)
        {
            if (String.IsNullOrEmpty(shortUrlKey))
                throw new ArgumentNullException(nameof(shortUrlKey));

            var shortUrl = _context.ShortUrls.FirstOrDefault(p => p.UrlKey.Equals(shortUrlKey));

            if (shortUrl == null || String.IsNullOrEmpty(shortUrl.OriginalUrl))
                throw new ArgumentNullException(nameof(shortUrlKey));

            shortUrl.LastRedirect = DateTime.UtcNow;
            shortUrl.RedirectsCount += 1;

            await SaveChangesAsync();

            return shortUrl;
        }

        public IEnumerable<ShortUrl> GetShortUrls(int page, int pageCapacity)
        {
            if (page <= 0)
                throw new ArgumentOutOfRangeException(nameof(page));

            if (pageCapacity <= 0)
                throw new ArgumentOutOfRangeException(nameof(pageCapacity));

            return _context.ShortUrls.Skip((page - 1) * pageCapacity).Take(pageCapacity);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() > 0);
        }
    }
}