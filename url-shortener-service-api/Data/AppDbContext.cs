using Microsoft.EntityFrameworkCore;
using UrlShortenerService.Models;

namespace UrlShortenerService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<ShortLink> ShortLinks { get; set; }
        public DbSet<ShortLinkKey> ShortLinkKeys { get; set; }
    }
}