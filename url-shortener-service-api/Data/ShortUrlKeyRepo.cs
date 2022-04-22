using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UrlShortenerService.Models;

namespace UrlShortenerService.Data
{
    public class ShortUrlKeyRepo : IShortUrlKeyRepo
    {
        private readonly AppDbContext _context;

        public ShortUrlKeyRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddRangeAsync(IEnumerable<ShortUrlKey> shortUrlKeys)
        {
            await _context.ShortUrlKeys.AddRangeAsync(shortUrlKeys);
        }

        public void Delete(DateTime dateTime)
        {
            _context.ShortUrlKeys.RemoveRange(_context.ShortUrlKeys.Where(x => x.IssuedOn >= dateTime));
        }

        public int Count(bool isUsed = false)
        {
            return _context.ShortUrlKeys.Where(k => k.IsUsed == isUsed).Count();
        }

        public IEnumerable<ShortUrlKey> Get(int keysAmount, bool isUsed = false)
        {
            return _context.ShortUrlKeys.Where(k => k.IsUsed == isUsed).Take(keysAmount);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() > 0);
        }
    }
}