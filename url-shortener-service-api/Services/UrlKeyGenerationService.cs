using System;
using System.Collections.Generic;
using System.Linq;
using UrlShortenerService.Models;

namespace UrlShortenerService.Services
{
    public class UrlKeyGenerationService : IUrlKeyGenerationService
    {
        private readonly Random _random;
        private const int amountOfCharsInUrlKey = 6;
        private const string allowedCharsInUrlKey = "abcdefghijklmnopqrstuvwxyz1234567890";

        public UrlKeyGenerationService()
        {
            _random = new Random();
        }

        public IEnumerable<ShortUrlKey> GenerateNewKeys(int shortUrlKeysAmount)
        {
            if (shortUrlKeysAmount <= 0)
                throw new ArgumentOutOfRangeException(nameof(shortUrlKeysAmount));

            return Enumerable.Range(0, shortUrlKeysAmount).Select(n => GenerateNewKey(amountOfCharsInUrlKey)).ToList();
        }

        private ShortUrlKey GenerateNewKey(int keySymbolsAmount)
        {
            return new ShortUrlKey { UrlKey = GenerateNewKeyValue(keySymbolsAmount) };
        }

        private string GenerateNewKeyValue(int symbolsAmount)
        {
            return new string(Enumerable.Range(1, symbolsAmount).Select(_ => allowedCharsInUrlKey[_random.Next(allowedCharsInUrlKey.Length)]).ToArray());
        }
    }
}