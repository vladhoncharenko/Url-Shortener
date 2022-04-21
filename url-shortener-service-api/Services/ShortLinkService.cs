using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using UrlShortenerService.Data;
using UrlShortenerService.DTOs;
using UrlShortenerService.Models;
using UrlShortenerService.Utils;

namespace UrlShortenerService.Services
{
    public class ShortLinkService : IShortLinkService
    {
        private readonly IShortLinkRepo _shortLinkRepo;
        private readonly IShortLinkKeyRepo _shortLinkKeyRepo;
        private readonly ILogger<ShortLinkService> _logger;
        private readonly IUrlUtil _urlUtil;

        public ShortLinkService(ILogger<ShortLinkService> logger, IShortLinkKeyRepo shortLinkKeyRepo, IShortLinkRepo shortLinkRepo, IUrlUtil urlUtil)
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

        public async Task<string> GetShortLinkRedirectUrlAsync(string shortLinkKey)
        {
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