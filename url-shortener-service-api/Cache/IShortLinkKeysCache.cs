using System.Collections.Generic;
using UrlShortenerService.Models;

namespace UrlShortenerService.Cache
{
    public interface IShortLinkKeyCache
    {
        public ShortLinkKey Get();

        public void Add(IEnumerable<ShortLinkKey> newShortLinkKeys);
    }
}
