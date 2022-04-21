using System.Collections.Generic;
using UrlShortenerService.Models;

namespace UrlShortenerService.Services
{
    public interface IShortLinkKeyGenerationService
    {
        IEnumerable<ShortLinkKey> GenerateNewShortLinkKeys(int shortLinkKeysAmount);

        ShortLinkKey CreateNewShortLinkKey(int keySymbolsAmount);

        string GenarateLinkKey(int symbolsAmount);
    }
}