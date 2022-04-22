using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UrlShortenerService.Cache;
using UrlShortenerService.Models;
using UrlShortenerService.Services;

namespace UrlShortenerService.Data
{
    public class ShortUrlKeyRepo : IShortUrlKeyRepo
    {
        private readonly AppDbContext _context;
        private readonly IStackCacheService<ShortUrlKey> _stackCacheService;
        private readonly IUrlKeyGenerationService _shortUrlKeyGenerationService;

        public ShortUrlKeyRepo(AppDbContext context, IUrlKeyGenerationService shortUrlKeyGenerationService, IStackCacheService<ShortUrlKey> stackCacheService)
        {
            _context = context;
            _shortUrlKeyGenerationService = shortUrlKeyGenerationService;
            _stackCacheService = stackCacheService;
        }

        public async Task<IEnumerable<ShortUrlKey>> GetAsync(int keysAmount)
        {
            if (keysAmount <= 0)
                throw new ArgumentOutOfRangeException(nameof(keysAmount));

            await _context.Database.EnsureCreatedAsync();

            if (_context.ShortUrlKeys.Where(k => !k.IsUsed).Count() <= 50)
            {
                await GenerateAsync(50);
            }

            var shortUrlKeys = _context.ShortUrlKeys.Where(k => !k.IsUsed).Take(keysAmount);
            shortUrlKeys.ToList().ForEach(k => k.IsUsed = true);

            await SaveChangesAsync();

            return shortUrlKeys;
        }

        public async Task GenerateAsync(int shortUrlKeysAmount)
        {
            if (shortUrlKeysAmount <= 0)
                throw new ArgumentOutOfRangeException(nameof(shortUrlKeysAmount));

            await _context.ShortUrlKeys.AddRangeAsync(_shortUrlKeyGenerationService.GenerateNewKeys(shortUrlKeysAmount));
        }

        public async Task<ShortUrlKey> GetAsync()
        {
            var shortUrlKey = _stackCacheService.Get();
            if (shortUrlKey == null)
            {
                _stackCacheService.Add(await GetAsync(50));
                shortUrlKey = _stackCacheService.Get();
            }

            return shortUrlKey;
        }

        public Task DeleteAsync(DateTime dateTime)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() > 0);
        }
    }
}