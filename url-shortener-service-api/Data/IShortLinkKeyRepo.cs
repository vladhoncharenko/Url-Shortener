using System.Collections.Generic;
using UrlShortenerService.Models;

namespace UrlShortenerService.Data
{
    public interface IShortLinkKeyRepo
    {
        bool SaveChanges();

        IEnumerable<ShortLinkKey> GetShortLinkKeys(int keysAmount);

        ShortLinkKey GetShortLinkKey();

        void GenerateNewShortLinkKeys(int shortLinkKeysAmount);
    }
}