using System.Collections.Generic;
using UrlShortenerService.Models;

namespace UrlShortenerService.Cache
{
    public class ShortLinkKeyCache
    {
        private Stack<ShortLinkKey> shortLinkKeys = new Stack<ShortLinkKey>() { };

        public ShortLinkKeyCache()
        {

        }

        public ShortLinkKey GetShortLinkKey()
        {
            return shortLinkKeys.Pop();
        }

        public void AddShortLinkKeys(IEnumerable<ShortLinkKey> newShortLinkKeys)
        {
            shortLinkKeys = new Stack<ShortLinkKey>(newShortLinkKeys) { };
        }
    }
}
