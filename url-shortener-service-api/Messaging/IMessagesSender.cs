using System.Threading.Tasks;

namespace UrlShortenerService.Messaging
{
    public interface IMessagesSender<T>
    {
        Task SendMessageAsync(T message);
    }
}