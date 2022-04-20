using System;
using System.ComponentModel.DataAnnotations;

namespace UrlShortenerService.DTOs
{
    public class ShortLinkReadDTO
    {
        [Required]
        public string OriginalUrl { get; set; }

        [Required]
        public string ShortenedUrl { get; set; }

        [Required]
        public int RedirectsCount { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public DateTime LastRedirect { get; set; }
    }
}