using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UrlShortenerService.Models;

namespace UrlShortenerService.Data
{
    public interface IShortUrlKeyRepo
    {
        Task<bool> SaveChangesAsync();

        Task<IEnumerable<ShortUrlKey>> GetAsync(int keysAmount);

        Task<ShortUrlKey> GetAsync();

        Task GenerateAsync(int shortUrlKeysAmount);

        Task DeleteAsync(DateTime dateTime);
    }
}