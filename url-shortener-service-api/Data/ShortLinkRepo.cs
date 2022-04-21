using System;
using System.Collections.Generic;
using System.Linq;
using UrlShortenerService.Models;

namespace UrlShortenerService.Data
{
    public class ShortLinkRepo : IShortLinkRepo
    {
        private readonly AppDbContext _context;

        public ShortLinkRepo(AppDbContext context)
        {
            _context = context;
        }

        public void AddShortLink(ShortLink shortLink)
        {
            if (shortLink == null)
                throw new ArgumentNullException(nameof(shortLink));

            _context.ShortLinks.Add(shortLink);
        }

        public ShortLink GetShortLink(string shortLinkKey)
        {
            if (String.IsNullOrEmpty(shortLinkKey))
                throw new ArgumentNullException(nameof(shortLinkKey));

            return _context.ShortLinks.FirstOrDefault(p => p.LinkKey.Equals(shortLinkKey));
        }

        public IEnumerable<ShortLink> GetShortLinks(int page, int pageCapacity)
        {
            if (page <= 0)
                throw new ArgumentOutOfRangeException(nameof(page));

            if (pageCapacity <= 0)
                throw new ArgumentOutOfRangeException(nameof(pageCapacity));

            return _context.ShortLinks.Skip(page * pageCapacity).Take(pageCapacity);
        }

        public void RegisterShortLinkRedirect(string shortLinkKey)
        {
            var shortLink = GetShortLink(shortLinkKey);
            shortLink.LastRedirect = DateTime.UtcNow;
            shortLink.RedirectsCount += 1;
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() > 0);
        }
    }
}