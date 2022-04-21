using System;
using System.Collections.Generic;
using System.Linq;
using UrlShortenerService.Cache;
using UrlShortenerService.Models;

namespace UrlShortenerService.Data
{
    public class ShortLinkRepo : IShortLinkRepo
    {
        private readonly AppDbContext _context;
        private readonly ShortLinkCache _shortLinkCache;

        public ShortLinkRepo(AppDbContext context, ShortLinkCache shortLinkCache)
        {
            _context = context;
            _shortLinkCache = shortLinkCache;
        }

        public void AddShortLink(ShortLink shortLink)
        {
            if (shortLink == null)
                throw new ArgumentNullException(nameof(shortLink));

            _context.ShortLinks.Add(shortLink);
            _shortLinkCache.AddShortLinkToCache(shortLink);
        }

        public ShortLink ResolveShortLink(string shortLinkKey)
        {
            if (String.IsNullOrEmpty(shortLinkKey))
                throw new ArgumentNullException(nameof(shortLinkKey));

            var shortLink = _shortLinkCache.GetShortLinkFromCache(shortLinkKey);
            if (shortLink == null)
            {
                shortLink = _context.ShortLinks.FirstOrDefault(p => p.LinkKey.Equals(shortLinkKey));
                if (shortLink != null)
                    _shortLinkCache.AddShortLinkToCache(shortLink);
            }

            if (shortLink == null || String.IsNullOrEmpty(shortLink.OriginalUrl))
                throw new ArgumentNullException(nameof(shortLinkKey));

            shortLink.LastRedirect = DateTime.UtcNow;
            shortLink.RedirectsCount += 1;

            SaveChanges();

            return shortLink;
        }

        public IEnumerable<ShortLink> GetShortLinks(int page, int pageCapacity)
        {
            if (page <= 0)
                throw new ArgumentOutOfRangeException(nameof(page));

            if (pageCapacity <= 0)
                throw new ArgumentOutOfRangeException(nameof(pageCapacity));

            return _context.ShortLinks.Skip(page * pageCapacity).Take(pageCapacity);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() > 0);
        }
    }
}