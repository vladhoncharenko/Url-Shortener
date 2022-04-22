
using System.ComponentModel.DataAnnotations;

namespace UrlShortenerService.DTOs
{
    public class ShortUrlCreateDTO
    {
        [Required]
        [StringLength(300)]
        [Url]
        public string OriginalUrl { get; set; }
    }
}