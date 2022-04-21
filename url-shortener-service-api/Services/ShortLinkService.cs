using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        public async Task<ShortLink> AddNewShortLinkAsync(ShortLinkCreateDTO shortLinkCreateDTO)
        {
            var shortLinkKey = await _shortLinkKeyRepo.GetShortLinkKeyAsync();
            var createdShortLink = new ShortLink()
            {
                LinkKey = shortLinkKey.LinkKey,
                OriginalUrl = shortLinkCreateDTO.OriginalUrl,
                CreatedOn = DateTime.UtcNow
            };
            
            await _shortLinkRepo.AddShortLinkAsync(createdShortLink);
            await _shortLinkRepo.SaveChangesAsync();

            return createdShortLink;
        }

        public async Task<string> GetShortLinkRedirectUrlAsync(string shortLinkKeyWithUrl)
        {
            var shortLinkKey = _urlUtil.GetShortLinkKeyFromUrl(shortLinkKeyWithUrl);
            var shortLink = await _shortLinkRepo.ResolveShortLinkAsync(shortLinkKey);

            return shortLink.OriginalUrl;
        }

        public IEnumerable<ShortLink> GetShortLinks(int page, int pageCapacity)
        {
            var shortLinks = _shortLinkRepo.GetShortLinks(page, pageCapacity);

            return shortLinks;
        }
    }
}