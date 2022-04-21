using System;
using System.Collections.Generic;
using System.Linq;
using UrlShortenerService.Models;

namespace UrlShortenerService.Cache
{
    public class ShortLinkCache : IShortLinkCache
    {
        private List<ShortLink> shortLinkKeys = new List<ShortLink>() { };

        public ShortLinkCache()
        {

        }

        public ShortLink GetShortLinkFromCache(string shortLinkKey)
        {
            var item = shortLinkKeys.First(x => x.LinkKey == shortLinkKey);
            shortLinkKeys.Remove(item);

            return item;
        }

        public void AddShortLinkToCache(ShortLink shortLink)
        {
            shortLinkKeys.Add(shortLink);
        }
    }
}
