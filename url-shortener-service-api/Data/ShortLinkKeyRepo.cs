using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UrlShortenerService.Cache;
using UrlShortenerService.Models;
using UrlShortenerService.Services;

namespace UrlShortenerService.Data
{
    public class ShortLinkKeyRepo : IShortLinkKeyRepo
    {
        private readonly AppDbContext _context;
        private readonly IStackCacheService<ShortLinkKey> _stackCacheService;
        private readonly IShortLinkKeyGenerationService _shortLinkKeyGenerationService;

        public ShortLinkKeyRepo(AppDbContext context, IShortLinkKeyGenerationService shortLinkKeyGenerationService, IStackCacheService<ShortLinkKey> stackCacheService)
        {
            _context = context;
            _shortLinkKeyGenerationService = shortLinkKeyGenerationService;
            _stackCacheService = stackCacheService;
        }

        public async Task<IEnumerable<ShortLinkKey>> GetShortLinkKeysAsync(int keysAmount)
        {
            if (keysAmount <= 0)
                throw new ArgumentOutOfRangeException(nameof(keysAmount));

            await _context.Database.EnsureCreatedAsync();

            if (_context.ShortLinkKeys.Where(k => !k.IsUsed).Count() <= 50)
            {
                await GenerateNewShortLinkKeysAsync(50);
            }

            var shortLinkKeys = _context.ShortLinkKeys.Where(k => !k.IsUsed).Take(keysAmount);
            shortLinkKeys.ToList().ForEach(k => k.IsUsed = true);

            await SaveChangesAsync();

            return shortLinkKeys;
        }

        public async Task GenerateNewShortLinkKeysAsync(int shortLinkKeysAmount)
        {
            if (shortLinkKeysAmount <= 0)
                throw new ArgumentOutOfRangeException(nameof(shortLinkKeysAmount));

            await _context.ShortLinkKeys.AddRangeAsync(_shortLinkKeyGenerationService.GenerateNewShortLinkKeys(shortLinkKeysAmount));
        }

        public async Task<ShortLinkKey> GetShortLinkKeyAsync()
        {
            var shortLinkKey = _stackCacheService.Get();
            if (shortLinkKey == null)
            {
                _stackCacheService.Add(await GetShortLinkKeysAsync(50));
                shortLinkKey = _stackCacheService.Get();
            }

            return shortLinkKey;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() > 0);
        }
    }
}