using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UrlShortenerService.Models;

namespace UrlShortenerService.Data
{
    public interface IShortUrlRepo
    {
        Task<bool> SaveChangesAsync();

        IEnumerable<ShortUrl> Get(int page, int pageCapacity);

        Task<ShortUrl> ResolveAsync(string shortUrlKey);

        Task AddAsync(ShortUrl shortUrl);

        void Delete(DateTime dateTime);
    }
}