namespace HandiMaker.Services.Services.Interface
{
    public interface ICacheServices
    {
        Task<T> GetCacheAsync<T>(string key);
        Task<bool> SetCacheAsync<T>(string key, T value, TimeSpan? expirationTime = null);
        Task<bool> RemoveCacheAsync(string key);

    }
}
