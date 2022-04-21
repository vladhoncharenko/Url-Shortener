using System;

namespace UrlShortenerService.Utils
{
    public class UrlUtil : IUrlUtil
    {
        public string GetShortLinkKeyFromUrl(string shortLinkKeyWithUrl)
        {
            if (string.IsNullOrEmpty(shortLinkKeyWithUrl))
                throw new ArgumentException(nameof(shortLinkKeyWithUrl));

            return shortLinkKeyWithUrl.Substring(shortLinkKeyWithUrl.LastIndexOf('/') + 1);
        }
    }
}