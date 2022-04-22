using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UrlShortenerService.Models;

namespace UrlShortenerService.Data
{
    public interface IShortUrlKeyRepo
    {
        Task<bool> SaveChangesAsync();

        Task AddRangeAsync(IEnumerable<ShortUrlKey> shortUrlKeys);

        void Delete(DateTime dateTime);

        int Count(bool isUsed = false);

        IEnumerable<ShortUrlKey> Get(int keysAmount, bool isUsed = false);
    }
}