
using System;
using Hqs.IService.Caches;
using Microsoft.Extensions.Caching.Memory;

namespace Hqs.Service.Caches
{
    public class MemoryCacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;

        public MemoryCacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public void Set(string key, object value, int expireSeconds = -1)
        {
            if (expireSeconds == -1)
            {
                _memoryCache.Set(key, value);
                return;
            }

            _memoryCache.Set(key, value, new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(expireSeconds)));
        }

        public T Get<T>(string key)
        {
            return _memoryCache.Get<T>(key);
        }

        public void Remove(params string[] key)
        {
            foreach (var item in key)
            {
                _memoryCache.Remove(item);
            }
        }
    }
}
