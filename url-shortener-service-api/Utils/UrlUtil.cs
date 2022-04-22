using System;

namespace UrlShortenerService.Utils
{
    public class UrlUtil : IUrlUtil
    {
        public string GetShortUrlKeyFromUrl(string shortUrlKeyWithUrl)
        {
            if (string.IsNullOrEmpty(shortUrlKeyWithUrl))
                throw new ArgumentException(nameof(shortUrlKeyWithUrl));

            return shortUrlKeyWithUrl.Substring(shortUrlKeyWithUrl.LastIndexOf('/') + 1);
        }
    }
}