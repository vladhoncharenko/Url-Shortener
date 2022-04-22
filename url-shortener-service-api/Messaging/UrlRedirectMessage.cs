using System;

namespace UrlShortenerService.Messaging
{
    public class UrlRedirectMessage
    {
        public UrlRedirectMessage(string urlKeyValue)
        {
            urlKey = urlKeyValue;
            RedirectedOn = DateTime.UtcNow;
        }

        public string urlKey { get; set; }

        public DateTime RedirectedOn { get; set; }
    }
}