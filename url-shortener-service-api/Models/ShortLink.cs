using System;
using System.ComponentModel.DataAnnotations;
using UrlShortenerService.Data;

namespace UrlShortenerService.Models
{
    public class ShortLink
    {
        [Key]
        [Required]
        public string LinkKey { get; set; }

        [Required]
        public string OriginalUrl { get; set; }

        [Required]
        public int RedirectsCount { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public DateTime LastRedirect { get; set; }

        public ShortLink()
        {

        }

        public ShortLink(string originalUrl)
        {
            LinkKey = _shortLinkKeyRepo.GetShortLinkKey().LinkKey;
            OriginalUrl = originalUrl;
            CreatedOn = DateTime.UtcNow;
        }

        public ShortLink(ShortLinkKeyRepo shortLinkKeyRepo)
        {
            _shortLinkKeyRepo = shortLinkKeyRepo;
        }

        private ShortLinkKeyRepo _shortLinkKeyRepo;
    }
}
