using System;
using System.ComponentModel.DataAnnotations;

namespace UrlShortenerService.Models
{
    public class ShortUrlKey
    {
        [Key]
        [Required]
        public string UrlKey { get; set; }

        [Required]
        public bool IsUsed { get; set; }

        [Required]
        public DateTime IssuedOn { get; set; }
    }
}
