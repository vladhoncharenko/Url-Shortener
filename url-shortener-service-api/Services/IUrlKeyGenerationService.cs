using System.Collections.Generic;
using UrlShortenerService.Models;

namespace UrlShortenerService.Services
{
    public interface IUrlKeyGenerationService
    {
        IEnumerable<ShortUrlKey> GenerateNewKeys(int shortUrlKeysAmount);
    }
}