using System.Collections.Generic;

namespace UrlShortenerService.Cache
{
    public class StackCacheService<T> : IStackCacheService<T>
    {
        private Stack<T> stack = new Stack<T>() { };

        public StackCacheService()
        {
        }

        public T Get()
        {
            return stack.Count > 0 ? stack.Pop() : default;
        }

        public void Add(IEnumerable<T> values)
        {
            foreach (var value in values)
            {
                stack.Push(value);
            }
        }
    }
}
