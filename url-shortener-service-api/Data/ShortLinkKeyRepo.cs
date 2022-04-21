using System;
using System.Collections.Generic;
using System.Linq;
using UrlShortenerService.Cache;
using UrlShortenerService.Models;
using UrlShortenerService.Services;

namespace UrlShortenerService.Data
{
    public class ShortLinkKeyRepo : IShortLinkKeyRepo
    {
        private readonly AppDbContext _context;
        private readonly ShortLinkKeyCache _shortLinkKeyCache;
        private readonly ShortLinkKeyGenerationService _shortLinkKeyGenerationService;

        public ShortLinkKeyRepo(AppDbContext context, ShortLinkKeyGenerationService shortLinkKeyGenerationService, ShortLinkKeyCache shortLinkKeyCache)
        {
            _context = context;
            _shortLinkKeyGenerationService = shortLinkKeyGenerationService;
            _shortLinkKeyCache = shortLinkKeyCache;
        }

        public IEnumerable<ShortLinkKey> GetShortLinkKeys(int keysAmount)
        {
            if (keysAmount <= 0)
                throw new ArgumentOutOfRangeException(nameof(keysAmount));

            var shortLinkKeys = _context.ShortLinkKeys.Where(k => !k.IsUsed).Take(keysAmount);
            shortLinkKeys.ToList().ForEach(k => k.IsUsed = true);

            if (_context.ShortLinkKeys.Where(k => !k.IsUsed).Count() <= 1000)
            {
                GenerateNewShortLinkKeys(1000);
            }

            SaveChanges();

            return shortLinkKeys;
        }

        public void GenerateNewShortLinkKeys(int shortLinkKeysAmount)
        {
            if (shortLinkKeysAmount <= 0)
                throw new ArgumentOutOfRangeException(nameof(shortLinkKeysAmount));

            _context.ShortLinkKeys.AddRange(_shortLinkKeyGenerationService.GenerateNewShortLinkKeys(shortLinkKeysAmount));
        }

        public ShortLinkKey GetShortLinkKey()
        {
            var shortLinkKey = _shortLinkKeyCache.GetShortLinkKey();
            if (shortLinkKey == null)
            {
                _shortLinkKeyCache.AddShortLinkKeys(GetShortLinkKeys(100));
                shortLinkKey = _shortLinkKeyCache.GetShortLinkKey();
            }

            return shortLinkKey;
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() > 0);
        }
    }
}