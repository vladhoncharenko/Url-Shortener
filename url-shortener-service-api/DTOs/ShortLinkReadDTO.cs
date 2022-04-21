using System;
using System.ComponentModel.DataAnnotations;

namespace UrlShortenerService.DTOs
{
    public class ShortLinkReadDTO
    {
        [Required]
        [StringLength(300)]
        [Url]
        public string OriginalUrl { get; set; }

        [Required]
        [StringLength(20)]
        [Url]
        public string ShortenedUrl { get; set; }

        [Required]
        public int RedirectsCount { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public DateTime LastRedirect { get; set; }
    }
}