using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _configuration;

        public ShortUrlKeyRepo(AppDbContext context, IUrlKeyGenerationService shortUrlKeyGenerationService, IStackCacheService<ShortUrlKey> stackCacheService, IConfiguration configuration)
        {
            _context = context;
            _shortUrlKeyGenerationService = shortUrlKeyGenerationService;
            _stackCacheService = stackCacheService;
            _configuration = configuration;
        }

        public async Task<IEnumerable<ShortUrlKey>> GetAsync(int keysAmount)
        {
            if (keysAmount <= 0)
                throw new ArgumentOutOfRangeException(nameof(keysAmount));

            if (_context.ShortUrlKeys.Where(k => !k.IsUsed).Count() <= _configuration.GetValue<int>("EnvVars:AmountOfKeysThreshold"))
            {
                await GenerateAsync(_configuration.GetValue<int>("EnvVars:AmountOfKeysRuntimeGenerated"));
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
                _stackCacheService.Add(await GetAsync(_configuration.GetValue<int>("EnvVars:AmountOfKeysInMemoryCache")));
                shortUrlKey = _stackCacheService.Get();
            }

            return shortUrlKey;
        }

        public void Delete(DateTime dateTime)
        {
            _context.ShortUrlKeys.RemoveRange(_context.ShortUrlKeys.Where(x => x.IssuedOn >= dateTime));
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() > 0);
        }
    }
}