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
        private readonly IShortLinkKeyCache _shortLinkKeyCache;
        private readonly IShortLinkKeyGenerationService _shortLinkKeyGenerationService;

        public ShortLinkKeyRepo(AppDbContext context, IShortLinkKeyGenerationService shortLinkKeyGenerationService, IShortLinkKeyCache shortLinkKeyCache)
        {
            _context = context;
            _shortLinkKeyGenerationService = shortLinkKeyGenerationService;
            _shortLinkKeyCache = shortLinkKeyCache;
        }

        public async Task<IEnumerable<ShortLinkKey>> GetShortLinkKeysAsync(int keysAmount)
        {
            if (keysAmount <= 0)
                throw new ArgumentOutOfRangeException(nameof(keysAmount));

            if (_context.ShortLinkKeys.Where(k => !k.IsUsed).Count() <= 1000)
            {
                await GenerateNewShortLinkKeysAsync(1000);
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
            var shortLinkKey = _shortLinkKeyCache.GetShortLinkKey();
            if (shortLinkKey == null)
            {
                _shortLinkKeyCache.AddShortLinkKeys(await GetShortLinkKeysAsync(100));
                shortLinkKey = _shortLinkKeyCache.GetShortLinkKey();
            }

            return shortLinkKey;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() > 0);
        }
    }
}