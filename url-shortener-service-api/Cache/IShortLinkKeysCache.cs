using System.Collections.Generic;
using UrlShortenerService.Models;

namespace UrlShortenerService.Cache
{
    public interface IShortLinkKeyCache
    {
        public ShortLinkKey GetShortLinkKey();

        public void AddShortLinkKeys(IEnumerable<ShortLinkKey> newShortLinkKeys);
    }
}
