using System.ComponentModel.DataAnnotations;

namespace UrlShortenerService.Models
{
    public class ShortLinkKey
    {
        [Key]
        [Required]
        public string LinkKey { get; set; }

        [Required]
        public bool IsUsed { get; set; } = true;
    }
}
