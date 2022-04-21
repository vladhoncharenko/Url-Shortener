
using System.ComponentModel.DataAnnotations;

namespace UrlShortenerService.DTOs
{
    public class ShortLinkCreateDTO
    {
        [Required]
        [StringLength(300)]
        [Url]
        public string OriginalUrl { get; set; }
    }
}