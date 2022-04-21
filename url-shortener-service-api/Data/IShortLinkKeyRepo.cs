using System.Collections.Generic;
using System.Threading.Tasks;
using UrlShortenerService.Models;

namespace UrlShortenerService.Data
{
    public interface IShortLinkKeyRepo
    {
        Task<bool> SaveChangesAsync();

        Task<IEnumerable<ShortLinkKey>> GetShortLinkKeysAsync(int keysAmount);

        Task<ShortLinkKey> GetShortLinkKeyAsync();

        Task GenerateNewShortLinkKeysAsync(int shortLinkKeysAmount);
    }
}