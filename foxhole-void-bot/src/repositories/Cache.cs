using Microsoft.Extensions.Caching.Memory;
using System;

namespace BeleidsPlanApi.src.database.Repo
{
    public class Cache<T>
    {
        private readonly IMemoryCache _cache;
        public Cache(IMemoryCache cache)
        {
            _cache = cache;
        }
        public T CacheSet(T item, object key)
        {
            var className = typeof(T).Name;  // Get the class name of T

            // Using class name as part of the cache key along with the key
            _cache.Set(GetCacheKey(className, key), item, TimeSpan.FromMinutes(5));

            return item;
        }
        /// <summary>
        /// return true when the corresponding key has been found
        /// </summary>
        /// <param name="key"> key to be used (needs to have a valid toString ovveride!</param>
        /// <param name="value"> value to cache</param>
        /// <returns></returns>
        public bool CacheGet(object key, out T value)
        {
            var className = typeof(T).Name;

            if (_cache.TryGetValue(GetCacheKey(className, key), out value))
            {
                CacheSet(value, key); // Refresh cache with the same item
                return true;
            }
            return false;
        }
        public void CacheInvalidate(object key)
        {
            var className = typeof(T).Name;  // Get the class name of T
            _cache.Remove(GetCacheKey(className, key));
        }
        private string GetCacheKey(string className, object key)
        {
            return $"_{className}_{key}";
        }

    }
}
