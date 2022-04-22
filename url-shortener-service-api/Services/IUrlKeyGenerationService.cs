using System.Collections.Generic;
using UrlShortenerService.Models;

namespace UrlShortenerService.Services
{
    public interface IUrlKeyGenerationService
    {
        /// <summary>
        /// Generates new unique keys to use as IDs for shortened URLs
        /// </summary>
        /// <param name="shortUrlKeysAmount"></param>
        /// <returns>Returns a collection of ShortUrlKey</returns>
        IEnumerable<ShortUrlKey> GenerateNewKeys(int shortUrlKeysAmount);
    }
}