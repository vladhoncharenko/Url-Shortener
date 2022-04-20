using System;
using System.Collections.Generic;
using System.Linq;
using UrlShortenerService.Models;

namespace UrlShortenerService.Data
{
    public class ShortLinkRepo : IShortLinkRepo
    {
        private readonly AppDbContext _context;

        public ShortLinkRepo(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<ShortLink> GetAllShortenedLinks()
        {
            throw new NotImplementedException();
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() > 0);
        }
    }
}