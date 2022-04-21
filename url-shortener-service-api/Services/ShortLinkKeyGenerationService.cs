using System;
using System.Collections.Generic;
using System.Linq;
using UrlShortenerService.Models;

namespace UrlShortenerService.Services
{
    public class ShortLinkKeyGenerationService : IShortLinkKeyGenerationService
    {
        private readonly Random _random;
        private const int amountOfCharsInLinkKey = 6;
        private const string allowedCharsInLinkKey = "abcdefghijklmnopqrstuvwxyz1234567890";

        public ShortLinkKeyGenerationService()
        {
            _random = new Random();
        }

        public IEnumerable<ShortLinkKey> GenerateNewShortLinkKeys(int shortLinkKeysAmount)
        {
            if (shortLinkKeysAmount <= 0)
                throw new ArgumentOutOfRangeException(nameof(shortLinkKeysAmount));

            return Enumerable.Range(0, shortLinkKeysAmount).Select(n => CreateNewShortLinkKey(amountOfCharsInLinkKey)).ToList();
        }

        public ShortLinkKey CreateNewShortLinkKey(int keySymbolsAmount)
        {
            if (keySymbolsAmount <= 0)
                throw new ArgumentOutOfRangeException(nameof(keySymbolsAmount));

            return new ShortLinkKey { LinkKey = GenarateLinkKey(keySymbolsAmount) };
        }

        public string GenarateLinkKey(int symbolsAmount)
        {
            if (symbolsAmount <= 0)
                throw new ArgumentOutOfRangeException(nameof(symbolsAmount));

            return new string(Enumerable.Range(1, symbolsAmount).Select(_ => allowedCharsInLinkKey[_random.Next(allowedCharsInLinkKey.Length)]).ToArray());
        }
    }
}