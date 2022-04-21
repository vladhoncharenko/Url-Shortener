using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UrlShortenerService.Cache;
using UrlShortenerService.Models;

namespace UrlShortenerService.Data
{
    public class ShortLinkRepo : IShortLinkRepo
    {
        private readonly AppDbContext _context;
        private readonly IShortLinkCache _shortLinkCache;

        public ShortLinkRepo(AppDbContext context, IShortLinkCache shortLinkCache)
        {
            _context = context;
            _shortLinkCache = shortLinkCache;
        }

        public async Task AddShortLinkAsync(ShortLink shortLink)
        {
            if (shortLink == null)
                throw new ArgumentNullException(nameof(shortLink));

            await _context.ShortLinks.AddAsync(shortLink);
            await _shortLinkCache.AddAsync(shortLink);
        }

        public async Task<ShortLink> ResolveShortLinkAsync(string shortLinkKey)
        {
            if (String.IsNullOrEmpty(shortLinkKey))
                throw new ArgumentNullException(nameof(shortLinkKey));

            var shortLink = _context.ShortLinks.FirstOrDefault(p => p.LinkKey.Equals(shortLinkKey));

            if (shortLink == null || String.IsNullOrEmpty(shortLink.OriginalUrl))
                throw new ArgumentNullException(nameof(shortLinkKey));

            shortLink.LastRedirect = DateTime.UtcNow;
            shortLink.RedirectsCount += 1;

            await SaveChangesAsync();

            return shortLink;
        }

        public IEnumerable<ShortLink> GetShortLinks(int page, int pageCapacity)
        {
            if (page <= 0)
                throw new ArgumentOutOfRangeException(nameof(page));

            if (pageCapacity <= 0)
                throw new ArgumentOutOfRangeException(nameof(pageCapacity));

            return _context.ShortLinks.Skip((page - 1) * pageCapacity).Take(pageCapacity);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() > 0);
        }
    }
}