using System;
using System.Collections.Generic;
using System.Linq;
using UrlShortenerService.Models;
using UrlShortenerService.Services;

namespace UrlShortenerService.Data
{
    public class ShortLinkKeyRepo : IShortLinkKeyRepo
    {
        private readonly AppDbContext _context;
        private readonly ShortLinkKeyGenerationService _shortLinkKeyGenerationService;

        public ShortLinkKeyRepo(AppDbContext context, ShortLinkKeyGenerationService shortLinkKeyGenerationService)
        {
            _context = context;
            _shortLinkKeyGenerationService = shortLinkKeyGenerationService;
        }

        public IEnumerable<ShortLinkKey> GetShortLinkKeys(int keysAmount)
        {
            if (keysAmount <= 0)
                throw new ArgumentOutOfRangeException(nameof(keysAmount));

            var shortLinkKeys = _context.ShortLinkKeys.Take(keysAmount);
            _context.ShortLinkKeys.RemoveRange(shortLinkKeys);
            SaveChanges();

            return shortLinkKeys;
        }

        public void GenerateNewShortLinkKeys(int shortLinkKeysAmount)
        {
            if (shortLinkKeysAmount <= 0)
                throw new ArgumentOutOfRangeException(nameof(shortLinkKeysAmount));

            _context.ShortLinkKeys.AddRange(_shortLinkKeyGenerationService.GenerateNewShortLinkKeys(shortLinkKeysAmount));

            SaveChanges();
        }

        public bool AreNewShortLinkKeysNeeded()
        {
            return (_context.ShortLinkKeys.Count() < 100);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() > 0);
        }
    }
}