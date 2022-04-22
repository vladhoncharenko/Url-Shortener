using System.Threading.Tasks;

namespace UrlShortenerService.Cache
{
    public interface ICacheService
    {
        Task<T> GetAsync<T>(string key);
        
        Task<T> SetAsync<T>(string key, T value);
    }
}