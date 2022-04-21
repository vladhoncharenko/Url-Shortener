using System.Collections.Generic;
using System.Threading.Tasks;
using UrlShortenerService.DTOs;
using UrlShortenerService.Models;

namespace UrlShortenerService.Services
{
    public interface IShortLinkService
    {
        Task<ShortLink> AddNewShortLinkAsync(ShortLinkCreateDTO shortLinkCreateDTO);

        Task<string> GetShortLinkRedirectUrlAsync(string shortLinkKeyWithUrl);

        IEnumerable<ShortLink> GetShortLinks(int page, int pageCapacity);
    }
}