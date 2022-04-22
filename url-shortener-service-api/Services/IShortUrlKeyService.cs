using System.Collections.Generic;
using System.Threading.Tasks;
using UrlShortenerService.Models;

namespace UrlShortenerService.Services
{
    public interface IShortUrlKeyService
    {
        Task<IEnumerable<ShortUrlKey>> GetAsync(int keysAmount);

        Task<ShortUrlKey> GetAsync();

        Task GenerateAsync(int shortUrlKeysAmount);
    }
}