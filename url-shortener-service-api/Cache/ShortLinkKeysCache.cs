using System.Collections.Generic;
using UrlShortenerService.Models;

namespace UrlShortenerService.Cache
{
    public class ShortLinkKeyCache : IShortLinkKeyCache
    {
        private Stack<ShortLinkKey> shortLinkKeys = new Stack<ShortLinkKey>() { };

        public ShortLinkKeyCache()
        {

        }

        public ShortLinkKey Get()
        {
            return shortLinkKeys.Count > 0 ? shortLinkKeys.Pop() : null;
        }

        public void Add(IEnumerable<ShortLinkKey> newShortLinkKeys)
        {

            shortLinkKeys = new Stack<ShortLinkKey>(newShortLinkKeys) { };
        }
    }
}
