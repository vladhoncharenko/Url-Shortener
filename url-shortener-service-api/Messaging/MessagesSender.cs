using System.Threading.Tasks;
using MassTransit;

namespace UrlShortenerService.Messaging
{
    public class MessagesSender<T> : IMessagesSender<T>
    {
        private readonly IBus _bus;

        public MessagesSender(IBus bus)
        {
            _bus = bus;
        }

        public async Task SendMessageAsync(T message)
        {
            await _bus.Send(message);
        }
    }
}