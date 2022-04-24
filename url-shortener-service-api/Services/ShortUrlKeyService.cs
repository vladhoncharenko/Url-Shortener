using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using UrlShortenerService.Cache;
using UrlShortenerService.Data;
using UrlShortenerService.Models;

namespace UrlShortenerService.Services
{
    public class ShortUrlKeyService : IShortUrlKeyService
    {
        readonly IConfiguration _configuration;
        private readonly IShortUrlKeyRepo _shortUrlKeyRepo;
        private readonly IStackCacheService<ShortUrlKey> _stackCacheService;
        private readonly IUrlKeyGenerationService _shortUrlKeyGenerationService;

        public ShortUrlKeyService(IUrlKeyGenerationService shortUrlKeyGenerationService, IShortUrlKeyRepo shortUrlKeyRepo, IStackCacheService<ShortUrlKey> stackCacheService,
            IConfiguration configuration)
        {
            _shortUrlKeyGenerationService = shortUrlKeyGenerationService;
            _shortUrlKeyRepo = shortUrlKeyRepo;
            _stackCacheService = stackCacheService;
            _configuration = configuration;
        }

        public async Task GenerateAsync(int shortUrlKeysAmount)
        {
            if (shortUrlKeysAmount <= 0)
                throw new ArgumentOutOfRangeException(nameof(shortUrlKeysAmount));

            var generatedKeys = _shortUrlKeyGenerationService.GenerateNewKeys(shortUrlKeysAmount);

            await _shortUrlKeyRepo.AddRangeAsync(generatedKeys);
            await _shortUrlKeyRepo.SaveChangesAsync();
        }

        public async Task<IEnumerable<ShortUrlKey>> GetAsync(int keysAmount)
        {
            if (keysAmount <= 0)
                throw new ArgumentOutOfRangeException(nameof(keysAmount));

            if (_shortUrlKeyRepo.Count() <= _configuration.GetValue<int>("EnvVars:AmountOfKeysThreshold"))
            {
                await GenerateAsync(_configuration.GetValue<int>("EnvVars:AmountOfKeysRuntimeGenerated"));
            }

            var shortUrlKeys = _shortUrlKeyRepo.Get(keysAmount);
            shortUrlKeys.ToList().ForEach(k => k.IsUsed = true);
            var shortUrlKeysResult = shortUrlKeys.ToList();
            await _shortUrlKeyRepo.SaveChangesAsync();

            return shortUrlKeysResult;
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
    }
}