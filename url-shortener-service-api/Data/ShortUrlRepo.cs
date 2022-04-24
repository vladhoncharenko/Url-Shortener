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

        public ShortUrl Get(string shortUrlKey)
        {
            return _context.ShortUrls.FirstOrDefault(p => p.UrlKey.Equals(shortUrlKey));
        }

        public async Task AddAsync(ShortUrl shortUrl)
        {
            if (shortUrl == null)
                throw new ArgumentNullException(nameof(shortUrl));

            await _context.ShortUrls.AddAsync(shortUrl);
        }

        public (IEnumerable<ShortUrl>, int) Get(int page, int pageCapacity)
        {
            if (page <= 0)
                throw new ArgumentOutOfRangeException(nameof(page));

            if (pageCapacity <= 0)
                throw new ArgumentOutOfRangeException(nameof(pageCapacity));

            return (_context.ShortUrls.OrderBy(x=>x.CreatedOn).Skip((page - 1) * pageCapacity).Take(pageCapacity), _context.ShortUrls.Count());
        }

        public void Delete(DateTime dateTime)
        {
            _context.ShortUrls.RemoveRange(_context.ShortUrls.Where(x => x.CreatedOn >= dateTime));
        }

        public ShortUrl RegisterRedirect(string shortUrlKey)
        {
            var shortUrl = Get(shortUrlKey);
            shortUrl.LastRedirect = DateTime.UtcNow;
            shortUrl.RedirectsCount += 1;

            return shortUrl;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() > 0);
        }
    }
}