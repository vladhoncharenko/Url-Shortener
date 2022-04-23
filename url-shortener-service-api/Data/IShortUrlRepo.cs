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

        ShortUrl Get(string shortUrlKey);

        Task AddAsync(ShortUrl shortUrl);

        void Delete(DateTime dateTime);

        ShortUrl RegisterRedirect(string shortUrlKey);
    }
}