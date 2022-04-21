using System.Collections.Generic;
using UrlShortenerService.Models;

namespace UrlShortenerService.Cache
{
    public class ShortLinkKeyCache : IShortLinkKeyCache
    {
        private static Stack<ShortLinkKey> shortLinkKeys = new Stack<ShortLinkKey>() { };

        public ShortLinkKeyCache()
        {

        }

        public ShortLinkKey GetShortLinkKey()
        {
            return shortLinkKeys.Count > 0 ? shortLinkKeys.Pop() : null;
        }

        public void AddShortLinkKeys(IEnumerable<ShortLinkKey> newShortLinkKeys)
        {
            shortLinkKeys = new Stack<ShortLinkKey>(newShortLinkKeys) { };
        }
    }
}
