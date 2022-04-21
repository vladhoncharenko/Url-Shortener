using System.Collections.Generic;
using System.Threading.Tasks;
using UrlShortenerService.Models;

namespace UrlShortenerService.Data
{
    public interface IShortLinkRepo
    {
        Task<bool> SaveChangesAsync();

        IEnumerable<ShortLink> GetShortLinks(int page, int pageCapacity);

        Task<ShortLink> ResolveShortLinkAsync(string shortLinkKey);

        Task AddShortLinkAsync(ShortLink shortLink);
    }
}