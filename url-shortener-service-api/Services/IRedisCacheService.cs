using System.Threading.Tasks;

namespace UrlShortenerService.Services
{
    public interface IRedisCacheService
    {
        Task<T> GetAsync<T>(string key);
        Task<T> SetAsync<T>(string key, T value);
    }
}