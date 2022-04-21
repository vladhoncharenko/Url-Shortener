namespace UrlShortenerService.Utils
{
    public class UrlUtil : IUrlUtil
    {
        public string GetShortLinkKeyFromUrl(string shortLinkKeyWithUrl)
        {
            return shortLinkKeyWithUrl.Substring(shortLinkKeyWithUrl.LastIndexOf('/') + 1);
        }
    }
}