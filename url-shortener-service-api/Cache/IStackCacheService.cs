using System.Collections.Generic;

namespace UrlShortenerService.Cache
{
    public interface IStackCacheService<T>
    {
        public T Get();
        public void Add(IEnumerable<T> values);
    }
}
