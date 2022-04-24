using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UrlShortenerService.Models;

namespace UrlShortenerService.Data
{
    public interface IShortUrlRepo
    {
        Task<bool> SaveChangesAsync();

        (IEnumerable<ShortUrl>, int) Get(int page, int pageCapacity);

        Task<ShortUrl> GetAsync(string shortUrlKey);

        Task AddAsync(ShortUrl shortUrl);

        void Delete(DateTime dateTime);

        Task<ShortUrl> RegisterRedirectAsync(string shortUrlKey);
    }
}