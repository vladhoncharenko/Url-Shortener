using System;
using System.ComponentModel.DataAnnotations;

namespace UrlShortenerService.Models
{
    public class ShortUrl
    {
        [Key]
        [Required]
        public string UrlKey { get; set; }

        [Required]
        public string OriginalUrl { get; set; }

        [Required]
        public int RedirectsCount { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public DateTime LastRedirect { get; set; }

        public void RegisterRedirect()
        {
            LastRedirect = DateTime.UtcNow;
            RedirectsCount += 1;
        }
    }
}
