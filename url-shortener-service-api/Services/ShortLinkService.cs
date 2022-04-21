using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using UrlShortenerService.Data;
using UrlShortenerService.DTOs;
using UrlShortenerService.Models;

namespace UrlShortenerService.Services
{
    public class ShortLinkService
    {
        private readonly IShortLinkRepo _shortLinkRepo;
        private readonly IShortLinkKeyRepo _shortLinkKeyRepo;
        private readonly ILogger<ShortLinkService> _logger;

        public ShortLinkService(ILogger<ShortLinkService> logger, IShortLinkKeyRepo shortLinkKeyRepo, IShortLinkRepo shortLinkRepo)
        {
            _logger = logger;
            _shortLinkRepo = shortLinkRepo;
            _shortLinkKeyRepo = shortLinkKeyRepo;
        }

        public ShortLink AddNewShortLink(ShortLinkCreateDTO shortLinkCreateDTO)
        {
            var createdShortLink = new ShortLink() { };
            _shortLinkRepo.AddShortLink(createdShortLink);
            _shortLinkRepo.SaveChanges();

            return createdShortLink;
        }

        public string GetShortLinkRedirectUrl(string shortLinkKeyWithUrl)
        {
            var shortLinkKey = GetShortLinkKeyFromUrl(shortLinkKeyWithUrl);
            var shortLink = _shortLinkRepo.ResolveShortLink(shortLinkKey);

            return shortLink.OriginalUrl;
        }

        private string GetShortLinkKeyFromUrl(string shortLinkKeyWithUrl)
        {
            return shortLinkKeyWithUrl;
        }

        public IEnumerable<ShortLink> GetShortLinks(int page, int pageCapacity)
        {
            var shortLinks = _shortLinkRepo.GetShortLinks(page, pageCapacity);

            return shortLinks;
        }
    }
}