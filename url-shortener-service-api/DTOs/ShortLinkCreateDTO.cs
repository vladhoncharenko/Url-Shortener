
using System.ComponentModel.DataAnnotations;

namespace UrlShortenerService.DTOs
{
    public class ShortLinkCreateDTO
    {
        [Required]
        public string OriginalUrl { get; set; }
    }
}