using System;
using System.Collections.Generic;
using System.Linq;
using UrlShortenerService.Models;

namespace UrlShortenerService.Data
{
    public class ShortLinkKeyRepo : IShortLinkKeyRepo
    {
        private readonly AppDbContext _context;

        public ShortLinkKeyRepo(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<ShortLinkKey> GetShortLinkKeys(int keysAmount)
        {
            return _context.ShortLinkKeys.ToList();
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() > 0);
        }
    }
}