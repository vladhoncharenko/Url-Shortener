using System.Collections.Generic;
using UrlShortenerService.Models;

namespace UrlShortenerService.Data
{
    public interface IShortLinkKeyRepo
    {
        bool SaveChanges();

        IEnumerable<ShortLinkKey> GetShortLinkKeys(int keysAmount);

        void GenerateNewShortLinkKeys(int shortLinkKeysAmount);

        bool AreNewShortLinkKeysNeeded();
    }
}