using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using UrlShortenerService.Data;
using UrlShortenerService.DTOs;
using UrlShortenerService.Models;
using UrlShortenerService.Utils;

namespace UrlShortenerService.Services
{
    public class ShortLinkService
    {
        private readonly IShortLinkRepo _shortLinkRepo;
        private readonly IShortLinkKeyRepo _shortLinkKeyRepo;
        private readonly ILogger<ShortLinkService> _logger;
        private readonly UrlUtil _urlUtil;

        public ShortLinkService(ILogger<ShortLinkService> logger, IShortLinkKeyRepo shortLinkKeyRepo, IShortLinkRepo shortLinkRepo, UrlUtil urlUtil)
        {
            _logger = logger;
            _shortLinkRepo = shortLinkRepo;
            _shortLinkKeyRepo = shortLinkKeyRepo;
            _urlUtil = urlUtil;
        }

        public ShortLink AddNewShortLink(ShortLinkCreateDTO shortLinkCreateDTO)
        {
            var createdShortLink = new ShortLink(shortLinkCreateDTO.OriginalUrl);
            _shortLinkRepo.AddShortLink(createdShortLink);
            _shortLinkRepo.SaveChanges();

            return createdShortLink;
        }

        public string GetShortLinkRedirectUrl(string shortLinkKeyWithUrl)
        {
            var shortLinkKey = _urlUtil.GetShortLinkKeyFromUrl(shortLinkKeyWithUrl);
            var shortLink = _shortLinkRepo.ResolveShortLink(shortLinkKey);

            return shortLink.OriginalUrl;
        }

        public IEnumerable<ShortLink> GetShortLinks(int page, int pageCapacity)
        {
            var shortLinks = _shortLinkRepo.GetShortLinks(page, pageCapacity);

            return shortLinks;
        }
    }
}