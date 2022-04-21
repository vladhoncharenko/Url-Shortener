using System.Threading.Tasks;
using UrlShortenerService.Models;

namespace UrlShortenerService.Cache
{
    public interface IShortLinkCache
    {
        Task<ShortLink> GetAsync(string shortLinkKey);

        Task AddAsync(ShortLink shortLink);
    }
}
