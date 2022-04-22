using System.Collections.Generic;
using System.Threading.Tasks;
using UrlShortenerService.Models;

namespace UrlShortenerService.Services
{
    public interface IShortUrlKeyService
    {
        /// <summary>
        /// Returns a portion of Short Url Keys to be placed in cache
        /// </summary>
        /// <param name="keysAmount"></param>
        /// <returns>Collection of ShortUrlKey</returns>
        Task<IEnumerable<ShortUrlKey>> GetAsync(int keysAmount);

        /// <summary>
        /// Rerurns a single ShortUrlKey entity to be used as unique ID of shortened URL
        /// </summary>
        /// <returns>A single ShortUrlKey entity</returns>
        Task<ShortUrlKey> GetAsync();

        /// <summary>
        /// Populates DB with a fresh URL keys
        /// </summary>
        /// <param name="shortUrlKeysAmount"></param>
        /// <returns></returns>
        Task GenerateAsync(int shortUrlKeysAmount);
    }
}