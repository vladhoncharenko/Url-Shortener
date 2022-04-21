using System;
using System.Collections.Generic;
using System.Linq;
using UrlShortenerService.Models;

namespace UrlShortenerService.Cache
{
    public class ShortLinkCache : IShortLinkCache
    {
        private static List<ShortLink> shortLinks = new List<ShortLink>() { };

        public ShortLinkCache()
        {

        }

        public ShortLink GetShortLinkFromCache(string shortLinkKey)
        {
            var item = shortLinks.First(x => x.LinkKey == shortLinkKey);

            return item;
        }

        public void AddShortLinkToCache(ShortLink shortLink)
        {
            shortLinks.Add(shortLink);
        }
    }
}
