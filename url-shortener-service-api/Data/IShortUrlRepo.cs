using System.Collections.Generic;
using System.Threading.Tasks;
using UrlShortenerService.Models;

namespace UrlShortenerService.Data
{
    public interface IShortUrlRepo
    {
        Task<bool> SaveChangesAsync();

        IEnumerable<ShortUrl> GetShortUrls(int page, int pageCapacity);

        Task<ShortUrl> ResolveShortUrlAsync(string shortUrlKey);

        Task AddShortUrlAsync(ShortUrl shortUrl);
    }
}