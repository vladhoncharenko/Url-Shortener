using UrlShortenerService.Models;

namespace UrlShortenerService.Cache
{
    public interface IShortLinkCache
    {
        ShortLink GetShortLinkFromCache(string shortLinkKey);

        void AddShortLinkToCache(ShortLink shortLink);
    }
}
