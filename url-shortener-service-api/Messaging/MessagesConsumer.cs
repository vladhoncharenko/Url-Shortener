using System.Threading.Tasks;
using MassTransit;
using UrlShortenerService.Cache;
using UrlShortenerService.Data;
using UrlShortenerService.Models;

namespace UrlShortenerService.Messaging
{
    public class MessagesConsumer : IConsumer<UrlRedirectMessage>
    {
        public IShortUrlRepo _shortUrlRepo;
        public ICacheService _cacheService;

        public MessagesConsumer(IShortUrlRepo shortUrlRepo, ICacheService cacheService)
        {
            _shortUrlRepo = shortUrlRepo;
            _cacheService = cacheService;
        }

        public async Task Consume(ConsumeContext<UrlRedirectMessage> context)
        {
            var shortUrl = _shortUrlRepo.RegisterRedirect(context.Message.urlKey);
            await _shortUrlRepo.SaveChangesAsync();
            await _cacheService.SetAsync<ShortUrl>(shortUrl.UrlKey, shortUrl);
        }
    }
}