using System.Collections.Generic;
using UrlShortenerService.Models;

namespace UrlShortenerService.Data
{
    public interface IShortLinkRepo
    {
        bool SaveChanges();

        IEnumerable<ShortLink> GetAllShortenedLinks();
    }
}