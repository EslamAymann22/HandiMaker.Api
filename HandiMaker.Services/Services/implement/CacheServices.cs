using HandiMaker.Services.Services.Interface;
using Microsoft.Extensions.Caching.Memory;

namespace HandiMaker.Services.Services.implement
{
    public class CacheServices : ICacheServices
    {
        private readonly IMemoryCache _memoryCache;

        public CacheServices(IMemoryCache memoryCache)
        {
            this._memoryCache = memoryCache;
        }

        public async Task<T> GetCacheAsync<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
                return default;

            T Data = _memoryCache.Get<T>(key);

            return Data;
        }

        public async Task<bool> RemoveCacheAsync(string key)
        {
            if (string.IsNullOrEmpty(key))
                return false;

            _memoryCache.Remove(key);
            return true;

        }

        public async Task<bool> SetCacheAsync<T>(string key, T value, TimeSpan? expirationTime = null)
        {

            if (string.IsNullOrEmpty(key))
                return false;

            _memoryCache.Set(key, value, expirationTime ?? TimeSpan.FromMinutes(5));
            return true;
        }
    }
}
