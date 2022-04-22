using Microsoft.EntityFrameworkCore;
using UrlShortenerService.Models;

namespace UrlShortenerService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<ShortUrl> ShortUrls { get; set; }

        public DbSet<ShortUrlKey> ShortUrlKeys { get; set; }
    }
}