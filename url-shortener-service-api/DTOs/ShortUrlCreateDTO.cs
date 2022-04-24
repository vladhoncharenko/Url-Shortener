
using System.ComponentModel.DataAnnotations;

namespace UrlShortenerService.DTOs
{
    public class ShortUrlCreateDTO
    {
        [Required]
        [StringLength(2000)]
        [Url]
        public string OriginalUrl { get; set; }
    }
}