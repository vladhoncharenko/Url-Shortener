using System.Collections.Generic;
using UrlShortenerService.Models;

namespace UrlShortenerService.Data
{
    public interface IShortLinkRepo
    {
        bool SaveChanges();

        IEnumerable<ShortLink> GetShortLinks(int page, int pageCapacity);

        ShortLink ResolveShortLink(string shortLinkKey);

        void AddShortLink(ShortLink shortLink);
    }
}