using System.Threading.Tasks;
using MassTransit;
using UrlShortenerService.Cache;
using UrlShortenerService.Data;
using UrlShortenerService.Models;

namespace UrlShortenerService.Messaging
{
    public class MessagesConsumer : IConsumer<UrlRedirectMessage>
    {
        private readonly IShortUrlRepo _shortUrlRepo;
        private readonly ICacheService _cacheService;

        public MessagesConsumer(IShortUrlRepo shortUrlRepo, ICacheService cacheService)
        {
            _shortUrlRepo = shortUrlRepo;
            _cacheService = cacheService;
        }

        public async Task Consume(ConsumeContext<UrlRedirectMessage> context)
        {
            var shortUrl = await _shortUrlRepo.RegisterRedirectAsync(context.Message.urlKey);
            await _shortUrlRepo.SaveChangesAsync();
            await _cacheService.SetAsync<ShortUrl>(shortUrl.UrlKey, shortUrl);
        }
    }
}